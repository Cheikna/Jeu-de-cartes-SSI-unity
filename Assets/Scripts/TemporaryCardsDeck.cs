using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCardsDeck : MonoBehaviour {

    List<Card> cards = new List<Card>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addCard(Card card)
    {
        cards.Add(card);
    }

    public List<Card> getCards()
    {
        return cards;
    }
}
