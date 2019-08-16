using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 17 產生海鷗群的指令搞
public class PlaceObject : MonoBehaviour {

    public GameObject objectsToPlace;
    public int count;
    //海鷗的初始位置在一個半徑為radius的球體內隨機產生
    public float radius;
    public bool IsPlanar;
    private void Awake()
    {
        Vector3 position = new Vector3(0, 0, 0);
        for(int i=0; i < count ; i++)
        {
            position = transform.position + Random.insideUnitSphere * radius;
            if (IsPlanar)
                position.y = objectsToPlace.transform.position.y;
            //產生海鷗實體
            Instantiate(objectsToPlace, position, Quaternion.identity,this.gameObject.transform);
        }
    }
}
