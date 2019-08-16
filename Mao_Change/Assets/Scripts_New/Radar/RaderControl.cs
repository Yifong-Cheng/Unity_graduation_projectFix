using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderControl : MonoBehaviour {

    //計時器
    private float timer = 0;

    //無須每畫面進行檢測，該變數設定檢測時間的間隔
    private float checkInterval = 0.3f;
    //設定鄰域半徑
    private float detectRadius = 3;

    public float lifetime;

    // Use this for initialization
    void Start () {
        Destroy(this.gameObject, lifetime*2);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        float radious = Expent(detectRadius, timer);
        this.transform.localScale = new Vector3(radious, .2f, radious);
    }

    private float Expent(float range, float timer)
    {
        int power = 2;
        return range * power * timer;
    }
}
