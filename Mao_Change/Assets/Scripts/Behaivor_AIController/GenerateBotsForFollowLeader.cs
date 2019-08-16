using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 20 用於產生多個跟隨者
public class GenerateBotsForFollowLeader : MonoBehaviour
{

    public GameObject botPrefab;
    public GameObject leader;
    public int botCount;
    ////長方體"盒子"定義了產生AI的初始位置
    public float minX = 88.0f;
    public float maxX = 150.0f;
    public float minZ = -640f;
    public float maxZ = -590.0f;
    public float Yvalue = 1.026003f;

    private void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;
        for(int i=0; i<botCount; i++)
        {
            //隨機產生一個產生位置，產生實體預設物體
            spawnPosition = new Vector3(Random.Range(minX, maxX), Yvalue, Random.Range(minZ, maxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity) as GameObject;
            bot.GetComponent<SteeringForLeaderFollowing>().leader = leader;
            bot.GetComponent<SteeringForEvade>().target = leader;
            bot.GetComponent<SteeringForEvade>().enabled = false;
            bot.GetComponent<EvadeController>().leader = leader;
        }
    }
}
