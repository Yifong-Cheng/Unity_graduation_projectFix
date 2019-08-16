using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryViewer : MonoBehaviour {
    public GameObject FactoryCanvas;
    private GameObject FactoryUI;
    private GameObject Content;
    private GameObject SoilderMat;
    private GameObject Board;
    public List<GameObject> SoilderTypeCreatList = new List<GameObject>();
    private FactoryController factoryController;
    private int CreatType;

    //
    public Text Energy_Text;
    public Text Mat_0_Text;
    public Text Mat_1_Text;
    public Text Mat_2_Text;
    public int CurrentCreat = 1;
    public Text CurrentCreatText;
    private FactoryObjInfo factoryObjInfo;


    //
    private OLDPlayerController m_playerController;
    private GameObject m_player;
    public string PlayerNameString = "Player";

    // Use this for initialization
    void Start () {
        //FactoryCanvas = GameObject.Find("FactoryCanvas");
        FactoryCanvas = transform.GetChild(1).gameObject;
        GetObjs();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetObjs()
    {
        
        factoryController = GetComponent<FactoryController>();
        FactoryUI = FactoryCanvas.transform.GetChild(0).gameObject;
        FactoryUI.SetActive(false);
        Board = FactoryCanvas.transform.GetChild(0).transform.GetChild(2).gameObject;
        Content = FactoryCanvas.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        for(int i=0;i<Content.transform.childCount;i++)
        {
            SoilderTypeCreatList.Add(Content.transform.GetChild(i).gameObject);
        }
        m_player = GameObject.Find(PlayerNameString).gameObject;
        m_playerController = m_player.GetComponent<OLDPlayerController>();

    }

    public void ShowFactoryUi()
    {

        FactoryUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowBoard(int TypeNum)
    {
        CreatType = TypeNum;
        factoryObjInfo = Content.transform.GetChild(CreatType).GetComponent<FactoryObjInfo>();
        Energy_Text.text = factoryObjInfo.EnegyCost.ToString();
        Mat_0_Text.text = factoryObjInfo.Mat_0Cost.ToString();
        Mat_1_Text.text = factoryObjInfo.Mat_1Cost.ToString();
        Mat_2_Text.text = factoryObjInfo.Mat_2Cost.ToString();
        CurrentCreat = 0;
        CurrentCreatText.text = CurrentCreat.ToString();

        Board.SetActive(true);
    }

    public void AddCurrent()
    {
        factoryObjInfo = Content.transform.GetChild(CreatType).GetComponent<FactoryObjInfo>();
        if(factoryObjInfo.type==0)
        {
            if ((factoryObjInfo.EnegyCost * CurrentCreat) < (m_playerController.m_bgInfo.Instance.Enegy))
            {
                CurrentCreat += 1;
            }
        }
        else
        {
            if ((factoryObjInfo.EnegyCost * CurrentCreat) < (m_playerController.m_bgInfo.Instance.Enegy)
                && (factoryObjInfo.Mat_0Cost * CurrentCreat) < (m_playerController.m_bgInfo.Instance.Mat0)
                && (factoryObjInfo.Mat_1Cost * CurrentCreat) < (m_playerController.m_bgInfo.Instance.Mat1)
                && (factoryObjInfo.Mat_2Cost * CurrentCreat) < (m_playerController.m_bgInfo.Instance.Mat2)
           )
            {
                CurrentCreat += 1;
            }
            else
            {
                CurrentCreat = CurrentCreat;
            }
        }

        

        
        CurrentCreatText.text = CurrentCreat.ToString();
        Energy_Text.text = (factoryObjInfo.EnegyCost* CurrentCreat).ToString();
        Mat_0_Text.text = (factoryObjInfo.Mat_0Cost* CurrentCreat).ToString();
        Mat_1_Text.text = (factoryObjInfo.Mat_1Cost* CurrentCreat).ToString();
        Mat_2_Text.text = (factoryObjInfo.Mat_2Cost* CurrentCreat).ToString();

    }
    public void DesCurrent()
    {
        if(CurrentCreat>0)
        {
            CurrentCreat-=1;
           
        }
        else
        {
            CurrentCreat= 0;
        }
        CurrentCreatText.text = CurrentCreat.ToString();

        factoryObjInfo = Content.transform.GetChild(CreatType).GetComponent<FactoryObjInfo>();
        Energy_Text.text = (factoryObjInfo.EnegyCost * CurrentCreat).ToString();
        Mat_0_Text.text = (factoryObjInfo.Mat_0Cost * CurrentCreat).ToString();
        Mat_1_Text.text = (factoryObjInfo.Mat_1Cost * CurrentCreat).ToString();
        Mat_2_Text.text = (factoryObjInfo.Mat_2Cost * CurrentCreat).ToString();

    }

    public void CloseFactoryUi()
    {
        Board.SetActive(false);
        FactoryUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void CloseBoard()
    {
        Board.SetActive(false);
    }

    public void ConfirmCreat()
    {
        //send to controller and creat
        factoryController.StartCreatSoilders(CreatType, 
                                             CurrentCreat, 
                                             (factoryObjInfo.EnegyCost* CurrentCreat), 
                                             (factoryObjInfo.Mat_0Cost* CurrentCreat), 
                                             (factoryObjInfo.Mat_1Cost* CurrentCreat), 
                                             (factoryObjInfo.Mat_2Cost* CurrentCreat)
                                             );
        //close
        Board.SetActive(false);

    }
}
