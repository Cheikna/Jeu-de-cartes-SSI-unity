using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonsActions : MonoBehaviour {



    #region Bouton & Canvas
    //[SerializeField]
    private Canvas showMyCardsCanvas;
    //[SerializeField]
    private Canvas mainCanvas;
    //[SerializeField]
    private Canvas healthCanvas;
    //[SerializeField]
    private Canvas chooseCharacterCanvas;
    //[SerializeField]
    private Component femaleCharacter;
    //[SerializeField]
    private Component maleCharacter;
    private Button femaleCharacterButton;
    private Button maleCharacterButton;
    #endregion


    // Use this for initialization
    void Start () {


    }

    public void hideMyCards()
    {
        showMyCardsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    public void showMyCards()
    {

        showMyCardsCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
    }
}
