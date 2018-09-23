using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideCanvas : MonoBehaviour {

    [SerializeField]
    private Canvas mainMenuCanvas;
    [SerializeField]
    private Canvas creditsCanvas;
    [SerializeField]
    private Canvas quitCanvas;

    // Use this for initialization
    void Start () {
        creditsCanvas.enabled = false;
        quitCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showHideCanvas(string nextCanvasToShow)
    {
        switch(nextCanvasToShow)
        {
            case "credits":
                creditsCanvas.enabled = true;
                quitCanvas.enabled = false;
                mainMenuCanvas.enabled = false;
                break;

            case "menu":
                creditsCanvas.enabled = false;
                quitCanvas.enabled = false;
                mainMenuCanvas.enabled = true;
                break;

            case "quit":
                creditsCanvas.enabled = false;
                quitCanvas.enabled = true;
                mainMenuCanvas.enabled = false;
                break;
        }
    }
}
