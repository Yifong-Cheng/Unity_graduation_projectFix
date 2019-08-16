using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//11避開障礙
//當AI角色在行進路線發現障礙時，產生排斥力，躲避最近障礙物的操控力
public class SteeringForCollisionAvoidance : Steering
{
    public bool IsPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private float maxForce;
    //避免障礙產生的操控力
    public float avoidanceForce;
    //能向前看到的最大距離
    public float MAX_SEE_AHEAD = 2.0f;
    //場景中所有碰撞體組成的陣列
    private GameObject[] allColliders;

	void Start ()
    {

        m_vehicle = GetComponent<Vehicle>();
        IsPlanar = m_vehicle.isPlanar;
        maxForce = m_vehicle.maxForce;
        maxSpeed = m_vehicle.maxSpeed;

        //如果避免障礙所產生的操控力大於最大操控力，將他截斷到最大操控力
        if(avoidanceForce > maxForce)
        {
            avoidanceForce = maxForce;
        }

        //儲存所有場景中的碰撞體，即tag為obstacle的遊戲體
        allColliders = GameObject.FindGameObjectsWithTag("obstacle");
	}

    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = new Vector3(0, 0, 0);
        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizedVelocity = velocity.normalized;

        //畫出一條射線，需要檢測與這條射線相交的碰撞體
        Debug.DrawLine(transform.position,
                       transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed));
        if(Physics.Raycast(transform.position,normalizedVelocity,out hit, MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed)))
        {
            //如果射線與某個碰撞體香蕉，表示可能與該碰撞體發生碰撞
            Vector3 ahead = transform.position + normalizedVelocity *
            MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed);

            //計算避免碰撞所需的操控力
            force = ahead - hit.collider.transform.position;
            force *= avoidanceForce;
            if (IsPlanar)
                force.y = 0;

            //將此碰撞體的顏色變為綠色，其他都變為灰色
            //foreach(GameObject c in allColliders)
            //{
            //    if(hit.collider.gameObject == c)
            //    {
            //        Renderer render = c.GetComponent<Renderer>();
            //        Material mat = render.sharedMaterial;
            //        mat.color = Color.black;
            //    }
            //    else
            //    {
            //        Renderer render = c.GetComponent<Renderer>();
            //        Material mat = render.sharedMaterial;
            //        mat.color = Color.white;
            //    }
            //}
        }
        else
        {
            //如果向前看的有限範圍內，無發生碰撞之可能
            //將所有碰撞體設為灰色
            //foreach (GameObject c in allColliders)
            //{
            //    Renderer render = c.GetComponent<Renderer>();
            //    Material mat = render.sharedMaterial;
            //    mat.color = Color.white;
            //}
        }
        //傳回操控力
        return force;
    }

}
