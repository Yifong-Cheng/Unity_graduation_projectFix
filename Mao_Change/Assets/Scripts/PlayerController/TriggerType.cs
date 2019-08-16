using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerType : MonoBehaviour {
    public string ColliderSoilderTypeName = "Soilder";
    private OLDPlayerController playerController;
    private GameObject Followers;
    // Use this for initialization
    void Start () {
        playerController = GameObject.Find("Player").GetComponent<OLDPlayerController>();
        Followers = GameObject.Find("Followers");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Script_TriggerType");
        //if(other.GetComponent<ObjInfo>()!=null)
        //{
        //    if (other.GetComponent<ObjInfo>().Type == ColliderSoilderTypeName)
        //    {
        //        //playerController.ColliderTypeEvent(other.GetComponent<ObjInfo>().Profession);
        //        if (other.GetComponent<AIStateController>().m_aiState == AIStateController.AIState.IDLE)
        //        {
        //            other.transform.parent = Followers.transform.GetChild(other.GetComponent<ObjInfo>().FollowerTypeID);
        //            other.GetComponent<AIStateController>().m_aiState = AIStateController.AIState.WALK;
        //        }
        //        else if (other.GetComponent<AIStateController>().m_aiState == AIStateController.AIState.ATTACK)
        //        {
        //            other.transform.parent = Followers.transform.GetChild(other.GetComponent<ObjInfo>().FollowerTypeID);
        //            other.GetComponent<AIStateController>().m_aiState = AIStateController.AIState.WALK;
        //        }
        //        else if (other.GetComponent<AIStateController>().m_aiState == AIStateController.AIState.AutoCreat)
        //        {
        //            other.transform.parent = Followers.transform.GetChild(other.GetComponent<ObjInfo>().FollowerTypeID);
        //            other.GetComponent<AIStateController>().m_aiState = AIStateController.AIState.WALK;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        playerController.UpdateFollowerTeamList();

        //    }
        //}
    }
}
