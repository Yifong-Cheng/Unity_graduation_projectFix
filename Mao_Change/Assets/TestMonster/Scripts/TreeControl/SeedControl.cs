using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeedControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Destroy(this.gameObject, 5f);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position -= new Vector3(0, 0.5f, 0) * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this.gameObject, 5f);
    }
}
