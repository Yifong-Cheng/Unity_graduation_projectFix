using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBox : MonoBehaviour {
    GameObject player;
    NewPlayerController p;
    // Use this for initialization
    void Start()
    {
        player = this.transform.parent.gameObject;
        p = player.GetComponent<NewPlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Seed")
        {
            //player.GetComponent<PlayerControlConform>().FrontObj = other.gameObject;
            //if(other.GetComponent<ObjInfo>().Type=="Seed")
            //{
            //    player.GetComponent<NewPlayerController>().FrontObj = other.gameObject;
            //}
            p.FrontObj = other.gameObject;
            p.PickUp();
        }
        if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Crystal")
        {
           
            p.FrontObj = other.gameObject;
            p.PickUp();
        }
        //if (other.tag == "Hole")
        //{
        //    p.FrontObj = other.gameObject;
        //}

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Seed")
        {
            p.FrontObj = null;
        }
        if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Crystal")
        {
            p.FrontObj = null;
        }
        //if (other.tag == "Hole")
        //{
        //    p.FrontObj = null;
        //}
        
    }
}
