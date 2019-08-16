using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeControl1 : MonoBehaviour {
    public bool GetSeed;
    private bool GrowSuccess;
    private GameObject seed;

    public string ShowState;
    enum TreeState
    {
        Null = -1,
        Seed = 0,
        Grow = 1,
        GrowSuccess = 2,
        Birth = 3,
        BirthSuccess = 4,
    }
    TreeState treeState;

	// Use this for initialization
	void Start () {
        treeState = TreeState.Null;
    }
	
	// Update is called once per frame
	void Update () {
        CheckState();
	}

    void CheckState()
    {
        ShowState = treeState.ToString();
        switch (treeState)
        {
            case TreeState.Null:
                {
                    treeState = TreeState.Seed;
                }
                break;

            case TreeState.Seed:
                {
                    
                }
                break;

            case TreeState.Grow:
                {
                    //if (GetSeed && !GrowSuccess)
                    //{
                    //    Grow();
                    //}
                    //else
                    //{
                    //    treeState = TreeState.GrowSuccess;
                    //}
                    Grow();
                }
                break;

            case TreeState.GrowSuccess:
                {
                    //change something

                }
                break;

            case TreeState.Birth:
                {
                    //start creat and run time

                }
                break;

            case TreeState.BirthSuccess:
                {
                    //creat success and return birth

                }
                break;

            default:
                break;
        }
    }
    void Grow()
    {
        if (transform.localScale.y < 5)
        {
            transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime;
        }
        else
        {
            //GrowSuccess = true;
            treeState = TreeState.GrowSuccess;

        }
    }
    void GetSeedTrigger()
    {
        //OnTriggerEnter(seed.GetComponent<Collider>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SeedControl>()!=null && treeState == TreeState.Seed)
        {
            Destroy(other.gameObject, 5f);
            treeState = TreeState.Grow;
            //seed = other.gameObject;
            //GetSeed = true;
        }
        
    }
}
