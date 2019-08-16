using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour {

    private int CreatType;
    public bool IsWorking;
    private int CreatNums;

    public bool TakeBreak = false;

    private string LittleMaoName = "LittleMao";
    private string TestMaoName = "TestMao";
    private string BattleMaoName = "BattleMao";
    private string FlyMaoName = "FlyMao";

    private float currentTime;
    private float BreakTime = 500;

    private float CreatTime = 1;
    private float currentCreatTime;

    public List<Transform> CreatPoint = new List<Transform>();

    private Animator m_anim;

    // Use this for initialization
    void Start () {
        m_anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        m_anim.SetBool("IsWorking", IsWorking);

        if (TakeBreak==false)
        {
            
            if(IsWorking)
            {
                AutoRandomCreat(CreatType);
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if(currentTime>BreakTime)
            {
                
                TakeBreak = false;
                currentTime = 0;
            }
        }
	}

    private void AutoRandomCreat(int Type)
    {
        CreatNums = Random.Range(1, 5);
        Debug.Log(CreatNums);

        switch(Type)
        {
            case 0:
                CreatSoilder(LittleMaoName, CreatNums);
                break;

            case 1:
                CreatSoilder(BattleMaoName, CreatNums);
                break;

            case 2:
                CreatSoilder(FlyMaoName, CreatNums);
                break;

            case 3:

                break;

            default:

                break;
        }
        

    }

    private void CreatSoilder(string Name,int Nums)
    {
        Debug.LogWarning("Script_HoleController CreatSoilder Must Be ReWriteCodeing");
        //GameObject SoilderPfb;
        //for (int i = 0; i < Nums; i++)
        //{
        //    if (i == 0)
        //    {
        //        SoilderPfb = Resources.Load(Name) as GameObject;
        //        SoilderPfb.GetComponent<AIStateController>().AddCompoments();
        //        SoilderPfb.transform.position = CreatPoint[i].transform.position;
        //        SoilderPfb.transform.rotation = Quaternion.identity;
        //        Instantiate(SoilderPfb);
        //    }
        //    else
        //    {
        //        SoilderPfb = Resources.Load(Name) as GameObject;
        //        SoilderPfb.GetComponent<AIStateController>().AddCompoments();
        //        SoilderPfb.transform.position = CreatPoint[i % CreatPoint.Count].transform.position;
        //        SoilderPfb.transform.rotation = Quaternion.identity;
        //        Instantiate(SoilderPfb);
        //    }
  
        //}
        //TakeBreak = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ObjInfo>()!=null)
        {
            if(other.GetComponent<ObjInfo>().Type == "Soilder")
            {
                if(other.GetComponent<ObjInfo>().State=="AutoCreat")
                {
                    CreatType = other.GetComponent<ObjInfo>().FollowerTypeID;
                    IsWorking = true;
                }
                
            }
        }
    }
}
