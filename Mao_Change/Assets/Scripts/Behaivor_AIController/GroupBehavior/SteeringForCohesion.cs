using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 16 聚集
//成群聚集再一起
public class SteeringForCohesion : Steering
{
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
    }
    public override Vector3 Force()
    {
        //操控向量
        Vector3 steeringForce = new Vector3(0, 0, 0);
        //AI角色的所有鄰居的質心，即平均位置
        Vector3 centerOfMass = new Vector3(0, 0, 0);
        //AI角色的鄰居數量
        int neighborCount = 0;
        //檢查鄰居列表中的每個鄰居
        foreach(GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果S不是目前AI角色
            if(s!=null && (s!=this.gameObject))
            {
                //累加S的位置
                centerOfMass += s.transform.position;
                //鄰居數加1
                neighborCount++;
            }
        }
        //如果鄰居數量大於0
        if(neighborCount > 0)
        {
            //將位置的累積質除以鄰居數量，獲得平均值
            centerOfMass /= (float)neighborCount;
            //預期速度為鄰居位置平均質與目前位置之差
            desiredVelocity = (centerOfMass - transform.position).normalized * maxSpeed;
            //預期速度減去目前速度，求出操控向量
            steeringForce = desiredVelocity - m_vehicle.velocity;
        }

        return steeringForce;
    }
}
