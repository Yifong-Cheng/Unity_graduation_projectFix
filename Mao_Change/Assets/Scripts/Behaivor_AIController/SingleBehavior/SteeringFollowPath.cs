using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//10路徑跟隨
//沿著預設的軌跡，組成路徑的一系列路點移動
public class SteeringFollowPath : Steering
{
    //由節點陣列表示的路徑
    public GameObject[] waypoints = new GameObject[4];
    //目標點
    private Transform target;
    //目前的路點
    private int currentNode;
    //與路點的最小距離小於這個值時，認為已到達，可以朝下一個路點前進
    private float arriveDistance;
    private float sqrArriveDistance;
    //路點的數量
    private int numberofNodes;
    //操控力
    private Vector3 force;
    //預期速度
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private bool IsPlanar;
    //當與目標小於這個距離時，開始減速
    public float slowDownDistance;

	void Start ()
    {
        //儲存路點陣列中的路點個數
        numberofNodes = waypoints.Length;
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        IsPlanar = m_vehicle.isPlanar;
        //設定目前路點為第0個路點
        currentNode = 0;
        //設定目前路點為目標點
        target = waypoints[currentNode].transform;
        arriveDistance = 1.0f;
        sqrArriveDistance = arriveDistance * arriveDistance;
	}

    public override Vector3 Force()
    {
        force = new Vector3(0, 0, 0);
        Vector3 dist = target.position - transform.position;
        if (IsPlanar)
            dist.y = 0;
        //如果目前路點已經是路點陣列中最後一個路點
        if(currentNode == numberofNodes-1)
        {
            //如果目前路點的距離大於減速距離
            if (dist.magnitude > slowDownDistance)
            {
                //求出預期速度
                desiredVelocity = dist.normalized * maxSpeed;
                //計算操控向量
                force = desiredVelocity - m_vehicle.velocity;
            }
            else
            {
                //與目前路點距離小於減速距離，開始減速，計算操控向量
                desiredVelocity = dist - m_vehicle.velocity;
                force = desiredVelocity - m_vehicle.velocity;
            }
        }
        else
        {
            //目前路點不是最後一個路點，即正走向中間路點
            if(dist.sqrMagnitude < sqrArriveDistance)
            {
                //如果與目前路點距離的平方小於到達距離的平方，
                //可以開始接近下一個路點，將下一路點設為目標點
                currentNode++;
                target = waypoints[currentNode].transform;
            }
            //計算預期速度和操控向量
            desiredVelocity = dist.normalized * maxSpeed;
            force = desiredVelocity-m_vehicle.velocity;
        }
        return (force);
    }
}
