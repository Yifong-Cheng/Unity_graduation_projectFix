using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public List<Camera> cameras = new List<Camera>();
    private GameObject art;
    public GameObject PlayerTarget;

    private int CameraIndex;
    public int currentIndex;
    private int lastCurrentIndex;

    // Use this for initialization
    void Start () {
        art = PlayerTarget.transform.GetChild(0).gameObject;
        CameraIndex = cameras.Count;
        currentIndex = 0;
        lastCurrentIndex = currentIndex;
    }
	
	// Update is called once per frame
	void LateUpdate() {

        //Debug.Log("PLayerArt _ x = "+ art.transform.rotation.x);

        //Debug.Log("PLayerArt _ y = " + art.transform.rotation.y);

        //Debug.Log("PLayerArt _ z = " + art.transform.rotation.z);

        //if (art.transform.rotation.y > 0 && art.transform.rotation.y <= 0.5f)
        //{
        //    currentIndex = 0;

        //}
        //else if (art.transform.rotation.y > 0.5f && art.transform.rotation.y < 1)
        //{
        //    currentIndex = 3;
        //}
        //else if (art.transform.rotation.y > -0.5 && art.transform.rotation.y <0)
        //{
        //    currentIndex = 1;
        //}
        //else if (art.transform.rotation.y >-1 && art.transform.rotation.y <-0.5)
        //{
        //    currentIndex = 2;
        //}

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(currentIndex<CameraIndex)
            {
                if(currentIndex< CameraIndex-1)
                {
                    currentIndex++;
                }else if(currentIndex==CameraIndex-1)
                {
                    currentIndex = 0;
                }
            }
        }

        if (currentIndex != lastCurrentIndex)
        {
            lastCurrentIndex = currentIndex;

        }
        else
        {
            for (int i = 0; i < CameraIndex; i++)
            {
                if (currentIndex == i)
                {
                    cameras[i].enabled = true;
                }
                else
                {
                    cameras[i].enabled = false;
                }

            }
        }

    }
}
