using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 06 抵達
public class SteeringForArrive : Steering
{

    public bool IsPlanar = true;
    public float arrivalDistance = 0.3f;
    public float characterRadius = 1.2f;
    //當目標小於這個距離時，開始減速
    public float slowDownDistance;

    public GameObject target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;

	void Start ()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        IsPlanar = m_vehicle.isPlanar;
	}

    public override Vector3 Force()
    {
        //計算AI角色與目標之間的距離
        Vector3 toTarget = target.transform.position - transform.position;

        //預期速度
        Vector3 desiredVelocity;

        //傳回的操控向量
        Vector3 returnForce;
        if (IsPlanar) toTarget.y = 0;
        float distance = toTarget.magnitude;

        //如果與目標之間的距離大於所設定的減速半徑
        if(distance > slowDownDistance)
        {
            //預期速度是AI角色與目標之間的距離
            desiredVelocity = toTarget.normalized * maxSpeed;
            //傳回預期速度與目前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
        }
        else
        {
            //計算預期速度，並傳回預期速度與目前速度的差
            desiredVelocity = toTarget - m_vehicle.velocity;
            //傳回預期速度與目前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
        }

        return returnForce;
    }

    private void OnDrawGizmos()
    {
        if(target!= null)
        {
            //在目標周圍話白色的線框球，顯示出減速範圍
            Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
        }
       
    }

}
