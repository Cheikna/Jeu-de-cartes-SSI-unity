using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

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
        if(isLocalPlayer)
        {
            //permet d'afficher uniquement ceux qui appartient au joueur local
            playerElements.SetActive(true);

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

            // TODO faire en sorte d'afficher uniquement le personnage des adversaires sans les autres éléments comme les canvas

            /*Component[] playerComponent = GetComponentsInChildren<Component>();
            foreach(Component comp in playerComponent)
            {
                Debug.Log("component : " + comp.tag);
                Debug.Log("component : " + comp.name);
            }*/


        }
    }

    public override void OnStartLocalPlayer()
    {
        Button[] playerButtons = GetComponentsInChildren<Button>();
        Canvas[] playerCanvas = GetComponentsInChildren<Canvas>();
        Component[] playerCharacters = GetComponentsInChildren<Component>();


        // Trouver tous les boutons du joueur
        /*foreach (Button btn in playerButtons)
        {
            string tag = btn.tag;

            switch(tag)
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
        }*/

        // Trouver tous les canvas du joueur
        /*foreach (Canvas canvas in playerCanvas)
        {            
            switch (canvas.tag)
            {
                case "healthCanvas":
                    healthCanvas = canvas;
                    break;

                case "mainCanvas":
                    mainCanvas = canvas;
                    break;

                case "myCardsCanvas":
                    showMyCardsCanvas = canvas;
                    break;

                case "chooseCharacterCanvas":
                    chooseCharacterCanvas = canvas;
                    break;
            }


        }*/

        //Trouver les différents personnages
        /*foreach(Component component in playerCharacters)
        {
            Debug.Log("personnage " + component.gameObject.tag);
            switch(component.gameObject.tag)
            {
                
                case "regina":
                    femaleCharacter = component;
                    break;

                case "boss":
                    maleCharacter = component;
                    break;
            }
        }

        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        chooseCharacterCanvas.gameObject.SetActive(true);
        healthCanvas.gameObject.SetActive(false);*/

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        /*Component[] components = GetComponentsInChildren<Component>();
        foreach(Component comp in components)
        {
            comp.gameObject.SetActive(false);
        }*/

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            //CmdFire();
            shootFromCardsDeckClass();

        }*/

    }

    public void shootFromCardsDeckClass(ComputerLayer targetLayer1, int damage1/*, ComputerLayer targetLayer2, int damage2*/)
    {
        if (!isLocalPlayer)
            return;

        CmdFire(targetLayer1, damage1);
    }


    [Command]
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
