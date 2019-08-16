using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  22 避開領導者的前方
public class EvadeController : MonoBehaviour {

    public GameObject leader;
    private Vehicle leaderLocomotion;
    private Vehicle m_vehicle;
    private bool IsPlanar;
    private Vector3 leaderAhead;
    private float LEADER_BEHIND_DIST;
    private Vector3 dist;
    public float evadeDistance;
    private float sqrEvadeDistance;
    private SteeringForEvade evadeScript;

    private void Start()
    {
        leaderLocomotion = leader.GetComponent<Vehicle>();
        evadeScript = GetComponent<SteeringForEvade>();
        m_vehicle = GetComponent<Vehicle>();
        IsPlanar = m_vehicle.isPlanar;
        LEADER_BEHIND_DIST = 2.0f;
        sqrEvadeDistance = evadeDistance * evadeDistance;
    }

    private void Update()
    {
        //計算領隊前方的點
        leaderAhead = leader.transform.position + leaderLocomotion.velocity.normalized * LEADER_BEHIND_DIST;
        //計算角色目前位置與領隊前方某點的距離，如果小於某個值，就需要躲避
        dist = transform.position - leaderAhead;
        if (IsPlanar)
            dist.y = 0;
        if(dist.sqrMagnitude < sqrEvadeDistance)
        {
            //如果小於躲避距離，啟動躲避行為
            evadeScript.enabled = true;
            Debug.DrawLine(transform.position, leader.transform.position);
        }
        else
        {
            //躲避行為處於於非啟動狀態
            evadeScript.enabled = false;
        }
    }
}
