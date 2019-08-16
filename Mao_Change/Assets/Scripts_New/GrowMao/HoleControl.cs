using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleControl : MonoBehaviour
{
    public TreeManger treeManger;
    public LoadAndSave data;
    public bool IsOn;
    public string Type;
    public MaoTreeControl tree;

    public bool NearPlayer;
    public GameObject Player;
    public GameObject holeTutorial;
    // Use this for initialization
    void Start () {
        treeManger = GameObject.FindObjectOfType<TreeManger>();
    }
	
	// Update is called once per frame
	void Update () {
		//if(NearPlayer)
  //      {
  //          Player.GetComponent<NewPlayerController>().NearHole = true;
  //      }
  //      else
  //      {
  //          Player.GetComponent<NewPlayerController>().NearHole = false;
  //      }
	}
    private IEnumerator StartChange(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        CreatMaoTree();
    }
    private void CreatMaoTree()
    {
        StageMaoTreeInfo treeInfo = new StageMaoTreeInfo();
        treeInfo.WaitTime = 20;
        treeInfo.State = 0;
        tree = treeManger.CreatMaoTree(this.transform.position, Type,treeInfo).GetComponent<MaoTreeControl>();//-------
        tree.Player = Player;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Seed" && !IsOn && teach.Instance.PutFirstTime == false)
        //{
        //    IsOn = true;
        //    teach.Instance.PutFirstTime = true;
        //    teach.Instance.TeachLevel = 51;
        //    switch (other.GetComponent<ObjInfo>().FollowerTypeID)
        //    {
        //        case 0:
        //            Type = "S0";
        //            break;
        //        case 1:
        //            Type = "S1";
        //            break;

        //        case 2:
        //            Type = "S2";
        //            break;

        //        case 3:
        //            Type = "S3";
        //            break;
        //        case 4:
        //            Type = "S4";
        //            break;

        //    }
        //    Destroy(other.gameObject, 1f);
        //    //StartCoroutine(StartChange(.3f));
        //    CreatMaoTree();
        //}

        //else 
        if (other.GetComponent<ObjInfo>() != null && other.GetComponent<ObjInfo>().Type == "Seed" && !IsOn)
        {
            IsOn = true;
            switch(other.GetComponent<ObjInfo>().FollowerTypeID)
            {
                case 0:
                    Type = "S0";
                    break;
                case 1:
                    Type = "S1";
                    break;

                case 2:
                    Type = "S2";
                    break;

                case 3:
                    Type = "S3";
                    break;
                case 4:
                    Type = "S4";
                    break;

            }
            Destroy(other.gameObject, 1f);
            //StartCoroutine(StartChange(.3f));
            CreatMaoTree();
        }
    }
}
