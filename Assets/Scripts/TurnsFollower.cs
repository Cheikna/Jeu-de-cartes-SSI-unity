using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnsFollower : NetworkBehaviour {

    List<PlayerController> players;

    public int numberOfPlayersExpected { get; set; }
    int currentNumberOfPlayers = 0;
    int indexPlayerWhoPlays = 0;

	// Use this for initialization
	void Start () {

        if (!isServer)
            return;

        players = new List<PlayerController>();
        StartCoroutine(GetAllOfThePlayersAfterFewSeconds());
	}
	
    IEnumerator GetAllOfThePlayersAfterFewSeconds()
    {
        yield return new WaitForSeconds(1.0f);
        List <GameObject> playersGameObject = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));
        foreach(GameObject p in playersGameObject)
        {
            players.Add(p.GetComponent<PlayerController>());
        }
        currentNumberOfPlayers = players.Count;
        numberOfPlayersExpected = Prototype.NetworkLobby.LobbyPlayerList.numberOfPlayerInTheRoom;
        Debug.Log("Nombre de joueurs actuels : " + currentNumberOfPlayers);
    }


	// Update is called once per frame
	void Update () {

        //TODO : enlever les commentaires une fois sûr
        /*if(currentNumberOfPlayers != numberOfPlayersExpected)
            players = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));*/


        

    }

    [ClientRpc]
    public void RpcChooseNextPlayer()
    {
        Debug.Log("RpcChooseNextPlayer()");
        if (indexPlayerWhoPlays >= currentNumberOfPlayers)
            indexPlayerWhoPlays = 0;

        foreach (PlayerController player in players)
        {
            player.isItMyTurn = false;
        }
        players[indexPlayerWhoPlays].isItMyTurn = true;
        Debug.Log("Joueur : " + indexPlayerWhoPlays);

        indexPlayerWhoPlays += 1;
    }
}
