using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPosActionTrigger : MonoBehaviour {
    public string PlayerObjName = "Player";
    private OLDPlayerController m_player;
    // Use this for initialization
    void Start () {
        m_player = GameObject.Find(PlayerObjName).GetComponent<OLDPlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "SU@idle")
        {
            m_player.ReturnPosIndex++;
        }
    }

}
