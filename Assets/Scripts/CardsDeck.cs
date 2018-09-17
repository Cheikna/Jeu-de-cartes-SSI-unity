using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardsDeck : MonoBehaviour {

    List<Card> cardsDeck;
    Card choosenCard;
    int numberOfCards;
    int indexOfCard;

    [SerializeField]
    private Text cardName;
    [SerializeField]
    private Text cardDefiniton;
    [SerializeField]
    private Text cardAction;
    [SerializeField]
    private Image backgroundColor;

    public CardsDeck()
    {
        cardsDeck = new List<Card>();
    }

    public void add(Card card)
    {
        cardsDeck.Add(card);
    }

    public void remove(Card card)
    {
        cardsDeck.Remove(card);
    }

	// Use this for initialization
	void Start () {

        Card trojan = new Card("Trojan", 
                               "Le cheval de Troie est un logiciel en apparence légitime, mais qui contient une fonctionnalité malveillante.C’est un virus statique.", 
                               "SOFTWARE -2", true, ComputerLayer.SOFTWARE,2, new Color(255, 0, 0));



        Card virusCrypto = new Card("VIRUS\nCRYPTOLOCKER",
                               " Alors que vous allumez votre ordinateur pour consulter vos mails, un message apparait. Il faut payer une rançon pour récupérer vos données.",
                               "OS -2", true,ComputerLayer.OS, 2, new Color(255, 0, 0));



        Card ddos = new Card("DDOS", "Une attaque DDoS vise à rendre un serveur indisponible en surchargeant la bande passante du serveur ou en accaparant ses ressources jusqu'à épuisement.",
                               "HARDWARE -4", true,ComputerLayer.HARDWARE, 4, new Color(255, 0, 0));
        
        add(trojan);
        add(virusCrypto);
        add(ddos);
        indexOfCard = 0;
        numberOfCards = cardsDeck.Count;
        showCardInformations();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickPreviousCard()
    {
        if(indexOfCard > 0)
        {
            indexOfCard--;
            showCardInformations();
        }       

    }

    public void onClickConfirmCard()
    {
        Card card = cardsDeck[indexOfCard];
        /*cardsDeck.RemoveAt(indexOfCard);
        numberOfCards--;*/
        showCardInformations();
        GetComponentInParent<PlayerController>().shootFromCardsDeckClass(card.touchedLayer, card.getDamage());
    }

    public void onClickNextCard()
    {
        if (indexOfCard < numberOfCards - 1)
        {
            indexOfCard++;
            showCardInformations();
        }
    }

    void showCardInformations()
    {
        Card card = cardsDeck[indexOfCard];
        cardName.text = card.name;
        cardDefiniton.text = card.definition;
        cardAction.text = card.action;
        backgroundColor.color = card.getCardColor();
    }
}
