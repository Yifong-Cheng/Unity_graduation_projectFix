using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachTrigger : MonoBehaviour {
    public bool istrigger;
    public int wanttoPlay = 0;
    private teach t;

    private void Awake()
    {
        t = GameObject.FindObjectOfType<teach>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !istrigger &&　teach.Instance.Final == false && teach.Instance.SkipTeach == false && teach.Instance.TeachLevel == (wanttoPlay-1))
        {
            istrigger = true;
            t.PlayState(wanttoPlay);


        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
