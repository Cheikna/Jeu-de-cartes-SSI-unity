using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouvementCameraMenu : MonoBehaviour {

	[SerializeField]
	private Transform vue_actuelle;
	private float vitesse = 0.05f;
	[SerializeField]
	private GameObject mainMenuComputer;
	[SerializeField]
	private Camera mainCamera;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, vue_actuelle.position, vitesse);
		transform.rotation = Quaternion.Slerp(transform.rotation, vue_actuelle.rotation, vitesse);

	}

	public void setVue(Transform nouvelle_vue)
	{
		vue_actuelle = nouvelle_vue;
	}

	public void quitGame()
	{
		Application.Quit();
	}

	public void goToPlayScene()
	{
        mainMenuComputer.SetActive(false);
        StartCoroutine(WaitFewSeconds());
	}

    IEnumerator WaitFewSeconds()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Lobby");
    }
}
