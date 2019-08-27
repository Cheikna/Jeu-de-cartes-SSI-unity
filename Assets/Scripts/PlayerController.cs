using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking.NetworkSystem;
using System;

public class PlayerController : NetworkBehaviour
{

    List<GameObject> playersInGame = new List<GameObject>();
    bool areAllPlayersHere = false;
    int indexPlayerWhoPlays = 0;
    List<Card> myCardsDeck = new List<Card>();
    int numberOfPeopleInTeam1 = 0;
    int numberOfPeopleInTeam2 = 0;
    int numberOfPlayersInLobby = 0;
    public bool testMode = true;
    Dictionary<NetworkInstanceId, NetworkIdentity> playersInTeam1Dico;
    Dictionary<NetworkInstanceId, NetworkIdentity> playersInTeam2Dico;
    private bool hasGameOverScreenBeenDisplayed = false;
    private ComputerHealth myHealth;

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
    //[SerializeField]
    //private TurnsFollower turnsFollower;
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
    [SerializeField]
    private Text numberOfTurnsText;
    #endregion

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
    [SerializeField]
    private Canvas chooseTeamCanvas;
    [SerializeField]
    private Canvas numberOfTurnsCanvas;
    #endregion

    #region Boutons auxquels on doit rajouter des fonctionnalités
    private Button femaleCharacterButton;
    private Button maleCharacterButton;
    #endregion


    [SyncVar]
    private string playerName = "";
    [SyncVar]
    private bool isWhiteHat = false;
    [SyncVar]
    string hatColor = "";
    [SyncVar]
    public int teamNumber = 0;
    [SyncVar]
    private Transform myPosition;
    private char regexTeam = '>';
    private char regexNewPlayer = '&';
    // teamsMembers = "name1" + regexTeam  + "team1" + regexNewPlayer + "name2" + regexTeam  + "team2" ;
    [SyncVar]
    public string teamsMembers = "";
    [SyncVar]
    bool teamChoosed = false;
    [SyncVar]
    bool characterChoosed = false;
    //Permet de savoir l'ordre du joueur et si l'ordre est égal à 1, alors c'est ce joueur qui incrémentera le nombre de tours
    [SyncVar]
    int playerOrder = 0;
    [SyncVar]
    int numberOfTurns = 1;
    const int maxNumberOfTurns = (int)Constants.MAX_NUMBER_OF_TURNS;
    [SyncVar]
    public int numberOfBallsFired = 0; // Lorsque 2 balles sont lancées (c'est-à-dire que les 2 équipes ont lancé chacun une balle) alors cela signifie qu'il y a eu un tour
    [SyncVar]
    int numberOfBallsFiredByMyTeam = (int)Constants.NUMBER_BASE_OF_CARDS_PLAYED_BY_EACH_TEAM;
    [SyncVar]
    string allTeamMateCardsString = "";
    [SyncVar]
    public int id;
    [SyncVar]
    bool allPlayersHaveChosenTheirCharacters = false;
    [SyncVar]
    bool playersCanStartToPlay = false;
    [SyncVar]
    int firstTeamWhichPlays = 1;
    [SyncVar]
    public bool isItMyTurn = false;
    [SyncVar]
    public bool isGameEnded = false;
    [SyncVar]
    bool hasPlayerBeenNotifiedForGameOver = false;
    [SyncVar]
    private int currentShooterTeamNumber = 1;
    #endregion

