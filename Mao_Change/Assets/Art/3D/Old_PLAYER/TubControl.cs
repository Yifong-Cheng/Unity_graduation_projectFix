using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubControl : MonoBehaviour {
    public GameObject m_RHand;
    [SerializeField]
    public GameObject sobj;
    public GameObject Player;
    //private GameObject RightHand;
	// Use this for initialization
	void Start () {
        this.transform.parent = m_RHand.transform;
        this.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = m_RHand.transform.rotation;
        
	}

    public void ShootObj(float waittime)
    {
        Invoke("Shoot", waittime);
    }

    private void Shoot()
    {
        GameObject go = Resources.Load<GameObject>("Shoot");

        sobj = Instantiate(go, this.transform.position, Player.transform.rotation);

        sobj.GetComponent<Rigidbody>().AddForce(Player.transform.forward * 20, ForceMode.Impulse);
    }
}
