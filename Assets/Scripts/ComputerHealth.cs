using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ComputerHealth : NetworkBehaviour {

    const int osMaxHealth = 6;
    const int softwareMaxHealth = 6;
    const int hardwareMaxHealth = 6;

    [SyncVar(hook = "onChangeHealth")]
    public int currentOsHealth = osMaxHealth;
    public int currentSofwareHealth = softwareMaxHealth;
    public int currentHardwareHealth = hardwareMaxHealth;

    public RectTransform osHealthBar;
    public RectTransform softwareHealthBar;
    public RectTransform hardwareHealthBar;

    
    public void takeDamage(ComputerLayer affectedLayer, int amount)
    {
        //Vérifier si les points de vies sont négatifs
        switch(affectedLayer)
        {
            case ComputerLayer.OS:
                currentOsHealth -= amount;
                break;

            case ComputerLayer.SOFTWARE:
                currentSofwareHealth -= amount;
                break;

            case ComputerLayer.HARDWARE:
                currentHardwareHealth -= amount;
                break;
        }        

        if (currentOsHealth <= 0)
            currentOsHealth = 0;
        if (currentSofwareHealth <= 0)
            currentSofwareHealth = 0;
        if (currentHardwareHealth <= 0)
            currentHardwareHealth = 0;

        int remainingLife = currentOsHealth + currentSofwareHealth + currentHardwareHealth;

        if (remainingLife <= 0)
            Debug.Log("La partie est terminée, une équipe a perdu");
    }

    void onChangeHealth(int health)
    {

    }
}
