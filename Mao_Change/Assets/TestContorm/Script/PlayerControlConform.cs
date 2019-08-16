using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlConform : MonoBehaviour
{
    public GameObject HandObj;
    public GameObject FrontObj;
    public Transform m_Hand;
    private float length = 3f;
    private Animator m_anim;

    private float stayTime = 0f;
    private float LimitTime = .4f;
    private int State;

    public GameObject MainProp;

    // Use this for initialization
    void Start()
    {
        //MainProp = GameObject.Find("MainProp");
        m_anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        
        switch (State)
        {
            case 0:
                if (HandObj == null)
                {
                    PickProp(MainProp.transform.GetChild(0).gameObject);
                }
                else
                {
                    ChangeHandProp();
                }
                break;

            case 1:
                if (HandObj == null)
                {

                }
                else
                {
                    ForWearProp();
                }
                break;

            default:
                break;
        }
    }

    void PickProp(GameObject parent)
    {
        FrontObj.transform.position = m_Hand.transform.position;
        HandObj = FrontObj;
        HandObj.transform.parent = m_Hand;
        HandObj.GetComponent<Rigidbody>().isKinematic = true;

        string PropName = HandObj.GetComponent<ObjInfo>().PropName;
        GameObject go = Resources.Load<GameObject>("SampleProp");
        go.GetComponent<PropInfoControl>().ImageInfoName = PropName;
        Instantiate(go, parent.transform.position + Vector3.zero, Quaternion.identity, parent.transform);

    }

    void ChangeHandProp()
    {
        HandObj.transform.parent = null;
        HandObj.GetComponent<Rigidbody>().isKinematic = false;
        HandObj = null;
        FrontObj.transform.position = m_Hand.transform.position;
        HandObj = FrontObj;
        HandObj.transform.parent = m_Hand;
        HandObj.GetComponent<Rigidbody>().isKinematic = true;
    }

    void UseProp()
    {

    }

    void ForWearProp()
    {
        HandObj.transform.parent = null;
        HandObj.GetComponent<Rigidbody>().isKinematic = false;
        HandObj = null;
    }

    #endregion

    #region GetPropInfo

    public void GetPropInfo()
    {
        if(MainProp.transform.GetChild(0).childCount!=0)
        {
            PropInfoControl m_propInfo = MainProp.transform.GetChild(0).GetChild(0).GetComponent<PropInfoControl>();
            ChangeRealProp(m_propInfo.ImageInfoName);
            Debug.Log(m_propInfo.ImageInfoName);
        }
        else
        {
            Debug.Log("Null Prop");
            ToPropBox();
        }
    }

    void ChangeRealProp(string Propname)
    {
        string path = "TestProps/" + Propname;
        if (Resources.Load<GameObject>(path) !=null)
        {
            Destroy(HandObj.gameObject);
            GameObject go = Resources.Load<GameObject>(path);
            HandObj = Instantiate(go, m_Hand.transform.position, Quaternion.identity, m_Hand);
            HandObj.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void ToPropBox()
    {
        
        Destroy(HandObj.gameObject);
    }

    #endregion

    #region ShowInfo_OnGUI

    void OnGUI()
    {

    }

    #endregion

}
