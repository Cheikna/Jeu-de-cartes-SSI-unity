using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VirusController : NetworkBehaviour
{   
    public ComputerLayer targetLayer1 { get; set; }
    // Dégats occasionnés
    public int damageLayer1 { get; set; }

    /*public ComputerLayer targetLayer2 { get; set; }
    public int damageLayer2 { get; set; }*/


    void OnTriggerEnter(Collider collision)
    {
        var computerHit = collision.gameObject;
        var computerHealth = computerHit.GetComponent<ComputerHealth>();
        
        if (computerHealth != null)
        {
            // Vérifier si ce sont des cartes d'attaques et non des cartes rajoutant des point de vie au joueur
            if(!targetLayer1.Equals(null) && damageLayer1 >= 0)
                computerHealth.setHealth(targetLayer1, damageLayer1);
            /*if (!targetLayer2.Equals(null) && damageLayer2 > 0)
                computerHealth.setHealth(targetLayer2, damageLayer2);*/
        }
        
        Destroy(gameObject, 7.0f);
    }

    
}
