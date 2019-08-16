using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PlayerAnimationTest : MonoBehaviour {
    Animation anim;
    public PlayableDirector playableDirector;
    bool PlayFirstTime;
    public GameObject tub;
    public GameObject MainModel;
    public GameObject boostplayer;
    public GameObject boostTimeline;

    float looptime,currenttime;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();
        playableDirector = GetComponent<PlayableDirector>();
        //anim["Idle"].layer = 123;
        currenttime = 0;
        looptime = 1f;


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !PlayFirstTime)
        {
            PlayFirstTime = true;
            Debug.Log("1");
            MainModel.SetActive(false);
            playableDirector.Play();
            
            
        }
        else if(Input.GetKeyDown(KeyCode.Space)&&PlayFirstTime)
        {

            boostTimeline.SetActive(false);
            //boostplayer.transform.position = new Vector3(boostplayer.transform.position.x, boostplayer.transform.localPosition.y, boostplayer.transform.position.z);
            boostplayer.SetActive(true);
            boostplayer.GetComponent<Animator>().SetBool("Boost", true);
            Debug.Log(boostplayer.GetComponent<Animator>().HasState(0, 0));
        }
        else
        {
            boostplayer.GetComponent<Animator>().SetBool("Boost", false);
            if (boostplayer.GetComponent<Animator>().HasState(0, 0))
            {
                boostplayer.SetActive(false);
            }
        }
        
        //Debug.Log(playableDirector.initialTime);
        Zero();

    }

    void EnternallyPush()
    {
        currenttime = 0;
    }

    void Zero()
    {
        runtime();
        if(currenttime>looptime)
        {
            currenttime = 0;
            
        }
    }

    float runtime()
    {
        return currenttime += Time.deltaTime;
    }
}
