using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 12 測試碰撞
public class ColliderColorChange : MonoBehaviour
{
	void Start () {
		
	}
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //如果與其他物件碰撞，碰撞體變成紅色
        print("collider !");
        if(other.gameObject.GetComponent<Vehicle>()!=null)
        {
            print("collider !");
            Renderer render = this.gameObject.GetComponent<Renderer>();
            Material mat = render.sharedMaterial;
            mat.color = Color.red;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //碰撞體變成灰色
        Renderer render = this.gameObject.GetComponent<Renderer>();
        Material mat = render.sharedMaterial;
        mat.color = Color.gray;
    }
}
