using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 19 領隊
//讓跟隨者保持在後方
public class DrawGizmos : MonoBehaviour {

    public float evadeDistance;
    //領隊前方的點
    private Vector3 center;
    private Vehicle vehicleScript;
    private float LEADER_BEHIND_DIST;

    private void Start()
    {
        //vehicleScript = GetComponent<Vehicle>();
        LEADER_BEHIND_DIST = 2.0f;
    }

    private void Update()
    {
       // center = transform.position + vehicleScript.velocity.normalized * LEADER_BEHIND_DIST;
        center = transform.position + gameObject.transform.forward*LEADER_BEHIND_DIST;
    }

    private void OnDrawGizmos()
    {
        //畫出一個位於領隊前方的線框球，如果其他角色進入這個販，需觸發逃避行為
        Gizmos.DrawSphere(center, evadeDistance);
    }
}
