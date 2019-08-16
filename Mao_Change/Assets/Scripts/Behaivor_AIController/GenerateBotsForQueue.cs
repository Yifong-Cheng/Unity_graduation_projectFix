using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 23 指令稿
public class GenerateBotsForQueue : MonoBehaviour {

    public GameObject botPrefab;
    public int botCount;
    public GameObject target;
    public float minX = 0f;
    public float maxX = 50f;
    public float minZ = 0f;
    public float maxZ = 50f;
    public float Yvalue = 4f;

    private void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;
        //在定義範圍內隨機產生多個角色;
        //為產生的角色指定目標;
        for(int i = 0; i<botCount; i++)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), Yvalue, Random.Range(minZ, maxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity) as GameObject;
            bot.GetComponent<SteeringForArrive>().target = target;
        }
    }
}
