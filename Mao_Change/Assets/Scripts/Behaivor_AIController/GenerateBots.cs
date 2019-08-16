using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 18 多AI角色障礙賽
public class GenerateBots : MonoBehaviour {

    public GameObject botPrefab;
    public int botCount;
    public GameObject target;
    //長方體"盒子"定義了隨機產生AI的初始位置
    public float minX = 75.0f;
    public float maxX = 160f;
    public float minZ = -650.0f;
    public float maxZ = -600.0f;
    public float Yvalue = 4.043714f;
    private void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;
        for(int i=0; i<botCount;i++)
        {
            //隨機選擇一個產生點，產生實體預設物體
            spawnPosition = new Vector3(Random.Range(minX, maxX), Yvalue, Random.Range(minZ, maxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity) as GameObject;
            bot.GetComponent<SteeringForArrive>().target = target;
        }
    }
}
