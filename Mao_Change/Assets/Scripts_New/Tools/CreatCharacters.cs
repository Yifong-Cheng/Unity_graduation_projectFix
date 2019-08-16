using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatCharacters : MonoBehaviour {
    public GameObject Obj;
    [Range (0.0f,10.0f)]
    public float maxRange, minRange;
    [Range(0, 5)]
    public int num,maxnums;

    [Range(0, 2)]
    public int height;

    public bool IsRandom;
	// Use this for initialization
	void Start () {
        if(IsRandom)
        {
            int n = Random.Range(num, maxnums);
            Creat(minRange, maxRange, n);
        }
        else
        {
            Creat(minRange, maxRange,num);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Creat(float min,float max,int nums)
    {
        for(int i=0;i<nums;i++)
        {

            Vector3 pos = 
                new Vector3(
                this.transform.position.x + Random.Range(min, max), 
                this.transform.position.y+ height,
                this.transform.position.z + Random.Range(min, max)
                );
            Instantiate(Obj, pos, Quaternion.identity, this.transform);
        }
    }
}
