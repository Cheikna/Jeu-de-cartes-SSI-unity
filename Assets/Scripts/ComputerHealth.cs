using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ComputerHealth : NetworkBehaviour {

    const int osMaxHealth = 6;
    const int softwareMaxHealth = 6;
    const int hardwareMaxHealth = 6;
    float oneLife;

    [SyncVar(hook = "onChangeOsHealth")]
    public int currentOsHealth = osMaxHealth;
    [SyncVar(hook = "onChangeSoftwareHealth")]
    public int currentSofwareHealth = softwareMaxHealth;
    [SyncVar(hook = "onChangeHardwareHealth")]
    public int currentHardwareHealth = hardwareMaxHealth;

    [SerializeField]
    private RectTransform osHealthBar;
    [SerializeField]
    private RectTransform softwareHealthBar;
    [SerializeField]
    private RectTransform hardwareHealthBar;

    void Start()
    {
        if (!isLocalPlayer)
            return;

        oneLife = softwareHealthBar.sizeDelta.x / softwareMaxHealth;
    }
    
    public void takeDamage(ComputerLayer touchedLayer, int amount)
    {
        if (!isServer)
            return;
        
        //Vérifier si les points de vies sont négatifs
        switch (touchedLayer)
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
        {
            Debug.Log("La partie est terminée, une équipe a perdu");
            //PlayerController.isGameOver = true;
        }
            
    }


    //Même si cette méthode ressemble à une méthode précédente,
    //elle reste nécessaire pour gérer les variations de points de vie à travers le serveurs et les clients
    public void resizeHealthBar(ComputerLayer touchedLayer, int health)
    {
        float sizeGreenLife = 0;
        switch (touchedLayer)
        {
            case ComputerLayer.OS:
                sizeGreenLife = oneLife * health;
                osHealthBar.sizeDelta = new Vector2(sizeGreenLife, osHealthBar.sizeDelta.y);
                break;

            case ComputerLayer.SOFTWARE:
                sizeGreenLife = oneLife * health;
                softwareHealthBar.sizeDelta = new Vector2(sizeGreenLife, softwareHealthBar.sizeDelta.y);
                break;

            case ComputerLayer.HARDWARE:
                sizeGreenLife = oneLife * health;
                hardwareHealthBar.sizeDelta = new Vector2(sizeGreenLife, hardwareHealthBar.sizeDelta.y);
                break;
        }      
        
    }

    // Obligation de passer par ces méthodes supplémentaires car Syncvar ne prend en 
    // paramètre qu'un seul argument alors que je voudrais qu'il prenne aussi computerLayer
    void onChangeOsHealth(int health)  { resizeHealthBar(ComputerLayer.OS, health); }
    void onChangeHardwareHealth(int health) { resizeHealthBar(ComputerLayer.HARDWARE, health); }
    void onChangeSoftwareHealth(int health) { resizeHealthBar(ComputerLayer.SOFTWARE, health); }

    void Update()
    {
    }
}
