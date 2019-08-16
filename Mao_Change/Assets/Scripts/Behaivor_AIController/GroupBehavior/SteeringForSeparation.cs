using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 14 分離
//與群中鄰居保持適當距離
public class SteeringForSeparation : Steering
{
    //可接受距離
    public float comfortDistance = 1;
    //當AI角色與鄰居之間距離過近十的懲罰因數
    public float multiplierInsideComfortDistance = 2;
    private void Start()
    {
        
    }
    public override Vector3 Force()
    {
        Vector3 steeringForce = new Vector3(0, 0, 0);

        //檢查此AI角色的崊居列表的每個鄰居
        foreach(GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果s不是目前AI角色
            if(s!=null &&(s!= this.gameObject))
            {
                //計算目前AI角色與鄰居S之間的距離
                Vector3 toNeighbor = transform.position - s.transform.position;
                float length = toNeighbor.magnitude;

                //計算這個鄰居引起的操控力
                steeringForce += toNeighbor.normalized / length;

                //如果兩者之間距離大於可接受之距離，排斥力再乘以一個額外因數
                if (length < comfortDistance)
                    steeringForce *= multiplierInsideComfortDistance;
            }
        }
        return steeringForce;
    }

}
