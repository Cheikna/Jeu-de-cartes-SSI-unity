using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerUtils : NetworkBehaviour {


    [SerializeField]
    private Text team1Text;
    [SerializeField]
    private Text team2Text;
    List<PlayerController> players;

    

    // Use this for initialization
    void Start () {

        players = new List<PlayerController>();
        StartCoroutine(GetAllOfThePlayersAfterFewSeconds());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [ClientRpc]
    public void RpcAPlayerChangedTeam()
    {
        List<PlayerController> players = new List<PlayerController>();
        Dictionary<NetworkInstanceId, NetworkIdentity> playersDico = NetworkServer.objects;
        PlayerController p;
        foreach (var pair in playersDico)
        {
            if (pair.Value.name == "Player(Clone)")
            {
                p = pair.Value.gameObject.GetComponent<PlayerController>();
                players.Add(p);
            }
        }


        string playersInTeam1 = "";
        string playersInTeam2 = "";

        foreach(PlayerController player in players)
        {
            if (player.getTeamNumber() == 1)
                playersInTeam1 += "- " + player.getPlayerName() + " \n";
            else
                playersInTeam2 += "- " + player.getPlayerName() + " \n";
        }

        team1Text.text = playersInTeam1;
        team2Text.text = playersInTeam2;
    }



    IEnumerator GetAllOfThePlayersAfterFewSeconds()
    {
        yield return new WaitForSeconds(1.15f);
        
    }
}
