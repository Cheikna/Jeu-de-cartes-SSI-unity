using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDictionnary {

    public static Dictionary<string, Card> getCardsDictionnary()
    {
        Dictionary<string, Card> cardsDictionary = new Dictionary<string, Card>();

        //Initialisation de toutes les cartes du jeu
        Card trojan = new Card("TROJAN",
                               "Le cheval de Troie est un logiciel en apparence légitime, mais qui contient une fonctionnalité malveillante.C’est un virus statique.",
                               "SOFTWARE -2", true, ComputerLayer.SOFTWARE, 2, new Color(255, 0, 0));

        Card virusCrypto = new Card("VIRUS\nCRYPTOLOCKER",
                               " Alors que vous allumez votre ordinateur pour consulter vos mails, un message apparait. Il faut payer une rançon pour récupérer vos données.",
                               "OS -2", true, ComputerLayer.OS, 2, new Color(255, 0, 0));

        Card trojanPlus = new Card("TROJAN+", "Une fois cette carte posée, le trojan fait des dégâts.",
                               "SOFTWARE -3", true, ComputerLayer.SOFTWARE, 3, new Color(0, 176, 80));

        Card ddos = new Card("DDOS", "Une attaque DDoS vise à rendre un serveur indisponible en surchargeant la bande passante du serveur ou en accaparant ses ressources jusqu'à épuisement.",
                               "HARDWARE -4", true, ComputerLayer.HARDWARE, 4, new Color(255, 0, 0));

        Card scan = new Card("SCAN", "Le scanner analyse les éléments de votre ordinateur.",
                               "SOFTWARE +1", false, ComputerLayer.SOFTWARE, 1, new Color(0, 176, 240));

        Card vpn = new Card("VPN", "Le VPN est un tunnel sécurisé à l’intérieur d’un réseau.",
                               "SOFTWARE +2", false, ComputerLayer.SOFTWARE, 2, new Color(0, 176, 240));


        //Ajout des cartes à au dictionnaire
        cardsDictionary.Add("trojan", trojan);
        cardsDictionary.Add("virusCrypto", virusCrypto);
        cardsDictionary.Add("trojanPlus", trojanPlus);
        cardsDictionary.Add("ddos", ddos);
        cardsDictionary.Add("scan", scan);
        cardsDictionary.Add("vpn", vpn);




        return cardsDictionary;
    }

    public static Dictionary<int, Card> getCardsDictionnaryForDistribution()
    {
        Dictionary<int, Card> cardsDictionary = new Dictionary<int, Card>();

        int i = 1;

        foreach (KeyValuePair<string, Card> card in getCardsDictionnary())
        {
            cardsDictionary.Add(i, card.Value);
        }

        return cardsDictionary;
    }



}
