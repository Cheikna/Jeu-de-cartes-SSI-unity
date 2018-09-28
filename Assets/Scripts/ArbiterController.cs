using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ArbiterController : MonoBehaviour {


    CardsDeck cardsDeck;
    List<GameObject> players;

	// Use this for initialization
	void Start () {
        cardsDeck = new CardsDeck();
        players = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    private int playerCount = 0;

    void OnPlayerConnected(NetworkIdentity player)
    {
        players.Add(player.gameObject);
        Debug.Log(player.gameObject.tag);
    }
}
