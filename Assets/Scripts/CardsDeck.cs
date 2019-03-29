using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardsDeck : MonoBehaviour {

    //[SerializeField]
    //private TemporaryCardsDeck tempDeck;
    [SerializeField]
    private CardPlayed cardPlayed;

    List<Card> cardsDeck = new List<Card>();
    Card choosenCard;
    int numberOfCards;
    int indexOfCard = 0;

    PlayerController playerController;

    [SerializeField]
    private Text indexOfTheCurrentCardText;
    [SerializeField]
    private Text cardName;
    [SerializeField]
    private Text cardDefiniton;
    [SerializeField]
    private Text cardAction;
    [SerializeField]
    private Image backgroundColor;

    public void add(Card card)
    {
        cardsDeck.Add(card);
    }

    public void remove(Card card)
    {
        cardsDeck.Remove(card);
    }

    public int getNbOfCards()
    {
        return cardsDeck.Count;
    }

    IEnumerator WaitBeforeLoadingMyCardsDeck()
    {
        yield return new WaitForSeconds(1.0f);
        //cardsDeck = tempDeck.getCards();
        numberOfCards = cardsDeck.Count;
        showCardInformations();

    }

    public Card getCard(int i)
    {
        return cardsDeck[i];
    }

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(WaitBeforeLoadingMyCardsDeck());
    }

    

    public void getDeckOfCardsFromThePlayerController(List<Card> cards)
    {
        //cardsDeck = playerController.getMyCardsDeck();
        getRandomCards();
        //cardsDeck = cards;
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
        cardPlayed.currentCardPlayed = card;
        GetComponentInParent<PlayerController>().shootFromCardsDeckClass(card.getCardinfosInAStringArray());
    }

    public void passMyTurn()
    {
        GetComponentInParent<PlayerController>().setIsItMyTurnHook(false);
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
        indexOfTheCurrentCardText.text = (indexOfCard + 1).ToString() + " / " + numberOfCards.ToString();
        Card card = cardsDeck[indexOfCard];
        cardName.text = card.name;
        cardDefiniton.text = card.definition;
        cardAction.text = card.action;
        backgroundColor.color = card.getCardColor();
    }

    private void getRandomCards()
    {
        Card trojan = new Card("Trojan",
                                       "Le cheval de Troie est un logiciel en apparence légitime, mais qui contient une fonctionnalité malveillante.C’est un virus statique.",
                                       "SOFTWARE -2", true, ComputerLayer.SOFTWARE, 2, new Color(255, 0, 0));



        Card virusCrypto = new Card("VIRUS\nCRYPTOLOCKER",
                               " Alors que vous allumez votre ordinateur pour consulter vos mails, un message apparait. Il faut payer une rançon pour récupérer vos données.",
                               "OS -2", true, ComputerLayer.OS, 2, new Color(255, 0, 0));



        Card ddos = new Card("DDOS", "Une attaque DDoS vise à rendre un serveur indisponible en surchargeant la bande passante du serveur ou en accaparant ses ressources jusqu'à épuisement.",
                               "HARDWARE -4", true, ComputerLayer.HARDWARE, 4, new Color(255, 0, 0));

        Card scan = new Card("SCAN", "Le scanner analyse les éléments de votre ordinateur.",
                               "SOFTWARE +1", false, ComputerLayer.SOFTWARE, 1, new Color(0, 176, 240));

        Card vpn = new Card("VPN", "Le VPN est un tunnel sécurisé à l’intérieur d’un réseau.",
                               "SOFTWARE +2", false, ComputerLayer.SOFTWARE, 2, new Color(0, 176, 240));

        add(trojan);
        add(virusCrypto);
        add(ddos);
        add(scan);
        add(vpn);
        //indexOfCard = 0;
        //numberOfCards = cardsDeck.Count;
        //showCardInformations();
    }
}
