using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObjInfo : MonoBehaviour {
    public int type;
    public int EnegyCost;
    public int Mat_0Cost;
    public int Mat_1Cost;
    public int Mat_2Cost;

    private FactoryViewer viewer;
    public GameObject factory;

	// Use this for initialization
	void Start () {
        viewer = factory.GetComponent<FactoryViewer>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickObj()
    {
        //給於數值

        //開啟
        viewer.ShowBoard(type);
    }
}
