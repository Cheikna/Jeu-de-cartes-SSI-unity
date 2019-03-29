using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VirusController : MonoBehaviour {

    
    public ComputerLayer targetLayer1 { get; set; }
    // Dégats occasionnés
    public int damageLayer1 { get; set; }

    public ComputerLayer targetLayer2 { get; set; }
    public int damageLayer2 { get; set; }
    public PlayerController playerWhoFiredTheVirus { get; set; }
    public int numberOfTeamWhoFiredTheVirus { get; set; }
    int teamNumberOfTheTouchedPlayer = 0;



    
    void OnTriggerEnter(Collider collision)
    {
        var computerHit = collision.gameObject;
        var computerHealth = computerHit.GetComponent<ComputerHealth>();
        var playerController = computerHit.GetComponent<PlayerController>();
        bool isItTimeToChangeTeam = false;

        // Récupération du joueur qui a instancié le virus, celui qui en est à l'origine afin de dire qu'il vient de jouer
        if (playerWhoFiredTheVirus != null)
        {
            playerWhoFiredTheVirus.CmdSetNumberOfBallsFiredByMyTeam(Constants.ADD, playerWhoFiredTheVirus.getTeamNumber());
            isItTimeToChangeTeam = (playerWhoFiredTheVirus.getNumberOfBallsFiredByMyTeam() == (int)Constants.NUMBER_MAX_OF_CARDS_PLAYED_BY_EACH_TEAM);
            if(isItTimeToChangeTeam)
            {
                playerWhoFiredTheVirus.setIsItMyTurnHook(false);
                playerWhoFiredTheVirus.CmdSetNumberOfBallsFiredByMyTeam(Constants.RESET, playerWhoFiredTheVirus.getTeamNumber());
            }
        }


        if(playerController != null && isItTimeToChangeTeam)
        {
            playerController.setIsItMyTurnHook(true);
        }
        
        if (computerHealth != null)
        {
            // Vérifier si ce sont des cartes d'attaques et non des cartes rajoutant des point de vie au joueur
            if(!targetLayer1.Equals(null) && damageLayer1 > 0)
                computerHealth.setHealth(targetLayer1, damageLayer1);
            if (!targetLayer2.Equals(null) && damageLayer2 > 0)
                computerHealth.setHealth(targetLayer2, damageLayer2);

            if (computerHealth.remainingLife <= 0)
            {
                computerHealth.gameOver();
                if (playerController != null)
                    playerController.CmdSetGameOverForAllPlayers();
            }

        }

        
        Destroy(gameObject, 7.0f);
    }
}
