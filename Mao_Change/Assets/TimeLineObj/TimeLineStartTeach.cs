using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineStartTeach : MonoBehaviour {
    public Story story;
    private void Awake()
    {
        //story = GameObject.FindObjectOfType<Story>();
    }
    // Use this for initialization
    void Start () {
        //StartTeach();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartTeach()
    {
        //Story story = GameObject.FindObjectOfType<Story>();
        story.StartTeachCanvas();
        
    }
}
