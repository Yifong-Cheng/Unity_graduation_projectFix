using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 21 跟隨者
[RequireComponent(typeof(SteeringForArrive))]
public class SteeringForLeaderFollowing : Steering
{
    public Vector3 target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private bool IsPlanar;
    //領隊遊戲體
    public GameObject leader;
    //領隊的控制指令搞
    private Vehicle leaderController;
    private Vector3 leaderVelocity;
    //跟隨者落後領隊的距離
    private float LEADER_BEHIND_DIST = 2.0f;
    private SteeringForArrive arriveScript;
    private Vector3 randomOffset;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        IsPlanar = m_vehicle.isPlanar;
        leaderController = leader.GetComponent<Vehicle>();

        //為抵達行為指定目標點
        arriveScript = GetComponent<SteeringForArrive>();
        arriveScript.target = new GameObject("arriveTarget");
        arriveScript.target.transform.position = leader.transform.position;

    }

    public override Vector3 Force()
    {
        leaderVelocity = leaderController.velocity;
        //計算目標點
        target = leader.transform.position + LEADER_BEHIND_DIST * (-leaderVelocity).normalized;
        arriveScript.target.transform.position = target;
        return new Vector3(0, 0, 0);
    }

}
