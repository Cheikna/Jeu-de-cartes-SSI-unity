using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        PlayerController localPlayer = gamePlayer.GetComponent<PlayerController>();
        string playerNameInLobby = lobby.playerName.Replace('>', '-').Replace('&', '-');
        localPlayer.setPlayerName(playerNameInLobby);
        
    }
}
