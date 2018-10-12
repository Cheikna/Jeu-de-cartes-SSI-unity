using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnsFollower : NetworkBehaviour {

    List<PlayerController> players;
    PlayerController currentPlayer;

    public int numberOfPlayersExpected { get; set; }
    int currentNumberOfPlayers = 0;
    int indexPlayerWhoPlays = -1;

	// Use this for initialization
	void Start () {
        

        players = new List<PlayerController>();
        StartCoroutine(GetAllOfThePlayersAfterFewSeconds());
	}
	
    IEnumerator GetAllOfThePlayersAfterFewSeconds()
    {
        yield return new WaitForSeconds(1.0f);

        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            Debug.Log(pair.Key + " --" + pair.Value);
            if (pair.Value.name == "Player(Clone)")
            {
                currentNumberOfPlayers++;
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                players.Add(p);
            }
        }
        Debug.Log("nombre de joueurs : " + players.Count);
        players[0].setIsItMyTurnHook(true);
        //CmdChooseNextPlayer();
    }

    
	// Update is called once per frame
	void Update () {

    }

    [ClientRpc]
    public void RpcChooseNextPlayer()
    {        
        indexPlayerWhoPlays += 1;
        Debug.Log("RpcChooseNextPlayer()");
        if (indexPlayerWhoPlays >= currentNumberOfPlayers)
            indexPlayerWhoPlays = 0;

        players[indexPlayerWhoPlays].setIsItMyTurnHook(true);
        currentPlayer = players[indexPlayerWhoPlays];

        Debug.Log("Joueur : " + indexPlayerWhoPlays);

        
    }

    IEnumerator WaitBeforeChangingPlayer()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
