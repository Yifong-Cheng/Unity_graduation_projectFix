using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMats : MonoBehaviour {
    private GameObject m_player;
    private OLDPlayerController m_controller;

    public string EnergyString = "Energy";
    public string Mat_0String = "Mat0";
    public string Mat_1String = "Mat1";
    public string Mat_2String = "Mat2";


    // Use this for initialization
    void Start () {
        m_player = transform.parent.gameObject;
        m_controller = m_player.GetComponent<OLDPlayerController>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ObjInfo>()!=null)
        {
            if(other.GetComponent<ObjInfo>().Type == EnergyString)
            {
                Destroy(other.gameObject, 0.5f);
                m_controller.m_bgInfo.Instance.Enegy += 100;
                Debug.Log(m_controller.m_bgInfo.Instance.Enegy);
            }
            else if(other.GetComponent<ObjInfo>().Type == Mat_0String)
            {
                Destroy(other.gameObject, 0.5f);
                m_controller.m_bgInfo.Instance.Mat0 += 1;
                Debug.Log(m_controller.m_bgInfo.Instance.Mat0);
            }
        }
    }


}
