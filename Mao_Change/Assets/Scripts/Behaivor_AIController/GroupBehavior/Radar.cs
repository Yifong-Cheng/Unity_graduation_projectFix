using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 13 檢測附近的AI角色
//檢測目前AI角色的鄰居
public class Radar : MonoBehaviour {

    //碰撞體陣列
    private Collider[] colliders;
    //計時器
    private float timer = 0;
    //鄰居列表
    public List<GameObject> neighbors;
    //無須每畫面進行檢測，該變數設定檢測時間的間隔
    public float checkInterval = 0.3f;
    //設定鄰域半徑
    public float detectRadius = 10f;
    //設定檢測哪一層的遊戲物件
    public LayerMask layersChecked;

    private void Start()
    {
        //初始化鄰居列表
        neighbors = new List<GameObject>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //如果距離上次檢測時間大於所設定的檢測時間間隔，那麼再次檢測
        if(timer > checkInterval)
        {
            //清除鄰居列表
            neighbors.Clear();

            //尋找目前AI角色鄰域內所有碰撞體。
            colliders = Physics.OverlapSphere(transform.position, detectRadius, layersChecked);
            //對於每個檢測到的碰撞體，取的Vehicle元件，並且加入鄰居列表中。
            for(int i=0;i<colliders.Length;i++)
            {
                if (colliders[i].GetComponent<Vehicle>())
                    neighbors.Add(colliders[i].gameObject);
            }
            //計時器歸0
            timer = 0;
        }
    }

}
