using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineSyringeFirstCreat : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FirstCreat();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FirstCreat()
    {
        StageChallange stageChallange = GameObject.FindObjectOfType<StageChallange>();
        stageChallange.Startevent = true;
        stageChallange.TimeLineFirstPlay();
    }
}
