using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public GameObject Bullet;
    public Transform shootPos;

	// Use this for initialization
	void Start () {
        //InvokeRepeating("Shoot", .2f, 30f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, .8f, 0);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
	}

    private void Shoot()
    {
        GameObject go = GameObject.Instantiate(Bullet, shootPos.position+ shootPos.forward*5, Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
    }

}
