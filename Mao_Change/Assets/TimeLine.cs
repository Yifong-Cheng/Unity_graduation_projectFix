using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
public class TimeLine : MonoBehaviour {


    public PlayableDirector playableDirector;
    private CameraFollow camera;
    private bool played = false;
    public CinemachineStateDrivenCamera statemachine;

    // Use this for initialization
    void Awake () {
		playableDirector = this.GetComponent<PlayableDirector>();
        camera = GameObject.FindObjectOfType<CameraFollow>();

        statemachine = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>();

    }

    // Update is called once per frame
    void Update () {
        if (playableDirector.state == PlayState.Paused && played == true)
        {
            camera.enabled = true;
            
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && played == false)
        {
            camera.enabled = false;
            playableDirector.Play();
            played = true;
        }
    }

    public void FlagControl()
    {
        
        //statemachine.m_AnimatedTarget = GameObject.FindObjectOfType<NewPlayerController>().FlagPlayer.GetComponent<Animator>();
        //GameObject.FindObjectOfType<NewPlayerController>().m_anim.Play("Command",0,-1.0);


    }
}
