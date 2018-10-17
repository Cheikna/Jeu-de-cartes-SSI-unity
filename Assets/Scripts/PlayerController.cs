using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

    List<GameObject> playersInGame;
    [SerializeField]
    private GameObject arbiterControllerGameObject;
    ArbiterController arbiter;
    public bool isGameOver { get; set; }
    public bool areAllCardsDistributed { get; set; }
    bool areAllPlayersHere = false;
    int indexPlayerWhoPlays = 0;
    List<Card> myCardsDeck = new List<Card>();
    [SerializeField]
    TemporaryCardsDeck tempCardsDeck;
    
    public int numberOfPlayersInTheGame { get; set; }
    [SerializeField]
    private TurnsFollower turnsFollower;
    [SerializeField]
    private CardsDeck cardsDeckScriptFromThePlayer;
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


    [SyncVar]
    private bool isItMyTurnHook = false;
    [SyncVar]
    public string playerName = "";
    [SyncVar]
    private bool isWhiteHat = false;
    [SyncVar]
    private int positionX =0;
    [SyncVar]
    private int positionY = 0;
    [SyncVar]
    private int positionZ = 0;

    IEnumerator WaitFewSecondsBeforeDistribuingCards()
    {
        yield return new WaitForSeconds(1.5f);

        if(hasAuthority)
            CmdCardDistributionFromPlayer();

        turnsFollower.gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        areAllCardsDistributed = false;
        numberOfPlayersInTheGame = Prototype.NetworkLobby.LobbyPlayerList.numberOfPlayerInTheRoom;
        playersInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));
        if (isServer)
        {
            StartCoroutine(WaitFewSecondsBeforeDistribuingCards());
        }
            
        if(isLocalPlayer)
        {
            //permet d'afficher uniquement ceux qui appartient au joueur local
            playerElements.SetActive(true);

            isGameOver = false;

            addPossibilityToChooseCharacterButtons();
            deactivateAllUselessCanvasForTheBeginning();
        }
        
    }

    public void onChangeIsItMyTurnHook()
    {

    }

    public void setIsItMyTurnHook(bool isItMyTurnHook)
    {
        if (!isServer)
            return;
        this.isItMyTurnHook = isItMyTurnHook;
    }

    public bool getIsItMyTurnHook()
    {
        return isItMyTurnHook;
    }

    void addPossibilityToChooseCharacterButtons()
    {
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
    }

    void deactivateAllUselessCanvasForTheBeginning()
    {
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        chooseCharacterCanvas.gameObject.SetActive(true);
        healthCanvas.gameObject.SetActive(false);
        isItMyTurnCanvas.gameObject.SetActive(false);
    }

    public void addCardToMyDeck(Card card)
    {
        if(isLocalPlayer)
        {
            if (card != null)
                tempCardsDeck.addCard(card);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (isItMyTurnHook)
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

    void getAllOfThePlayersInAList()
    {        
        
        if (playersInGame.Count != numberOfPlayersInTheGame)
            playersInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));

        arbiter = arbiterControllerGameObject.GetComponent<ArbiterController>();
        arbiter.numberOfPlayersInTheGame = Prototype.NetworkLobby.LobbyPlayerList.numberOfPlayerInTheRoom;
        arbiter.distributeCards(playersInGame);

        areAllPlayersHere = true;
    }

    [Command]
    public void CmdCardDistributionFromPlayer()
    {
        RpcServerDistributeCards();
    }

    [ClientRpc]
    public void RpcServerDistributeCards()
    {
        getAllOfThePlayersInAList();
    }


    public void shootFromCardsDeckClass(string[] cardPlayedInfos)
    {
        if (!isLocalPlayer)
            return;

        //L'attribut card n'est pas passé en paramètre car cela engendre des problèmes lorsque que l'on tente d'instancier des balles sur les clients
        //De plus, on ne peut passer des objets (ex: Card) en paramètre pour les fonctions Command (s'éxécutant sur les clients)
        CmdFire(cardPlayedInfos);
    }


    [Command]
    public void CmdFire(string[] cardPlayedInfos)
    {

        bool isAttakCard = (cardPlayedInfos[3].ToUpper() == "TRUE") ? true : false;
        ComputerLayer touchedLayer = ComputerLayer.HARDWARE;
        Debug.Log("int touched lay String : " + cardPlayedInfos[4]);
        string touchedLayerString = cardPlayedInfos[4].ToUpper();
        if (touchedLayerString == "SOFTWARE")
            touchedLayer = ComputerLayer.SOFTWARE;
        else if (touchedLayerString == "OS")
            touchedLayer = ComputerLayer.OS;


        Card cardPlayedByThePlayer = new Card(cardPlayedInfos[0],
                                              cardPlayedInfos[1],
                                              cardPlayedInfos[2],
                                              isAttakCard,
                                              touchedLayer,
                                              int.Parse(cardPlayedInfos[5]),
                                              new Color(int.Parse(cardPlayedInfos[6]), int.Parse(cardPlayedInfos[7]), int.Parse(cardPlayedInfos[8]))                                              
                                              );


        if (cardPlayedByThePlayer.isAttackCard)
        {
            var virus = (GameObject)Instantiate(virusPrefab, virusSpawn.position, virusSpawn.rotation);
            var virusEffects = virus.GetComponent<VirusController>();
            virusEffects.targetLayer1 = cardPlayedByThePlayer.touchedLayer;
            virusEffects.damageLayer1 = cardPlayedByThePlayer.getDamage();
            virusEffects.playerWhoFiredTheVirus = this;

            //Changer la couleur de la balle pour qu'elle soit de la même couleur que la carte
            Renderer[] rends = virus.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.material.color = cardPlayedByThePlayer.getCardColor();
            
            virus.GetComponent<Rigidbody>().velocity = virus.transform.forward * 6;

            //faire apparaitre la balle sur les clients
            NetworkServer.Spawn(virus);
            //Faire disparaitre la balle au bout de 5sec pour éviter une surcharge des ressources
            Destroy(virus, 5.0f);

        }
        // Si la carte n'est pas une carte d'attaque alors c'est une carte qui nous rajoute des points
        else if (!cardPlayedByThePlayer.isAttackCard)
        {
            GetComponent<ComputerHealth>().setHealth(cardPlayedByThePlayer.touchedLayer, cardPlayedByThePlayer.getDamage(), false);
        }
    }

    [Command]
    public void CmdChooseCharacter(string character)
    {
        RpcUpdateChooseCharacter(character);

    }

    [ClientRpc]
    public void RpcUpdateChooseCharacter(string character)
    {
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


    public string getPlayerName()
    {
        return playerName;
    }

    public void setPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    public int getPositionX()
    {
        return positionX;
    }

    public void setPositionX(int x)
    {
        this.positionX = x;
    }

    public int getPositionY()
    {
        return positionY;
    }

    public void setPositionY(int y)
    {
        this.positionY = y;
    }

    public int getPositionZ()
    {
        return positionZ;
    }

    public void setPositionZ(int z)
    {
        this.positionZ = z;
    }

}
