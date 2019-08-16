using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour {
    private int TotalCreat;
    private int SoilderType;
    private OLDPlayerController m_controller;
    private GameObject m_player;
    public string PlayerName = "Player";

    private int Enegy;
    private int Mat0;
    private int Mat1;
    private int Mat2;

    private string LittleMaoName = "LittleMao";
    private string TestMaoName = "TestMao";
    private string BattleMaoName = "BattleMao";
    private string FlyMaoName = "FlyMao";

    //public List<GameObject> SoilderPrefabs = new List<GameObject>();
    public List<Transform> CreatPoint = new List<Transform>();

    // Use this for initialization
    void Start () {
        m_player = GameObject.Find(PlayerName).gameObject;
        m_controller = m_player.GetComponent<OLDPlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartCreatSoilders(int Type,int totalNum,int totalEnergy,int totalMat0,int totalMat1,int totalMat2)
    {
        TotalCreat = totalNum;
        SoilderType = Type;
        Enegy = totalEnergy;
        Mat0 = totalMat0;
        Mat1 = totalMat1;
        Mat2 = totalMat2;
        RunCreat();

    }

    private void RunCreat()
    {
        //
        m_controller.m_bgInfo.Instance.Enegy -= Enegy;
        m_controller.m_bgInfo.Instance.Mat0 -= Mat0;
        m_controller.m_bgInfo.Instance.Mat1 -= Mat1;
        m_controller.m_bgInfo.Instance.Mat2 -= Mat2;
        Debug.Log("SuccessCreat Type_"+ SoilderType +"_Soilder the energy is _ " + m_controller.m_bgInfo.Instance.Enegy);
        //GameObject SoilderPfb;
        //
        switch (SoilderType)
        {
            case 0:
                CreatSoilder(LittleMaoName);
                break;

            case 1:
                CreatSoilder(BattleMaoName);
                break;

            case 2:
                CreatSoilder(FlyMaoName);
                break;

            case 3:

                break;

            default:

                break;
        }
        //Instantiate(SoilderPrefabs[SoilderType]);
    }

    private void CreatSoilder(string Name)
    {
        GameObject SoilderPfb;
        for (int i = 0; i < TotalCreat; i++)
        {
            if(i== 0)
            {
                SoilderPfb = Resources.Load(Name) as GameObject;
                Debug.LogWarning("AIStatecontroller class Is Change");
                //SoilderPfb.GetComponent<AIStateController>().AddCompoments();
                SoilderPfb.transform.position = CreatPoint[i].transform.position;
                SoilderPfb.transform.rotation = Quaternion.identity;
                Instantiate(SoilderPfb);
            }
            else
            {
                SoilderPfb = Resources.Load(Name) as GameObject;
                Debug.LogWarning("AIStatecontroller class Is Change");
                //SoilderPfb.GetComponent<AIStateController>().AddCompoments();
                SoilderPfb.transform.position = CreatPoint[i% CreatPoint.Count].transform.position;
                SoilderPfb.transform.rotation = Quaternion.identity;
                Instantiate(SoilderPfb);
            }
            
        }
    }
}
