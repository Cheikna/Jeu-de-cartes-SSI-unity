using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ArbiterController : MonoBehaviour {


    List<Card> arbiterCardsDeck = new List<Card>();
    public int numberOfPlayersInTheGame { get; set; }
    public static bool isCardsDistributionOver { get; set; }
    //List<GameObject> players;

	// Use this for initialization
	void Start () {
        isCardsDistributionOver = false;
	}

    // bleue 0, 176, 240
    // rouge 255,0,0
    // vert 0, 176, 80

    void loadCards()
    {
        Card trojan = new Card("TROJAN",
                               "Le cheval de Troie est un logiciel en apparence légitime, mais qui contient une fonctionnalité malveillante.C’est un virus statique.",
                               "SOFTWARE -2", true, ComputerLayer.SOFTWARE, 2, new Color(255, 0, 0));



        Card virusCrypto = new Card("VIRUS\nCRYPTOLOCKER",
                               " Alors que vous allumez votre ordinateur pour consulter vos mails, un message apparait. Il faut payer une rançon pour récupérer vos données.",
                               "OS -2", true, ComputerLayer.OS, 2, new Color(255, 0, 0));



        Card ddos = new Card("DDOS", "Une attaque DDoS vise à rendre un serveur indisponible en surchargeant la bande passante du serveur ou en accaparant ses ressources jusqu'à épuisement.",
                               "HARDWARE -4", true, ComputerLayer.HARDWARE, 4, new Color(255, 0, 0));

        Card scan = new Card("SCAN", "Le scanner analyse les éléments de votre ordinateur.",
                               "SOFTWARE +1", false, ComputerLayer.SOFTWARE, 1, new Color(0,176,240));

        Card vpn = new Card("VPN", "Le VPN est un tunnel sécurisé à l’intérieur d’un réseau.",
                               "SOFTWARE +2", false, ComputerLayer.SOFTWARE, 2, new Color(0, 176, 240));

        Card trojanPlus = new Card("TROJAN+", "Une fois cette carte posée, le trojan fait des dégâts.",
                               "SOFTWARE -3", true, ComputerLayer.SOFTWARE, 3, new Color(0, 176, 80));




        arbiterCardsDeck.Add(trojan);
        arbiterCardsDeck.Add(trojan);
        arbiterCardsDeck.Add(trojan);
        arbiterCardsDeck.Add(virusCrypto);
        arbiterCardsDeck.Add(ddos);
        arbiterCardsDeck.Add(scan);
        arbiterCardsDeck.Add(scan);
        arbiterCardsDeck.Add(vpn);
        arbiterCardsDeck.Add(trojanPlus);

    }


    // Cette méthode doit être appelée par le serveur
    public void distributeCards(List<GameObject> playersGameObject)
    {
        // L'arbitre récupère les cartes dans son deck
        loadCards();

        // On convertit la liste de GameObject en liste de joueur afin d'accéder aux différentes méthodes présentes dans la classe
        List<PlayerController> players = new List<PlayerController>();

        foreach(GameObject playerGameObject in playersGameObject)
        {
            players.Add(playerGameObject.GetComponent<PlayerController>());
        }
        
        int nbPlayers = numberOfPlayersInTheGame; //TODO players.count
        int nbCardsForEachPlayer = arbiterCardsDeck.Count / nbPlayers;

        // On doit passer par le calcul précedent au cas où il y aurait des cartes en trop ce qui provoquerait un nombre de cartes différents pour les joueurs
        int nbCardsToDistrib = nbCardsForEachPlayer * nbPlayers;
        int indexPlayer = 0;

        for(int i = 0; i < nbCardsToDistrib; i++)
        {
            if (indexPlayer >= nbPlayers)
                indexPlayer = 0;
            //Si on est à la dernière carte à distribuer pour chaque joueurs
            if (i == (nbCardsToDistrib - 1))
                players[indexPlayer].areAllCardsDistributed = true;
            players[indexPlayer].addCardToMyDeck(arbiterCardsDeck[i]);
            

            indexPlayer++;
        }

        //Faire en sorte que tous les joueurs puissent savoir que la distribution des cartes est terminée.
        isCardsDistributionOver = true;
    }


	// Update is called once per frame
	void Update () {
	}
}
