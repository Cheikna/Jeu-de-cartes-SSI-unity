using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlayerController : NetworkBehaviour
{

    List<GameObject> playersInGame = new List<GameObject>();
    ArbiterController arbiter;
    bool areAllPlayersHere = false;
    int indexPlayerWhoPlays = 0;
    List<Card> myCardsDeck = new List<Card>();    
    const int minNumberOfPlayersPerTeam = 1;
    int numberOfPeopleInTeam1 = 0;
    int numberOfPeopleInTeam2 = 0;



    #region variables getters;setters
    public bool isGameOver { get; set; }
    public bool areAllCardsDistributed { get; set; }
    public int numberOfPlayersInTheGame { get; set; }
    public string team1Members { get; set; }
    public string team2members { get; set; }
    #endregion

    #region SerializeField - permet d'attribuer des valeurs à ces atttributs via Unity directement
    [SerializeField]
    private GameObject arbiterControllerGameObject;
    [SerializeField]
    TemporaryCardsDeck tempCardsDeck;    
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
    [SerializeField]
    private Text team1MembersText;
    [SerializeField]
    private Text team2MembersText;
    [SerializeField]
    private Transform positionCenterTeam1;
    [SerializeField]
    private Transform positionCenterTeam2;
    [SerializeField]
    private Transform position1;
    [SerializeField]
    private Transform position2;
    [SerializeField]
    private Transform position3;
    [SerializeField]
    private Transform position4;
    #endregion

    #region Bouton & Canvas [SerializedField]
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
    [SerializeField]
    private Canvas chooseTeamCanvas;
    #endregion

    #region Boutons auxquels on doit rajouter des fonctionnalités
    private Button femaleCharacterButton;
    private Button maleCharacterButton;
    private Button chooseTeam1Button;
    private Button chooseTeam2Button;
    private Button teamChoosedButton;
    #endregion

    #region SynVars
    [SyncVar]
    private bool isItMyTurnHook = false;
    [SyncVar]
    private string playerName = "";
    [SyncVar]
    private bool isWhiteHat = false;
    [SyncVar]
    string hatColor = "";
    [SyncVar]
    private int teamNumber = 0;
    [SyncVar]
    private Transform myPosition;
    private char regexTeam = '>';
    private char regexNewPlayer = '&';
    // teamsMembers = "name1" + regexTeam  + "team1" + regexNewPlayer + "name2" + regexTeam  + "team2" ;
    [SyncVar]    
    public string teamsMembers = "";
    [SyncVar]
    bool teamChoosed = false;
    #endregion

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

            addPossibilityToChooseTeamButtons();
            deactivateAllUselessCanvasForTheBeginning();

            /*if(teamChanged)
            {
                updateTeamMembersText();
                teamChanged = false;
            }*/
        }
        
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

    void addPossibilityToChooseTeamButtons()
    {
        Button[] playerButtons = GetComponentsInChildren<Button>();
        // Trouver tous les boutons actifs du joueur
        foreach (Button btn in playerButtons)
        {
            string tag = btn.tag;

            switch (tag)
            {
                case "chooseTeam1Button":
                    chooseTeam1Button = btn;
                    chooseTeam1Button.onClick.AddListener(delegate { CmdSetTeamNumber(1); });
                    break;

                case "chooseTeam2Button":
                    chooseTeam2Button = btn;
                    chooseTeam2Button.onClick.AddListener(delegate { CmdSetTeamNumber(2); });
                    break;

                case "teamChoosedButton":
                    teamChoosedButton = btn;
                    teamChoosedButton.onClick.AddListener(delegate { CmdTeamChoosed(); });
                    teamChoosedButton.interactable = false;
                    break;
            }
        }
    }

    void deactivateAllUselessCanvasForTheBeginning()
    {
        chooseTeamCanvas.gameObject.SetActive(true);
        chooseCharacterCanvas.gameObject.SetActive(false);
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
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
        if (isServer)
        {
            if(!teamChoosed)
                updateTeamMembersText();

        }
            

        if (!isLocalPlayer)
            return;

        if(!teamChoosed)
            setTeamsMembersTexts();

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
    public void CmdTeamChoosed()
    {
        RpcTeamChoosed();
    }

    [ClientRpc]
    public void RpcTeamChoosed()
    {
        setMyPosition();
        chooseTeamCanvas.gameObject.SetActive(false);
        chooseCharacterCanvas.gameObject.SetActive(true);
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        healthCanvas.gameObject.SetActive(false);
        isItMyTurnCanvas.gameObject.SetActive(false);
        teamChoosed = true;

        // Possibilité de choisir un joueur après avoir choisit son équipe
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

    public void setMyPosition()
    {
        //Pour connaitre notre equipe, on peut utiliser : teamsMembers = "name1" + regexTeam  + "team1" + regexNewPlayer + "name2" + regexTeam  + "team2" ;
        /* On split au niveau du regexNewPlayer, on regarde la taille et si la taille est de 2 alors le premier joueur est de l'équipe 1
         *  et le deuxième joueur est de l'équipe 2. Sinon si la taille est de 4 alors les deux premiers joueurs sont de l'équipe 1
         *  et les deux derniers de l'équipe 2. On peut l'affirmer car les joueurs sont triés en fonction de leurs équipes.
         *  Enfin pour chaque case on verifie si elle contient le nom du joueur et si c'est le cas l'index de la case (+1 car les index commencent à 0)
         *  devient le numéro de la position du joueur
         */

        string[] playersWithTeams = teamsMembers.Split(regexNewPlayer);
        // On fait -1 car à la fin il reste un regexnewPlayer tout seul
        int arrayLength = playersWithTeams.Length - 1;

        Debug.Log("array length l.391 : " + arrayLength);
        Transform myFuturePosition = null;


        //Si on est de l'équipe 1, nos positions possible sont position1 et position2
        //Si on est de l'équipe 2, nos positions possible sont position3 et position4
        if (arrayLength <= 2)
        {
            if (teamNumber == 1)
                myFuturePosition = positionCenterTeam1;
            else if (teamNumber == 2)
                myFuturePosition = positionCenterTeam2;
        }
        else // si c'est égale à 4
        {
            if(teamNumber == 1)
            {
                //Les cases 0 et 1 pour l'équipe 1 et les cases 2 et 3 pour l'équipe 2
                if (playersWithTeams[0].Contains(playerName))
                    myFuturePosition = position1;
                else
                    myFuturePosition = position2;
            }
            else if (teamNumber == 2)
            {
                if (playersWithTeams[2].Contains(playerName))
                    myFuturePosition = position3;
                else
                    myFuturePosition = position4;

            }
        }

        if(myFuturePosition != null)
        {
            transform.position = myFuturePosition.position;
            transform.rotation = myFuturePosition.rotation;
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

    public int getTeamNumber()
    {
        return teamNumber;
    }

    [Command]
    public void CmdSetTeamNumber(int teamNumber)
    {
        RpcSetTeamNumber(teamNumber);
    }


    public void setTeamNumber(string teamNumberString)
    {
        if(isServer)
            teamNumber = int.Parse(teamNumberString);

        if (!isLocalPlayer)
            return;
        teamNumber = int.Parse(teamNumberString);
    }

    [ClientRpc]
    public void RpcSetTeamNumber(int teamNumber)
    {
        this.teamNumber = teamNumber;       

    }

    public void updateTeamMembersText()
    {
        
        List<PlayerController> players = new List<PlayerController>();
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                players.Add(p);
            }
        }
        
        string playersRepartitionInTeams = "";
        // Ces 2 variables permettront d'avoir en sortie d'abord les joueurs de lé'équipe 1 puis ceux de l'équipe 2
        string playersInTeam1 = "";
        string playersInTeam2 = "";
        int team = 0;

        foreach (PlayerController player in players)
        {
            team = player.getTeamNumber();
            if(team == 1)
                playersInTeam1 += player.getPlayerName() + regexTeam + team + regexNewPlayer;
            else if(team == 2)
                playersInTeam2 += player.getPlayerName() + regexTeam + team + regexNewPlayer;
        }

        playersRepartitionInTeams += playersInTeam1 + playersInTeam2;

        foreach (PlayerController player in players)
        {
            player.setTeamsMembers(playersRepartitionInTeams);
        }
    }

    public void setTeamsMembersTexts()
    {
        // teamsMembers = "name1" + regexTeam  + "team1" + regexNewPlayer + "name2" + regexTeam  + "team2" + regexNewPlayer ;
        string playersInTeam1 = "";
        string playersInTeam2 = "";
        numberOfPeopleInTeam1 = 0;
        numberOfPeopleInTeam2 = 0;

        string[] playersWithTeams = teamsMembers.Split(regexNewPlayer);
        foreach(string playerWithTeam in playersWithTeams)
        {
            if(playersWithTeams != null && playersWithTeams.Length > 1)
            {
                string[] playerWithTeamSplit = playerWithTeam.Split(regexTeam);
                if(playerWithTeamSplit.Length >= 2)
                {
                    string playerName = playerWithTeamSplit[0];
                    string team = playerWithTeamSplit[1];
                    if (team == "1")
                    {
                        playersInTeam1 += "- " + playerName + "\n";
                        numberOfPeopleInTeam1++;
                    }
                    else if (team == "2")
                    {
                        playersInTeam2 += "- " + playerName + "\n";
                        numberOfPeopleInTeam2++;
                    }

                }
                
            }
        }

        if (numberOfPeopleInTeam1 >= minNumberOfPlayersPerTeam && numberOfPeopleInTeam1 == numberOfPeopleInTeam2)
            teamChoosedButton.interactable = true;
        else
            teamChoosedButton.interactable = false;

        team1MembersText.text = playersInTeam1;
        team2MembersText.text = playersInTeam2;
    }



    public string getHatColor()
    {
        return hatColor;
    }

    [Command]
    public void CmdSetHatColor(string hatColor)
    {
        RpcSetHatColor(hatColor);
    }

    [ClientRpc]
    public void RpcSetHatColor(string hatColor)
    {
        this.hatColor = hatColor;

    }

    public string getTeamsMembers()
    {
        return teamsMembers;
    }

    public void setTeamsMembers(string teamsMembers)
    {
        this.teamsMembers = teamsMembers;
    }

    [Command]
    public void CmdActivateIsMyTurnHookOfPeopleofMyTeam()
    {
        RpcActivateIsMyTurnHookOfPeopleofMyTeam();
    }

    [ClientRpc]
    public void RpcActivateIsMyTurnHookOfPeopleofMyTeam()
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                if (p.getTeamNumber() == teamNumber)
                    p.setIsItMyTurnHook(true);
            }
        }
        
    }
}
