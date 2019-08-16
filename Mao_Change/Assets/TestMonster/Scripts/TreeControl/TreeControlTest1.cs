using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControlTest1 : MonoBehaviour {
    protected GameObject TheTree;
    protected List<Transform> Props = new List<Transform>();

    [SerializeField]
    protected bool IsDrop;
    protected float CurrentDroptime = 0f;
    protected float DropTime = 3f;
    [SerializeField]
    protected bool IsGrounded;
    protected float CurrentReturnTime = 0f;
    protected float ReturnTime = 5f;

    protected Vector3 originalPos;

    public GameObject Floor;
	// Use this for initialization
	void Start () {
        originalPos = this.transform.position;
        TheTree = this.transform.parent.gameObject;
        //for(int i=1;i<TheTree.transform.GetChildCount();i++)
        //{
        //    Props.Add(TheTree.transform.GetChild(i));
        //}
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        if(!IsDrop)
        {
            CurrentDroptime += Time.deltaTime;
            if(CurrentDroptime>DropTime)
            {
                CurrentDroptime = 0f;
                IsDrop = true;
            }
        }else if(IsDrop&&!IsGrounded)
        {
            this.transform.position -= new Vector3(0, .01f, 0);
        }

        if(Vector3.Distance(new Vector3(0, this.transform.position.y,0), new Vector3(0, Floor.transform.position.y,0))<0.3f)
        {
            IsGrounded = true;
            
        }
        if(IsGrounded == true)
        {
            CurrentReturnTime += Time.deltaTime;
            if (CurrentReturnTime > ReturnTime)
            {
                CurrentReturnTime = 0;
                this.transform.position = originalPos;
                IsGrounded = false;
                IsDrop = false;
                
            }
        }
	}

}
