using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterReturn : MonoBehaviour {
    public GameObject playerAgentBase;
    bool rotate;
    Vehicle aI;
    public bool isArrival;
    SteeringForArrive steeringForArrive;
    // Use this for initialization
    void Start () {
        aI = GetComponent<AILocomotion>();
        steeringForArrive = GetComponent<SteeringForArrive>();
	}
    public void RotateTowards()
    {
        float strength = 1f;
        float str;

        Vector3 direction = (transform.position - playerAgentBase.transform.position).normalized;
        direction.y = 0;
        str = Mathf.Min(strength * Time.deltaTime, 1);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        playerAgentBase.gameObject.transform.rotation = Quaternion.Slerp(playerAgentBase.gameObject.transform.rotation, lookRotation, str);
        float angle = Quaternion.Angle(playerAgentBase.gameObject.transform.rotation, transform.rotation);
        if (angle >= 179f)
        {
            rotate = false;
        }
        
    }
    void Update()
    {
        //if (rotate)
        //{
        //    RotateTowards();
        //}


        if (Distance() < 1 && !isArrival)
        {
            
            isArrival = true;
            steeringForArrive.enabled = false;
            aI.velocity = Vector3.zero;
            //RotateSelf();
            
            //RotateTowards();
        }
        else if(isArrival)
        {
            RotateSelf();
            //RotateSelf(playerAgentBase);
        }
        //RotateSelf(playerAgentBase);
    }

    private void RotateSelf()
    {
        float strength = 1f;
        float str;
        Vector3 direction = (transform.position - playerAgentBase.transform.position).normalized;
        //direction.y = 0;
        str = Mathf.Min(strength * Time.deltaTime, 1);
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        this.transform.rotation = lookRotation;

    }

    private void RotateSelf(GameObject target)
    {
        //Quaternion.Euler(target.transform.position.x,);
        this.transform.rotation = target.transform.rotation;
    }

    float Distance()
    {
        Vector3 thispos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 targetpos = new Vector3(playerAgentBase.transform.position.x, 0, playerAgentBase.transform.position.z);
        return Vector3.Distance(thispos,targetpos);
    }
}
