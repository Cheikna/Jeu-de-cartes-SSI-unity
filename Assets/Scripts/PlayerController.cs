using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public Transform bulletSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

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

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