    IEnumerator WaitFewSecondsBeforeDistribuingCards()
    {
        yield return new WaitForSeconds(1.5f);
        //turnsFollower.gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        // On doit attendre que tous les joueurs soient prêts avant de pouvoir lancer une carte
        confirmCardButton.interactable = false;
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

            //Initialisation du deck de cartes du joueurs local
            cardsDeckScriptFromThePlayer.loadCards();
            myHealth = GetComponent<ComputerHealth>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (isServer)
            {
                RpcGetNumberOfSpawnedVirus();

                if (!teamChoosed)
                    updateTeamMembersText();
                if (!allPlayersHaveChosenTheirCharacters)
                    allPlayersHaveChosenTheirCharacters = haveAllPlayerChosenTheirCharacters();

                if (numberOfBallsFired >= 2)
                {
                    RpcIncreaseNumberOfTurns();
                    // Changer l'équipe qui doit jouer
                    currentShooterTeamNumber = (currentShooterTeamNumber == 1) ? 2 : 1;
                    RpcSetTeamWhichPlays(currentShooterTeamNumber);
                }

            }

            if (!isLocalPlayer)
                return;

            numberOfTurnsText.text = "Tour : " + numberOfTurns + " / " + maxNumberOfTurns;

            if (!teamChoosed)
                setTeamsMembersTexts();

            if (allPlayersHaveChosenTheirCharacters)
            {
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

                if(numberOfTurns > maxNumberOfTurns)
                {
                    //TODO
                }

                //Notifier tous les joueurs de la partie que l'on a perdu
                // La deuxième condition permet de notifier les joueurs une unique fois
                if (myHealth.getRemainingLife() <= 0 && !hasPlayerBeenNotifiedForGameOver)
                {
                    CmdSetGameOverForAllPlayers();
                }

                // Fin de la partie et on affiche le résultat : Gagné ou perdu
                if(isGameEnded && !hasGameOverScreenBeenDisplayed)
                {
                    hasGameOverScreenBeenDisplayed = true;
                    myHealth.gameOver();
                }
            }
            else
            {
                isItMyTurnText.text = "En attente des autres joueurs...";
            }

        }
        catch(Exception e)
        {
            Debug.Log("Une exception a été lancée : " + e);
        }

    }

    public void setIsItMyTurn(bool isItMyTurn)
    {
        if(isServer)
            this.isItMyTurn = isItMyTurn;

        if (!isLocalPlayer)
            return;

        this.isItMyTurn = isItMyTurn;

    }

    public bool getIsItMyTurn()
    {
        return isItMyTurn;
    }

    /*[Command]
    public void CmdSetIsItMyTurnForAllMyTeam(int teamNumber, bool isItMyTurn)
    {
        RpcSetIsItMyTurnForAllMyTeam(teamNumber, isItMyTurn);
    }*/

    /*[ClientRpc]
    public void RpcSetIsItMyTurnForAllMyTeam(int teamNumber, bool isItMyTurn)
    {*/
        /**
         * Les joueurs de l'équipe en paramètre peuvent jouer ou ne plus jouer en fonction du paramètre isItMyTurn
         * Et les joueurs de l'équipe adverse ont donc le choix inverse
         */
        /*if(teamNumber == 1)
        {
            foreach (var pair in playersInTeam1Dico)
                pair.Value.gameObject.GetComponent<PlayerController>().setIsItMyTurn(isItMyTurn);

            foreach (var pair in playersInTeam2Dico)
                pair.Value.gameObject.GetComponent<PlayerController>().setIsItMyTurn(!isItMyTurn);
        }
        else if(teamNumber == 2)
        {
            foreach (var pair in playersInTeam2Dico)
                pair.Value.gameObject.GetComponent<PlayerController>().setIsItMyTurn(isItMyTurn);

            foreach (var pair in playersInTeam1Dico)
                pair.Value.gameObject.GetComponent<PlayerController>().setIsItMyTurn(!isItMyTurn);
        }
    }*/

    void addPossibilityToChooseTeamButtons()
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
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
        chooseCharacterCanvas.gameObject.SetActive(true);
        healthCanvas.gameObject.SetActive(false);
        numberOfTurnsCanvas.gameObject.SetActive(false);
        isItMyTurnCanvas.gameObject.SetActive(false);
    }


    void getAllOfThePlayersInAList()
    {
        // Vérifie si on n'a bien récupérer tous les joueurs en comparant le nombre de joueurs du lobby et la liste où on a récupéré les joueurs
        if (playersInGame.Count != numberOfPlayersInTheGame)
            playersInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));

        //arbiter = arbiterControllerGameObject.GetComponent<ArbiterController>();
        //TODO nombre de joueurs à récupérer quelques part
        numberOfPlayersInLobby = Prototype.NetworkLobby.LobbyPlayerList.numberOfPlayerInTheRoom;
        //arbiter.distributeCards(playersInGame);

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
        CmdIncreaseNumberOfBallsFired();
    }


    [Command]
    public void CmdFire(string[] cardPlayedInfos)
    {
        bool isAttakCard = (cardPlayedInfos[3].ToUpper() == "TRUE");
        ComputerLayer touchedLayer = ComputerLayer.HARDWARE;
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
            myHealth.setHealth(cardPlayedByThePlayer.touchedLayer, cardPlayedByThePlayer.getDamage(), false);
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
        numberOfTurnsCanvas.gameObject.SetActive(false);
        isItMyTurnCanvas.gameObject.SetActive(false);
        teamChoosed = true;
        if (teamNumber == firstTeamWhichPlays)
            isItMyTurn = true;

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

    [Command]
    public void CmdSetPlayersDicos()
    {
        RpcSetPlayersDicos();
    }

    [ClientRpc]
    private void RpcSetPlayersDicos()
    {
        //Set the team dictionnary
        playersInTeam1Dico = NetworkServer.objects;
        playersInTeam2Dico = NetworkServer.objects;
        List<NetworkInstanceId> instanceToDeleteInDicoTeam1 = new List<NetworkInstanceId>();
        List<NetworkInstanceId> instanceToDeleteInDicoTeam2 = new List<NetworkInstanceId>();

        //Mise à jour du dictionnaire des joueurs de l'équipe 1, pour qu'il n'y ait que des joueurs de l'équipe 1
        foreach (var pair in playersInTeam1Dico)
        {
            Debug.Log(pair.Value);
            if (pair.Value.name == "Player(Clone)")
            {
                int dictionnaryPlayerTeamNumber = pair.Value.gameObject.GetComponent<PlayerController>().getTeamNumber();

                if (dictionnaryPlayerTeamNumber != 1)
                    instanceToDeleteInDicoTeam1.Add(pair.Key);
            }
            else
                instanceToDeleteInDicoTeam1.Add(pair.Key);
        }

        //Mise à jour du dictionnaire des joueurs de l'équipe 2, pour qu'il n'y ait que des joueurs de l'équipe 2
        foreach (var pair in playersInTeam2Dico)
        {

            if (pair.Value.name == "Player(Clone)")
            {
                int dictionnaryPlayerTeamNumber = pair.Value.gameObject.GetComponent<PlayerController>().getTeamNumber();

                if (dictionnaryPlayerTeamNumber != 2)
                    instanceToDeleteInDicoTeam2.Add(pair.Key);
            }
            else
                instanceToDeleteInDicoTeam2.Add(pair.Key);
        }

        //Suppression des différents éléments incompatibles dans les dictionnaires
        foreach (NetworkInstanceId netId in instanceToDeleteInDicoTeam1)
            playersInTeam1Dico.Remove(netId);
        foreach (NetworkInstanceId netId in instanceToDeleteInDicoTeam2)
            playersInTeam2Dico.Remove(netId);
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
    #region Choix du personnage
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

        characterChoosed = true;
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        chooseCharacterCanvas.gameObject.SetActive(false);
        healthCanvas.gameObject.SetActive(true);
        numberOfTurnsCanvas.gameObject.SetActive(true);
        isItMyTurnCanvas.gameObject.SetActive(true);
    }
    #endregion

    #region Affichage des cartes
    public void hideCards(string whoseCards = "me")
    {
        if (!isLocalPlayer)
            return;
        if(whoseCards == "me")
        {
            showMyCardsCanvas.gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(true);
        }
        else
        {
            //TODO gérer la disparition de l'écran des cartes du coéquipier
        }

    }

    public void showCards(string whoseCards = "me")
    {
        if (!isLocalPlayer)
            return;

        if(whoseCards == "me")
        {
            showMyCardsCanvas.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(false);
            cardsDeckScriptFromThePlayer.showCardInformations();
        }

    }

    #endregion

    public string getPlayerName()
    {
        return playerName;
    }

    public void setPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    public int getId()
    {
        return id;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    #region Méthodes liées au teamNumber
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

    public bool isCharacterChoosed()
    {
        return characterChoosed;
    }

    public string getTeamsMembers()
    {
        return teamsMembers;
    }

    public void setTeamsMembers(string teamsMembers)
    {
        this.teamsMembers = teamsMembers;
    }

    public void updateTeamMembersText()
    {
        string playersRepartitionInTeams = "";
        // Ces 2 variables permettront d'avoir en sortie d'abord les joueurs de lé'équipe 1 puis ceux de l'équipe 2
        string playersInTeam1 = "";
        string playersInTeam2 = "";
        int team = 0;

        List<PlayerController> players = new List<PlayerController>();
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController player;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                player = pair.Value.gameObject.GetComponent<PlayerController>();
                players.Add(player);
                team = player.getTeamNumber();
                if (team == 1)
                    playersInTeam1 += player.getPlayerName() + regexTeam + team + regexNewPlayer;
                else if (team == 2)
                    playersInTeam2 += player.getPlayerName() + regexTeam + team + regexNewPlayer;
            }
        }

        playersRepartitionInTeams += playersInTeam1 + playersInTeam2;

        foreach (PlayerController p in players)
        {
            p.setTeamsMembers(playersRepartitionInTeams);
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

        if (numberOfPeopleInTeam1 > 0 && numberOfPeopleInTeam2 > 0)
            teamChoosedButton.interactable = true;
        else
            teamChoosedButton.interactable = false;


        team1MembersText.text = playersInTeam1;
        team2MembersText.text = playersInTeam2;
    }

    public bool haveAllPlayerChosenTheirCharacters()
    {
        bool allPlayersHaveATeam = true;
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                allPlayersHaveATeam = (allPlayersHaveATeam & pair.Value.gameObject.GetComponent<PlayerController>().isCharacterChoosed());
            }
        }

        /*if(allPlayersHaveATeam)
        {
            RpcSetTeamWhichPlays(currentShooterTeamNumber);
        }*/

        return allPlayersHaveATeam;
    }

    #endregion

    #region Méthodes liées au hatColor
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
    #endregion

    #region Méthodes liées aux tours

    [Command]
    public void CmdIncreaseNumberOfTurns()
    {
        RpcIncreaseNumberOfTurns();
    }

    [ClientRpc]
    public void RpcIncreaseNumberOfTurns()
    {

        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                p.setNumberOfTurns();

            }
        }

    }

    [Command]
    public void CmdIncreaseNumberOfBallsFired()
    {
        RpcIncreaseNumberOfBallsFired();
    }

    [ClientRpc]
    public void RpcIncreaseNumberOfBallsFired()
    {

        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                p.setNumberOfBallsFired();

            }
        }

    }

    public int getNumberOfTurns()
    {
        return numberOfTurns;
    }

    //Le nombre de tour n'augmente que de 1, donc pas besoin de passer d'arguments en paramètre
    public void setNumberOfTurns()
    {
        numberOfTurns++;
    }

    public void setNumberOfBallsFired()
    {
        numberOfBallsFired++;
    }

    [Command]
    public void CmdSetNumberOfBallsFiredByMyTeam(Constants action, int teamWhoFiredTheBalls)
    {
        RpcSetNumberOfBallsFiredByMyTeam(action, teamWhoFiredTheBalls);
    }


    [ClientRpc]
    public void RpcSetNumberOfBallsFiredByMyTeam(Constants action, int teamWhoFiredTheBalls)
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                p.setNumberOfBallsFiredByMyTeam(action, teamWhoFiredTheBalls);
            }
        }
    }

    public void setNumberOfBallsFiredByMyTeam(Constants action, int team)
    {
        if(teamNumber == team)
        {
            if(action == Constants.ADD)
            {
                numberOfBallsFiredByMyTeam++;
            }
            else if(action == Constants.RESET)
            {
                numberOfBallsFiredByMyTeam = (int)Constants.NUMBER_BASE_OF_CARDS_PLAYED_BY_EACH_TEAM;
            }
        }
        else
        {
            numberOfBallsFiredByMyTeam = (int)Constants.NUMBER_BASE_OF_CARDS_PLAYED_BY_EACH_TEAM;
        }
    }


    public int getNumberOfBallsFiredByMyTeam()
    {
        return numberOfBallsFiredByMyTeam;
    }

    [Command]
    public void CmdSetTeamWhichPlays(int number)
    {
        RpcSetTeamWhichPlays(number);
    }


    [ClientRpc]
    public void RpcSetTeamWhichPlays(int number)
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                p.setIsItMyTurn(p.getTeamNumber() == number);
            }
        }
        numberOfBallsFired = 0;
    }

    #endregion


    [ClientRpc]
    public void RpcGetNumberOfSpawnedVirus()
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> virusDico = NetworkServer.objects;
        int numberOfSpawnedVirus = 0;

        foreach (var pair in virusDico)
        {
            if (pair.Value.name == "Virus(Clone)")
            {
                numberOfSpawnedVirus++;
            }
        }
    }

    [Command]
    public void CmdSetGameOverForAllPlayers()
    {
        RpcSetGameOverForAllPlayers();
    }

    [ClientRpc]
    public void RpcSetGameOverForAllPlayers()
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController player;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                player = pair.Value.gameObject.GetComponent<PlayerController>();
                player.setIsGameEnded(true);
                player.setHasPlayerBeenNotifiedForGameOver(true);
            }
        }
    }

    public void setIsGameEnded(bool end)
    {
        this.isGameEnded = end;
    }

    public void setHasPlayerBeenNotifiedForGameOver(bool over)
    {
        this.hasPlayerBeenNotifiedForGameOver = over;
    }
}
