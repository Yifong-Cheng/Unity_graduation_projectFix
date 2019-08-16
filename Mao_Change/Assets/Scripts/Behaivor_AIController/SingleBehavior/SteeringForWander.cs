using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//09隨機徘徊
//需要注意，這個函數方法的效果與每秒顯示畫面相關
public class SteeringForWander : Steering
{
    //徘徊半徑，即Wander圈的半徑
    public float wanderRadius;
    //徘徊距離，即Wander圈突出在AI角色前面的距離
    public float wanderDistance;
    //每秒加到目標的隨機位移最大值
    public float wanderJitter;
    public bool IsPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;

    void Start ()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        IsPlanar = m_vehicle.isPlanar;
        //選取圓圈上的點作為初始點
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
		
	}

    public override Vector3 Force()
    {
        //計算隨機位移
        Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 * wanderJitter, (Random.value - 0.5f) * 2 * wanderJitter, (Random.value - 0.5f) * 2 * wanderJitter);
        if (IsPlanar)
            randomDisplacement.y = 0;
        //將隨機位移加到初始點上，獲得新的位置
        circleTarget += randomDisplacement;
        //由於新位置可能不在圓周上，因此需要投影到圓周上
        circleTarget = wanderRadius * circleTarget.normalized;
        //之前計算出的直視相對於AI角色和AI角色地向前方向的，需要轉換為世界座標
        wanderTarget = m_vehicle.velocity.normalized * wanderDistance + circleTarget + transform.position;
        //計算預期速度，傳回操控向量
        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity-m_vehicle.velocity);
    }
}
