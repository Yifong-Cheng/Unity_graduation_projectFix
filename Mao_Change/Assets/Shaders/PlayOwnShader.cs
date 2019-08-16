using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOwnShader : MonoBehaviour {
    int runtime;
    bool hit;
    float currenttime;
    float returntime = 10;
	// Use this for initialization
	void Start () {
        //StartCoroutine(ChangeShader(.1f));
	}
	
	// Update is called once per frame
	void Update () {
		if(hit)
        {
            currenttime += Time.deltaTime;
            if(currenttime>returntime)
            {
                hit = false;
                this.GetComponent<BoxCollider>().isTrigger = false;
                currenttime = 0;
                runtime = 0;
                GetComponent<Renderer>().material.SetFloat("_DissolveThreshold", 0);
            }
        }
	}

    private IEnumerator ChangeShader(float waitTime)
    {
        float buff = .1f;
        
        if(GetComponent<Renderer>().material.GetFloat("_DissolveThreshold")<1)
        {
            runtime += 1;
            GetComponent<Renderer>().material.SetFloat("_DissolveThreshold", buff * runtime);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(ChangeShader(waitTime));
        }
        else
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
            hit = true;
            StopCoroutine(ChangeShader(waitTime));

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="Bullet")
            StartCoroutine(ChangeShader(.1f));
    }
}
