using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerTake : MonoBehaviour {
    public GameObject p_Hand;
    public bool isTake;
    MeshRenderer[] meshRenderer = new MeshRenderer[2];

	// Use this for initialization
	void Start () {
        for(int i =0;i<transform.childCount;i++)
        {
            meshRenderer[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (isTake)
            FollowOtherHand();
        else
        {
            ShowOffRender();
        }
            //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void FollowOtherHand()
    {
        //this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].enabled = true;
        }
        this.transform.position = new Vector3( p_Hand.transform.position.x + 0.04f, p_Hand.transform.position.y - 0.055f, p_Hand.transform.position.z +0.05f);
        this.transform.rotation = new Quaternion(40,90,0,0);
    }

    void ShowOffRender()
    {
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].enabled = false;
        }
        this.transform.position = new Vector3(p_Hand.transform.position.x + 0.04f, p_Hand.transform.position.y - 0.055f, p_Hand.transform.position.z + 0.05f);
        this.transform.rotation = new Quaternion(40, 90, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jar")
        {
            Destroy(other.gameObject, 0.3f);
            isTake = true;
        }
            
    }
}
