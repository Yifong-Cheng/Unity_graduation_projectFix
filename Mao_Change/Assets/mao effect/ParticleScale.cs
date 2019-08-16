using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScale : MonoBehaviour {
    public GameObject scale1;
    public bool X;
    public bool Y;
    public bool Z;
    private Vector3 scaleRange;
    public int ScaleSpeed = 3;
	// Use this for initialization
	void Start () {
        scaleRange = new Vector3(X ? 1 : 0, Y ? 1 : 0, Z ? 1 : 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += scaleRange * Time.deltaTime*ScaleSpeed;
        if(transform.localScale.x>5)
        {
            if(scale1!=null)
            {
                scale1.SetActive(true);
            }
            
        }
	}
}
