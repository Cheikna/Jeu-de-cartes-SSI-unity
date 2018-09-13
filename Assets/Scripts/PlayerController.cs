using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public Transform bulletSpawn;
    

    [SerializeField]
    private GameObject controlPanel;

    public Button goLeftBtn;
    public Button goRightBtn;
    public Button goUpBtn;
    public Button goDownBtn;
    private Button fireButton;

    float moveX = 0;
    float moveZ = 0;

    // Use this for initialization
    void Start () {

        /*goLeftBtn = goLeftBtn.GetComponent<Button>();
        goRightBtn = goLeftBtn.GetComponent<Button>();
        goUpBtn = goLeftBtn.GetComponent<Button>();
        goDownBtn = goLeftBtn.GetComponent<Button>();

        fireButton = GetComponentInChildren<Button>();*/
        

        /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            controlPanel.SetActive(true);
        }
        */
        //fireButton.GetComponent<Button>().onClick.AddListener(delegate { MoveOnPhone("er"); });
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        /*goLeftBtn.onClick.AddListener(delegate() { MoveOnPhone("left"); });
        goRightBtn.onClick.AddListener(delegate() { MoveOnPhone("right"); });*/

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
		
	}

    [Command]
    public void CmdFire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        //faire apparaitre la balle sur les clients
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }

    public void Chut()
    {
        Debug.Log("Chut");
    }

    void MoveOnPhone(string direction)
    {
        moveX = 0;
        moveZ = 0;
        if (direction == "left")
        {
            moveX -= 1f;
            Debug.Log("Gauche");
        }
        else if (direction == "right")
        {
            moveX += 1f;
            Debug.Log("Droite");
        }
        else if (direction == "up")
        {
            moveZ += 1f;
        }
        else if (direction == "down")
        {
            Debug.Log("bas");
            moveZ -= 1f;
        }
        else if (direction == "fire")
        {
            Debug.Log("tirer");
            CmdFire();
        }
        else
            Debug.Log("not working cheikna");

        transform.Rotate(0, moveX * 5, 0);
        transform.Translate(0, 0, moveZ);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;

        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            string tag = button.tag;
            button.onClick.AddListener(delegate { MoveOnPhone(tag); });

            if (tag == "fire")
            {
                fireButton = button;
            }
            else if (tag == "down")
            {
                goDownBtn = button;
            }
            else if (tag == "left")
            {
                goLeftBtn = button;
            }
            else if (tag == "right")
            {
                goRightBtn = button;
            }
            else if (tag == "up")
            {
                goUpBtn = button;
            }

        }
    }
}
