using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatProjector : MonoBehaviour {
    public GameObject projector;

    Ray ray;
    RaycastHit hit;
    Vector3 pos;

    //public Texture2D splashTexture;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        pos = this.transform.position;
        GameObject go = GameObject.Instantiate(projector, pos + new Vector3(0, 5, 0), Quaternion.identity);
        go.transform.LookAt(pos);
        //ray = new Ray(transform.position, Vector3.forward);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    //pos = hit.transform.position;
        //    DrawPaint script = hit.transform.gameObject.GetComponent<DrawPaint>();
        //    if (null != script)
        //        script.PaintOn(hit.textureCoord, splashTexture);
        //}
        //DrawPaint script = other.GetComponent<DrawPaint>();
        //if (null != script)
        //    script.PaintOn(pos, splashTexture);
        Destroy(go.gameObject, 2f);
        Destroy(this.gameObject);
    }
}
