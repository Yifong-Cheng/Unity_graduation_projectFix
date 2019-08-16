using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    public GameObject TypeCircle;
    float angel = 0;
    int theRealNum;

    List<GameObject> circles = new List<GameObject>();
    int Choose = -1;

    List<GameObject> types = new List<GameObject>();
    int ChooseType = -1;

    public Text L_InfoText;
    public Text R_InfoText;

    public Transform R_bgInfo;
    public Transform L_bgInfo;

    public GameObject MainCircle;

    private float stayTime = 0f;
    private float LimitTime = .4f;
    private int State;

    private PlayerControlConform m_playerInfoControl;

    private void Awake()
    {
        m_playerInfoControl = GameObject.Find("Player").GetComponent<PlayerControlConform>();
        CreatLeftCircles();
        CreatMainCircle();
    }

    // Use this for initialization
    void Start()
    {
        
        //CreatTestProps();   
    }

    // Update is called once per frame
    void Update()
    {
        ChangeInfo();
    }

    #region ChangeInfo
    void ChangeInfo()
    {
        TotalControl();

    }

    //空罐的位置
    void TotalControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Choose < (circles.Count - 1))
            {
                Choose++;
            }
            else
            {
                if (Choose == circles.Count - 1)
                {
                    Choose = 0;
                }
            }
            int realNum = ((Choose + 3) < 20 ? (Choose + 3) : (Choose + 3 - 20));
            for (int i = 0; i < circles.Count; i++)
            {
                if (i == realNum)
                {
                    circles[realNum].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
                else
                {
                    circles[i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
            //LeftRotate();
            L_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
            L_InfoText.text = realNum.ToString();
            theRealNum = realNum;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Choose > 0)
            {
                Choose--;
            }
            else
            {
                if (Choose == 0)
                {
                    Choose = circles.Count;
                }
            }
            int realNum = ((Choose + 3) < 20 ? (Choose + 3) : (Choose + 3 - 20));
            for (int i = 0; i < circles.Count; i++)
            {
                if (i == realNum)
                {
                    circles[realNum].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }
                else
                {
                    circles[i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
            //RightRotate();
            L_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
            L_InfoText.text = realNum.ToString();
            theRealNum = realNum;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            R_InfoText.text = theRealNum.ToString();
            //if (circles[theRealNum].gameObject.transform.GetChild(0).childCount != null)
            //{
            //    GameObject changeObj_0 = circles[theRealNum].gameObject.transform.GetChild(0).GetChild(0).gameObject;
            //    changeObj_0.transform.parent = MainCircle.transform.GetChild(0);
            //    changeObj_0.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            //    GameObject changeObj_1 = MainCircle.transform.GetChild(0).GetChild(0).gameObject;
            //    changeObj_1.transform.parent = circles[theRealNum].gameObject.transform.GetChild(0);
            //    changeObj_1.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            //}
            //else if (circles[theRealNum].gameObject.transform.GetChild(0).childCount == null)
            //{
            //    GameObject changeObj_0 = circles[theRealNum].gameObject.transform.GetChild(0).GetChild(0).gameObject;
            //    changeObj_0.transform.parent = MainCircle.transform.GetChild(0);
            //    changeObj_0.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            //}
            //else if(MainCircle.transform.GetChild(0).childCount==null)
            //{
            //    GameObject changeObj_0 = circles[theRealNum].gameObject.transform.GetChild(0).GetChild(0).gameObject;
            //    changeObj_0.transform.parent = MainCircle.transform.GetChild(0);
            //    changeObj_0.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            //}

            //------------
            if (MainCircle.transform.GetChild(0).childCount == 0)
            {
                if (circles[theRealNum].gameObject.transform.GetChild(0).childCount != 0)
                {
                    GameObject changeObj_0 = circles[theRealNum].gameObject.transform.GetChild(0).GetChild(0).gameObject;
                    changeObj_0.transform.parent = MainCircle.transform.GetChild(0);
                    changeObj_0.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    changeObj_0.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);

                }
                else if (circles[theRealNum].gameObject.transform.GetChild(0).childCount == 0)
                {
                    
                }
            }
            else
            {
                if (circles[theRealNum].gameObject.transform.GetChild(0).childCount != 0)
                {
                    GameObject changeObj_0 = circles[theRealNum].gameObject.transform.GetChild(0).GetChild(0).gameObject;
                    changeObj_0.transform.parent = MainCircle.transform.GetChild(0);
                    changeObj_0.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    changeObj_0.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);

                    GameObject changeObj_1 = MainCircle.transform.GetChild(0).GetChild(0).gameObject;
                    changeObj_1.transform.parent = circles[theRealNum].gameObject.transform.GetChild(0);
                    changeObj_1.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    changeObj_1.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);
                }
                else if (circles[theRealNum].gameObject.transform.GetChild(0).childCount == 0)
                {
                    GameObject changeObj_1 = MainCircle.transform.GetChild(0).GetChild(0).gameObject;
                    changeObj_1.transform.parent = circles[theRealNum].gameObject.transform.GetChild(0);
                    changeObj_1.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    changeObj_1.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);

                }
            }

        }
        else if (Input.GetMouseButton(0))
        {
            //switch(MainCircle.transform.GetChild(0).GetChild(0).GetComponent<Text>().text)
            //{
            //    case "0":

            //        break;

            //    default:

            //        break;

            //}
            //if(MainCircle.transform.GetChild(0).transform.childCount!=null )
            //{
            //    Debug.Log(MainCircle.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
            //}
            //else
            //{
            //    Debug.Log("IsEmpty");
            //}
            stayTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (stayTime > LimitTime)
            {
                State = 1;
                stayTime = 0;
            }
            else
            {
                State = 0;
                stayTime = 0;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            m_playerInfoControl.GetPropInfo();
        }
    }

    //裝完的罐子或道具的位置
    void FullJarUIControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ChooseType < (types.Count - 1))
            {
                ChooseType++;
            }
            else
            {
                if (ChooseType == types.Count - 1)
                {
                    ChooseType = 0;
                }
            }
            int realNum = ((ChooseType + 3) < 20 ? (ChooseType + 3) : (ChooseType + 3 - 20));
            for (int i = 0; i < types.Count; i++)
            {
                if (i == realNum)
                {

                }
                else
                {

                }
            }

            RightRotate();
        }
    }

    #endregion

    #region CreatEmptyTransform
    void CreatLeftCircles()
    {
        float Radius = 50;
        int nums = 20;
        for (int id = 0; id < nums; id++)

        {
            GameObject go = Resources.Load("TypeCircle") as GameObject;

            Vector2 Pos = new Vector2(
                                        Mathf.Sin(360 / nums * id * Mathf.PI / 180) * Radius * nums,
                                        Mathf.Cos(360 / nums * id * Mathf.PI / 180) * Radius * nums);
            Vector2 thisPos = new Vector2(L_bgInfo.GetComponent<RectTransform>().anchoredPosition.x + 960, L_bgInfo.GetComponent<RectTransform>().anchoredPosition.y + 540);
            TypeCircle = Instantiate(go, thisPos + Pos, Quaternion.identity, L_bgInfo);
            TypeCircle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (id - 3) * 360 / 20));
            
            circles.Add(TypeCircle);
        }

        for(int i=0;i<(circles.Count-8);i++)
        {
            SampleProp(i, circles[i].transform.GetChild(0).gameObject);
        }
    }

    void SampleProp(int num , GameObject parent)
    {
        string name = null;
        switch(num%3)
        {
            case (0):
                {
                    name = "Jar";
                }
                break;

            case (1):
                {
                    name = "BlackBall";
                }
                break;

            case (2):
                {
                    name = "RedBall";
                }
                break;

        }
        GameObject go = Resources.Load("SampleProp")as GameObject;
        go.GetComponent<PropInfoControl>().ImageInfoName = name;
        Instantiate(go, parent.transform.position + Vector3.zero, Quaternion.identity, parent.transform);

    }
    void testLoadSprite()
    {
        GameObject go = Resources.Load("SampleProp") as GameObject;
        go.GetComponent<PropInfoControl>().ImageInfoName = name;
        Instantiate(go, Vector3.zero, Quaternion.identity,L_bgInfo);

    }

    //產生空道具測試
    void CreatTestProps()
    {
        for (int id = 0; id < circles.Count; id++)
        {
            GameObject go = Resources.Load("TestText") as GameObject;
            go.GetComponent<Text>().text = id.ToString();

            TypeCircle = Instantiate(go, L_bgInfo.transform.GetChild(id).GetChild(0));
            TypeCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 50);
        }
        GameObject mt = Resources.Load("TestText") as GameObject;
        mt.GetComponent<Text>().text = "Null";
        TypeCircle = Instantiate(mt, MainCircle.transform.GetChild(0));
        TypeCircle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 50);
        TypeCircle = null;
    }

    void CreatRightCircles()
    {
        float Radius = 50;
        int nums = 20;
        for (int id = 0; id < nums; id++)

        {
            GameObject go = Resources.Load("TypeCircle") as GameObject;

            Vector2 Pos = new Vector2(
                                        Mathf.Sin(360 / nums * id * Mathf.PI / 180) * Radius * nums,
                                        Mathf.Cos(360 / nums * id * Mathf.PI / 180) * Radius * nums);
            Vector2 thisPos = new Vector2(R_bgInfo.GetComponent<RectTransform>().anchoredPosition.x + 960, R_bgInfo.GetComponent<RectTransform>().anchoredPosition.y + 540);
            TypeCircle = Instantiate(go, thisPos + Pos, Quaternion.identity, R_bgInfo);
            TypeCircle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (id - 3) * 360 / 20));
            types.Add(go);
        }
    }

    void CreatMainCircle()
    {
        GameObject go = Resources.Load("MainCircle") as GameObject;
        Vector2 thisPos = new Vector2(R_bgInfo.GetComponent<RectTransform>().anchoredPosition.x + 170, R_bgInfo.GetComponent<RectTransform>().anchoredPosition.y + 1160);
        go.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        go.name = "MainProp";
        MainCircle = Instantiate(go, thisPos, Quaternion.identity, R_bgInfo);
        m_playerInfoControl.MainProp = MainCircle;
    }

    #endregion

    #region 旋轉

    void LeftRotate()
    {
        Quaternion ChangeAngle = new Quaternion(0, 0, Choose * 20, -1);
        L_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
    }

    void RightRotate()
    {
        Quaternion ChangeAngle = new Quaternion(0, 0, ChooseType * 20, -1);
        L_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Choose * 360 / 20));
    }


    #endregion

    #region change

    public void OnPointerClick()
    {

        R_InfoText.text = theRealNum.ToString();
        if (circles[theRealNum].gameObject.transform.GetChild(0).childCount != null)
        {

        }
        else
        {

        }
    }


    #endregion

    #region ShowInfo_OnGUI

    void OnGUI()
    {
        GUI.Box(new Rect(10, 200, 400, 150), "DebugLog");

        GUI.TextField(new Rect(20, 240, 180, 20), string.Format("StayTime: {0}", stayTime));
        GUI.TextField(new Rect(20, 280, 180, 20), string.Format("State: {0}", State));
        GUI.TextField(new Rect(20, 320, 360, 20), string.Format(" IF State = 0 run : {0} State = 1 run :{1}", "使用道具", "把道具丟棄或把地上道具撿起或跟手中交換"));

    }

    #endregion
}
