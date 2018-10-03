using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardPlayed : NetworkBehaviour {

    [SyncVar(hook = "setCurrentCard")]
    public Card currentCardPlayed;

    public string cardName = " ";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentCardPlayed != null)
            cardName = currentCardPlayed.name;
		
	}

    public void setCurrentCard(Card newCard)
    {
        if(isLocalPlayer)
            currentCardPlayed = newCard;
    }

    public Card getCardPlayedByThePlayer()
    {
        return currentCardPlayed;
    }
}
