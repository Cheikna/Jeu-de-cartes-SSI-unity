using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardsDeck : MonoBehaviour {

    List<Card> cardsDeck = new List<Card>();
    Card choosenCard;
    int numberOfCards;
    int indexOfCard = 0;

    //PlayerController playerController;

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

    public void loadCards()
    {
        CardsDictionnary.loadCardsDictionnary();
        this.numberOfCards = (int)Constants.NUMBER_CARDS_PER_PLAYER;

        for(int i = 0; i < numberOfCards; i++)
        {
            Card card = CardsDictionnary.getRandomCard();
            if(card != null)
                cardsDeck.Add(card);
        }
        //showCardInformations();
    }

    public int getNbOfCards()
    {
        return cardsDeck.Count;
    }

    public Card getCard(int i)
    {
        return cardsDeck[i];
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
        // Retrait de la carte qui va être jouée
        cardsDeck.Remove(card);
        // Ajout d'une nouvelle carte dans le deck
        cardsDeck.Add(CardsDictionnary.getRandomCard());
        //Affichage des nouvelles informations après le retrait et l'ajout d'une nouvelle carte
        showCardInformations();
        // Appel à la méthode fire afin de lancer la carte
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

    public void showCardInformations()
    {
        indexOfTheCurrentCardText.text = (indexOfCard + 1).ToString() + " / " + numberOfCards.ToString();
        Card card = cardsDeck[indexOfCard];
        cardName.text = card.name;
        cardDefiniton.text = card.definition;
        cardAction.text = card.action;
        backgroundColor.color = card.getCardColor();
    }
}
