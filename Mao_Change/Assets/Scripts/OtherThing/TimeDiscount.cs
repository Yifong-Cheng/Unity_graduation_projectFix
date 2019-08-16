using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDiscount : MonoBehaviour {
    public float timecount;
    private float currentTimecount;
    private Text text;
    private GameObject Sign;
	// Use this for initialization
	void Start () {
        currentTimecount = timecount;
        
        Sign = transform.Find("Empty").gameObject;
        Sign.SetActive(false);
        text = Sign.transform.GetChild(0).GetComponentInChildren<Text>();
        StartCoroutine(DisCountTime());
	}
	
	// Update is called once per frame
	void Update () {
        
        //text.text = ((int)currentTimecount).ToString() + " S ";
	}

    IEnumerator DisCountTime()
    {
        
        while (currentTimecount>5)
        {
            currentTimecount -= Time.deltaTime;
            yield return null;
        }
        text.text = "請小心怪物突擊";
        text.color = Color.red;
        Sign.SetActive(true);
    }
}
