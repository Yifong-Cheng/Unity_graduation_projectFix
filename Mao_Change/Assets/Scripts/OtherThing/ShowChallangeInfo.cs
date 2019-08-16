using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowChallangeInfo : MonoBehaviour {
    public int currentChallange;
    public int totalChallange;
    public Text currentText;
    public Text totalText;

	// Use this for initialization
	void Start () {
        currentText.text = currentChallange.ToString();
        totalText.text = totalChallange.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetChallangeText()
    {
        currentText.text = currentChallange.ToString();
        totalText.text = totalChallange.ToString();
    }
}
