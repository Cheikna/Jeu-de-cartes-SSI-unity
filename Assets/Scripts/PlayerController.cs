using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    List<GameObject> playersInGame;
    public bool isItMyTurn { get; set; }
    public bool isItMyTurn2;
    public bool isGameOver { get; set; }
    bool doItOnce = false;
    int indexPlayerWhoPlays = 0;

    [SerializeField]
    private Canvas isItMyTurnCanvas;
    [SerializeField]
    private Text isItMyTurnText;
    [SerializeField]
    private Button confirmCardButton;

    [SerializeField]
    private GameObject virusPrefab;
    [SerializeField]
    private Transform virusSpawn;
    [SerializeField]
    private GameObject playerElements;

    #region Bouton & Canvas
    [SerializeField]
    private Canvas showMyCardsCanvas;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas healthCanvas;
    [SerializeField]
    private Canvas chooseCharacterCanvas;
    [SerializeField]
    private Component femaleCharacter;
    [SerializeField]
    private Component maleCharacter;
    private Button femaleCharacterButton;
    private Button maleCharacterButton;
    #endregion


    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            playersInGame = new List<GameObject>();
            //playersInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));
        }
            
        if(isLocalPlayer)
        {
            //permet d'afficher uniquement ceux qui appartient au joueur local
            playerElements.SetActive(true);
            isItMyTurn = false;
            isGameOver = false;

            Button[] playerButtons = GetComponentsInChildren<Button>();
            // Trouver tous les boutons du joueur
            foreach (Button btn in playerButtons)
            {
                string tag = btn.tag;

                switch (tag)
                {
                    case "femaleCharacterButton":
                        femaleCharacterButton = btn;
                        femaleCharacterButton.onClick.AddListener(delegate { CmdChooseCharacter("female"); });
                        break;

                    case "maleCharacterButton":
                        maleCharacterButton = btn;
                        maleCharacterButton.onClick.AddListener(delegate { CmdChooseCharacter("male"); });
                        break;
                }
            }

            showMyCardsCanvas.gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            chooseCharacterCanvas.gameObject.SetActive(true);
            healthCanvas.gameObject.SetActive(false);
            isItMyTurnCanvas.gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        isItMyTurn2 = isItMyTurn;
        //TODO add a blinking effect texte (effet clignotement)
        if (isItMyTurn)
        {
            isItMyTurnText.text = "A votre tour de jouer";
            confirmCardButton.interactable = true;
        }
        else
        {
            isItMyTurnText.text = "Au tour de l'adversaire !";
            confirmCardButton.interactable = false;

        }

    }

    [Command]
    public void CmdNextPlayerToPlay()
    {
        RpcServerChoosesWhoPlays();
    }

    [ClientRpc]
    public void RpcServerChoosesWhoPlays()
    {
        if(!doItOnce)
        {
            playersInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));
            doItOnce = true;

        }
        //if(isServer)
        // {
        if (indexPlayerWhoPlays >= playersInGame.Count)
            indexPlayerWhoPlays = 0;
        foreach(GameObject player in playersInGame)
        {
            player.GetComponent<PlayerController>().isItMyTurn = false;
        }
        Debug.Log("Numéro du joueur qui doit jouer : " + indexPlayerWhoPlays);
        playersInGame[indexPlayerWhoPlays].GetComponent<PlayerController>().isItMyTurn = true;
        indexPlayerWhoPlays += 1;
        //}
    }

    //TODO : remplacer les paramètres de cette méthode par des cartes
    public void shootFromCardsDeckClass(ComputerLayer targetLayer1, int damage1/*, ComputerLayer targetLayer2, int damage2*/)
    {
        if (!isLocalPlayer)
            return;

        CmdFire(targetLayer1, damage1);
    }


    [Command] //TODO : remplacer les paramètres de cette méthode par des cartes et vérifier dans la méthodes si ce sont des cartes d'attaques
    public void CmdFire(ComputerLayer targetLayer1, int damage1/*, ComputerLayer targetLayer2, int damage2*/)
    {
        var virus = (GameObject)Instantiate(virusPrefab, virusSpawn.position, virusSpawn.rotation);
        var virusEffects = virus.GetComponent<VirusController>();
        virusEffects.targetLayer1 = targetLayer1;
        virusEffects.damageLayer1 = damage1;

        /*if (damage2 != 0)
        {
            virusEffects.targetLayer2 = targetLayer2;
            virusEffects.damageLayer2 = damage2;
        }*/
        virus.GetComponent<Rigidbody>().velocity = virus.transform.forward * 6;

        //faire apparaitre la balle sur les clients
        NetworkServer.Spawn(virus);
        Destroy(virus, 5.0f);
    }

    [Command]
    public void CmdChooseCharacter(string character)
    {
        RpcUpdateChooseCharacter(character);

    }

    [ClientRpc]
    public void RpcUpdateChooseCharacter(string character)
    {
        /*if (!isLocalPlayer)
            return;*/

        if (character == "female")
        {
            maleCharacter.gameObject.SetActive(false);
            femaleCharacter.gameObject.SetActive(true);
        }
        else if (character == "male")
        {
            maleCharacter.gameObject.SetActive(true);
            femaleCharacter.gameObject.SetActive(false);
        }


        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        chooseCharacterCanvas.gameObject.SetActive(false);
        healthCanvas.gameObject.SetActive(true);
        isItMyTurnCanvas.gameObject.SetActive(true);
        CmdNextPlayerToPlay();
    }

    public void hideMyCards()
    {
        if (!isLocalPlayer)
            return;

        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    public void showMyCards()
    {
        if (!isLocalPlayer)
            return;

        showMyCardsCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
    }


}
