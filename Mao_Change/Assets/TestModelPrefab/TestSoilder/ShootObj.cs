using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObj : MonoBehaviour {
    float lifetime = 1.5f;
    int power = 20;
    int gravity = 1;
    AudioController audioController;
	// Use this for initialization
	void Start () {
        audioController = GetComponent<AudioController>();
        StartCoroutine(audioController.PlaySound(0, .5f));
        Destroy(this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position += (Vector3.forward * power)*Time.deltaTime;
        //applyGravity();
	}

    void applyGravity()
    {
        this.transform.position -= new Vector3(0, gravity*Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(other.GetComponent<ObjInfo>()!=null)
        {
            if (other.GetComponent<ObjInfo>().Type == "Enemy")
            {
                other.GetComponent<PlayerHealth>().TakeDamage(2, 80);
            }
            else if(other.GetComponent<ObjInfo>().Type == "Soilder"&& other.GetComponent<ObjInfo>().FollowerTypeID ==1)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(10, 100);
            }
            //Destroy(this.gameObject);
            //audioController.PlaySound(1);
        }
        
    }
}
