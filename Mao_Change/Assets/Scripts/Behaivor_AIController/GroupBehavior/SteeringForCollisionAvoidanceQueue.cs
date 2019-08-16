using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 25 指令稿
// 用於碰撞避免的指令行為
public class SteeringForCollisionAvoidanceQueue : Steering
{
    public bool IsPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private float maxForce;
    public float avoidanceForce;
    public float MAX_SEE_AHEAD;
    private GameObject[] allColliders;
    private int layerid;
    private LayerMask layerMask;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxForce = m_vehicle.maxForce;
        IsPlanar = m_vehicle.isPlanar;
        maxSpeed = m_vehicle.maxSpeed;
        if(avoidanceForce > maxForce)
        {
            avoidanceForce = maxForce;
        }
        allColliders = GameObject.FindGameObjectsWithTag("obstacle");
        layerid = LayerMask.NameToLayer("obstacle");
        layerMask = 1 << layerid;
    }

    //計算避免碰撞所需的操控力，這裡利用了隱藏，只考慮場景中其他角色的碰撞
    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = new Vector3(0, 0, 0);
        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        if(Physics.Raycast(transform.position,normalizedVelocity,out hit,MAX_SEE_AHEAD,layerMask))
        {
            Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD;
            force = ahead - hit.collider.transform.position;
            force *= avoidanceForce;
            if(IsPlanar)
            {
                force.y = 0;
            }
        }
        return force;
    }

}
