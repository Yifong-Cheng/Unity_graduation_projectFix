using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 24 指令稿
// 用於實現排隊行為
public class SteeringForQueue : Steering
{
    public float MAX_QUEUE_AHEAD;
    public float MAX_QUEUE_RADIUS;
    private Collider[] colliders;
    public LayerMask layersChecked;
    private Vehicle m_vehicle;
    private int layerid;
    private LayerMask layerMask;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        layerid = LayerMask.NameToLayer("vehicles");
        layerMask = 1 << layerid;
    }

    public override Vector3 Force()
    {
        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        //計算出角色前方的一點
        Vector3 ahead = transform.position + normalizedVelocity * MAX_QUEUE_AHEAD;
        //如果以ahead點為中心，MAX_QUEUE_RADIUS的球體內有其他角色;
        colliders = Physics.OverlapSphere(ahead, MAX_QUEUE_RADIUS, layerMask);
        if(colliders.Length > 0 )
        {
            //對於所有位於此球體內的其他角色，如果他們的速度比目前角色速度更慢，
            //目前角色放慢速度，避免發生碰撞
            foreach(Collider c in colliders)
            {
                if( (c.gameObject != this.gameObject) && (c.gameObject.GetComponent<Vehicle>().velocity.magnitude < velocity.magnitude))
                {
                    m_vehicle.velocity *= 0.8f;
                    break;
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
}
