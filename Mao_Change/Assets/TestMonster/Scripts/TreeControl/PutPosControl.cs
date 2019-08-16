using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutPosControl : MonoBehaviour {
    GameObject player;
	// Use this for initialization
	void Start () {
        player = this.transform.parent.gameObject;
	}


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ObjInfo>()!=null)
        {
            player.GetComponent<PutAndPush>().FrontObj = other.gameObject;
        }
    }
}
