using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnsFollower : NetworkBehaviour {

    /*List<PlayerController> players;
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
            if (pair.Value.name == "Player(Clone)")
            {
                currentNumberOfPlayers++;
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                players.Add(p);
            }
        }        
        //players[0].setIsItMyTurnHook(true);
        //players[0].CmdSetFirstTeamWhichPlays();
    }

    public void setPlayersIsItMyTurnHook()
    {
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController player;
        int firstTeamToPlay = 1;

        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                player = pair.Value.gameObject.GetComponent<PlayerController>();
                Debug.Log("team : " + player.getTeamNumber());
                Debug.Log("team : " + player.teamNumber);
                if (player.teamNumber == firstTeamToPlay)
                    player.setIsItMyTurnHook(true);
                else
                    player.setIsItMyTurnHook(false);
            }
        }
    }

    
	// Update is called once per frame
	void Update () {

    }

    [ClientRpc]
    public void RpcChooseNextPlayer()
    {        
        indexPlayerWhoPlays += 1;
        if (indexPlayerWhoPlays >= currentNumberOfPlayers)
            indexPlayerWhoPlays = 0;

        players[indexPlayerWhoPlays].setIsItMyTurnHook(true);
        currentPlayer = players[indexPlayerWhoPlays];        
    }

    IEnumerator WaitBeforeChangingPlayer()
    {
        yield return new WaitForSeconds(2.0f);
    }*/
}
