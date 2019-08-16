using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 15 團隊
//與群體中鄰居朝向一致
public class SteeringForAlignment : Steering
{
    private void Start()
    {
        
    }
    public override Vector3 Force()
    {
        //目前AI角色的鄰居平均朝向
        Vector3 averageDirection = new Vector3(0, 0, 0);
        //鄰居的數量
        int neighborCount = 0;
        //檢查目前AI角色的所有鄰居
        foreach(GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果s不是目前AI角色
            if(s!=null && (s!=this.gameObject))
            {
                //將s地朝向向量加到averageDirection之中
                averageDirection += s.transform.forward;
                //鄰居數量+1
                neighborCount++;
            }

        }

        //如果鄰居數量大於0
        if(neighborCount>0)
        {
            //將累加獲得地朝向向量除以鄰居的個數，求出平均朝向向量
            averageDirection /= (float)neighborCount;
            //平均朝向向量減去目前朝向向量，獲得操控向量
            averageDirection -= transform.forward;
        }
        return averageDirection;
    }
}
