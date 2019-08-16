using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PutAndPush : MonoBehaviour
{
    public GameObject HandObj;
    public GameObject FrontObj;
    public Transform m_Hand;
    private float length = 3f;
    private Animator m_anim;

    private float stayTime = 0f;
    private float LimitTime = .4f;
    private int State;

    // Use this for initialization
    void Start () {
        m_anim = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            stayTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (stayTime > LimitTime)
            {
                State = 1;
                stayTime = 0;
            }
            else
            {
                State = 0;
                stayTime = 0;
            }
            MouseState();
        }
    }

    #region use

    void MouseState()
    {
        GameObject info = null;
        info = HandObj;
        switch (State)
        {
            case 0:
                //Debug.Log("0");
                if(info==null)
                {

                    FrontObj.transform.position = m_Hand.transform.position;
                    HandObj = FrontObj;
                    HandObj.transform.parent = m_Hand;
                    HandObj.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    HandObj.transform.parent=null;
                    HandObj.GetComponent<Rigidbody>().isKinematic = false;
                    HandObj = null;
                    FrontObj.transform.position = m_Hand.transform.position;
                    HandObj = FrontObj;
                    HandObj.transform.parent = m_Hand;
                    HandObj.GetComponent<Rigidbody>().isKinematic = true;
                }
                break;

            case 1:
                //Debug.Log("1");
                if (info == null)
                {
                }
                else
                {
                    HandObj.transform.parent = null;
                    HandObj.GetComponent<Rigidbody>().isKinematic = false;
                    HandObj = null;
                    //FrontObj.transform.position = m_Hand.transform.position;
                    //HandObj = FrontObj;
                    //HandObj.transform.parent = m_Hand;
                    //HandObj.GetComponent<Rigidbody>().isKinematic = true;
                }
                break;

            default:
                break;
        }
    }

    #endregion

    #region ShowInfo_OnGUI

    void OnGUI()
    {
        GUI.Box(new Rect(10, 200, 400, 150), "DebugLog");

        GUI.TextField(new Rect(20, 240, 180, 20), string.Format("StayTime: {0}", stayTime));
        GUI.TextField(new Rect(20, 280, 180, 20), string.Format("State: {0}", State));

    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
