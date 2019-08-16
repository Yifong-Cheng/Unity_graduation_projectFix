using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {
    public Image img;
    //private SpriteRenderer sprite;
    [SerializeField]
    int alpha = 255;
	// Use this for initialization
	void Start () {
        //sprite = img.GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator Fade()
    {
       
        while(img.color.a>0)
        {
            alpha-=10;
            //img.color = new Color(img.color.r,img.color.g,img.color.b,alpha);
            img.color = new Color(0,0,0,alpha);
            yield return null;
        }
        
    }
}
