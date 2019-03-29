using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ComputerHealth : NetworkBehaviour {
    

    [SyncVar(hook = "onChangeOsHealth")]
    public int currentOsHealth = (int)Constants.OS_MAX_HEALTH;
    [SyncVar(hook = "onChangeSoftwareHealth")]
    public int currentSoftwareHealth = (int)Constants.SOFTWARE_MAX_HEALTH;
    [SyncVar(hook = "onChangeHardwareHealth")]
    public int currentHardwareHealth = (int)Constants.HARWARE_MAX_HEALTH;
    [SyncVar]
    public int remainingLife = (int)Constants.OS_MAX_HEALTH + (int)Constants.SOFTWARE_MAX_HEALTH + (int)Constants.HARWARE_MAX_HEALTH;

    public float size = 0;


    [SerializeField]
    private RectTransform osHealthBar;
    [SerializeField]
    private RectTransform softwareHealthBar;
    [SerializeField]
    private RectTransform hardwareHealthBar;
    [SerializeField]
    private RectTransform osHealthBar2;
    [SerializeField]
    private RectTransform softwareHealthBar2;
    [SerializeField]
    private RectTransform hardwareHealthBar2;
    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private Text gameStateText;


    //Unité d'une vie par rapport à la taille de la barre de vie totale
    float oneLife = (int)Constants.HEALTH_BAR_SIZE / (int)Constants.OS_MAX_HEALTH;
    float oneLife2 = (int)Constants.HEALTH_BAR_SIZE / (int)Constants.OS_MAX_HEALTH;

    void Start()
    {
        if (!isLocalPlayer)
            return;
    }
    
    
    public void setHealth(ComputerLayer touchedLayer, int amount, bool isCardDecreaseHealth = true)
    {
        if (!isServer)
            return;
        
        //Vérifier si les points de vies sont négatifs
        switch (touchedLayer)
        {
            case ComputerLayer.OS:
                if(isCardDecreaseHealth)
                    currentOsHealth -= amount;
                else
                    currentOsHealth = updateIfItIsLessThanTheMaximumHealth(currentOsHealth + amount);
                break;

            case ComputerLayer.SOFTWARE:
                if (isCardDecreaseHealth)
                    currentSoftwareHealth -= amount;
                else
                    currentSoftwareHealth = updateIfItIsLessThanTheMaximumHealth(currentSoftwareHealth + amount);
                break;

            case ComputerLayer.HARDWARE:
                if (isCardDecreaseHealth)
                    currentHardwareHealth -= amount;
                else
                    currentHardwareHealth = updateIfItIsLessThanTheMaximumHealth(currentHardwareHealth + amount);
                break;
        }

        if (currentOsHealth <= 0)
            currentOsHealth = 0;
        if (currentSoftwareHealth <= 0)
            currentSoftwareHealth = 0;
        if (currentHardwareHealth <= 0)
            currentHardwareHealth = 0;

        remainingLife = currentOsHealth + currentSoftwareHealth + currentHardwareHealth;

        /*if (remainingLife <= 0)
        {
            gameOver();
        }*/
            
    }

    public int updateIfItIsLessThanTheMaximumHealth(int health)
    {
        return (health > 6) ? 6 : health;
    }


    public void resizeHealthBar(ComputerLayer touchedLayer, int health)
    {
        float sizeGreenLife = oneLife * health;
        float sizeGreenLife2 = oneLife2 * health;
        switch (touchedLayer)
        {
            case ComputerLayer.OS:
                //sizeGreenLife = oneLife * health;
                osHealthBar.sizeDelta = new Vector2(sizeGreenLife, osHealthBar.sizeDelta.y);
                osHealthBar2.sizeDelta = new Vector2(sizeGreenLife2, osHealthBar2.sizeDelta.y);
                break;

            case ComputerLayer.SOFTWARE:
                //sizeGreenLife = oneLife * health;
                softwareHealthBar.sizeDelta = new Vector2(sizeGreenLife, softwareHealthBar.sizeDelta.y);
                softwareHealthBar2.sizeDelta = new Vector2(sizeGreenLife2, softwareHealthBar2.sizeDelta.y);
                break;

            case ComputerLayer.HARDWARE:
                //sizeGreenLife = oneLife * health;
                hardwareHealthBar.sizeDelta = new Vector2(sizeGreenLife, hardwareHealthBar.sizeDelta.y);
                hardwareHealthBar2.sizeDelta = new Vector2(sizeGreenLife2, hardwareHealthBar2.sizeDelta.y);
                break;
        }
        size = sizeGreenLife2;



    }

    // Obligation de passer par ces méthodes supplémentaires car Syncvar ne prend en 
    // paramètre qu'un seul argument alors que je voudrais qu'il prenne aussi computerLayer
    void onChangeOsHealth(int health)  { resizeHealthBar(ComputerLayer.OS, health); }
    void onChangeHardwareHealth(int health) { resizeHealthBar(ComputerLayer.HARDWARE, health); }
    void onChangeSoftwareHealth(int health) { resizeHealthBar(ComputerLayer.SOFTWARE, health); }


    //Opérations à faire lorsque la partie est terminée
    public void gameOver()
    {
        if(remainingLife <= 0)
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameStateText.text = "VOUS AVEZ PERDU !";
        }
        else
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameStateText.text = "VOUS AVEZ GAGNE !";
        }
    }

    public void onClickBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
