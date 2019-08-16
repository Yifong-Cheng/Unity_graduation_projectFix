using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//04接近
public class SteeringForSeek : Steering
{
    //需要尋找的目標物體;
    public GameObject target;
    //預期速度;
    private Vector3 desiredVelocity;
    //獲得被操控AI角色，以便查詢這個AI角色的最大速度等資訊;
    private Vehicle m_vehicle;
    //最大速度;
    private float maxSpeed;
    //是否在二維平面上運動;
    private bool isPlanar;

    void Start () {
        //獲得被操控AI角色，並讀取AI角色的允許最大速度，是否僅在平面上運動
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
    }


    //計算操控向量(操控力)
    public override Vector3 Force()
    {
        //計算預期速度
        desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
        if (isPlanar)
            desiredVelocity.y = 0;
        //傳回操控向量，即預期速度與目前速度的差
        return (desiredVelocity - m_vehicle.velocity);

    }
}
