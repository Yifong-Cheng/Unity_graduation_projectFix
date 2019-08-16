using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//05離開
public class SteeringForFlee : Steering
{
    public GameObject target;
    //設定使AI角色意識到危險並開始逃跑的範圍
    public float fearDistance = 20;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
	
	void Start ()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
	}

    public override Vector3 Force()
    {
        Vector3 tmpPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 tmpTargetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        //如果AI角色與目標的距離大於逃跑距離，那麼傳回0向量
        if(Vector3.Distance(tmpPos,tmpTargetPos) > fearDistance)
        {
            return new Vector3(0, 0, 0);
        }
        //如果AI角色與目標小於逃跑距離，那麼計算逃跑所需的操控向量
        desiredVelocity = (transform.position - target.transform.position).normalized * maxSpeed;
        return (desiredVelocity - m_vehicle.velocity);


    }


}
