using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//07追逐
public class SteeringForPursuit : Steering
{
    public GameObject target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    public float slowDownDistance;


    void Start ()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
		
	}

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;

        //計算追逐者的正向與逃避者正向的夾角
        float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);
        
        //如果夾角大於0，且追逐者基本面對逃避者。那麼直接向逃避者目前位置移動
        if(Vector3.Dot(toTarget,transform.forward)>0 && (relativeDirection<-0.95f))
        {
            //計算預期速度
            desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
            //傳回操控向量
            return (desiredVelocity - m_vehicle.velocity);
        }

        //計算預測時間，正比於追逐者與逃避者的距離，反比於追逐者和逃避者的速度和
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);

        //計算預期速度
        desiredVelocity = (target.transform.position + target.GetComponent<Vehicle>().velocity * lookaheadTime - transform.position).normalized * maxSpeed;
        
        //傳回操控向量
        //return (desiredVelocity - m_vehicle.velocity);

        //改進版本 會剎車
        float distance = toTarget.magnitude;
        if (distance > slowDownDistance)
        {
            return (desiredVelocity - m_vehicle.velocity);
        }
        else
        {
            //計算預期速度，並傳回預期速度與目前速度的差
            desiredVelocity = toTarget - m_vehicle.velocity;
            //傳回預期速度與目前速度的差
            return (desiredVelocity - m_vehicle.velocity);
        }

    }

}
