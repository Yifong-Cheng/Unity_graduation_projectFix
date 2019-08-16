using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour {
    public AudioClip[] Sounds;
    
    private AudioSource audioSource;

    private string SceneName;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
    }

    public IEnumerator PlaySound(int soundIndex,float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        audioSource.clip = Sounds[soundIndex];
        audioSource.Play();
    }

    public void PlaySound(int soundIndex)
    {
        if (audioSource.isPlaying)
        {
            //audioSource.Stop();
            //audioSource.clip = Sounds[soundIndex];
            //audioSource.Play();
        }
        else
        {
            audioSource.clip = Sounds[soundIndex];
            audioSource.Play();
        }
        
    }

    public void StopSound(int soundIndex)
    {
        if(audioSource!=null)
        {
            if (audioSource.clip != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            else if (audioSource.clip == null)
            {

            }
        }
    }

    public void StopIndexSound(int soundIndex)
    {
        if (audioSource != null)
        {
            if (audioSource.clip != null)
            {
                if (audioSource.clip==Sounds[soundIndex])
                {
                    audioSource.Stop();
                }
            }
        }
    }

    public void PlayAudioClip(int soundIndex,float volume)
    {
        audioSource.clip = Sounds[soundIndex];
        audioSource.PlayOneShot(Sounds[soundIndex], volume);
        
    }
}
