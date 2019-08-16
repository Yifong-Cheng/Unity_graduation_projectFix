using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoxBackageUIControl : MonoBehaviour {
    private int BackageNums = 6;
    private GameObject Prop;
    public List<GameObject> Props = new List<GameObject>();
    public int Choose = 0;
    public GameObject Sign;
    public GameObject LightSign;

    public GameObject Player;

    public GameObject Message;
    public Text MessageText;

    public int Day = 7;
    public Text DayText;
    private float timecount;
    private float time2 = 0.0f;
    private float time3 = 0.0f;

    public GameObject success;
    public GameObject fail;

    public static BoxBackageUIControl Instance;

    public bool TimeStop = false;
    public bool PassOrNot;
    private int runtime;
    public bool MessageShow = false;

    public MaoGame maogame;
    //Crystal
    
    public float TargetNeed;
    public Image Targetbar;
    public Text Crystal;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        Time.timeScale = 1.0f;
        Instance = this;
        PassOrNot = GameObject.Find("Stage1").GetComponent<LoadAndSave>().m_datainfolist.stageData.Pass;
        runtime = GameObject.Find("Stage1").GetComponent<LoadAndSave>().m_datainfolist.stageData.RunTime;
        //Debug.Log(runtime);
    }

    // Use this for initialization
    void Start()
    {
        //CreatBackageObj(BackageNums);
        //CreatTestProp(BackageNums);
        success.SetActive(false);
        fail.SetActive(false);
        Message.SetActive(false);
        //FailState();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeChoose();
        //if (PassOrNot == true)
        //{
        //    time2 += Time.deltaTime;
        //    time3 += Time.deltaTime;
        //}
        //if (time2 > 0.5f && MessageShow == true)
        //{
        //    Message.SetActive(false);
        //    MessageShow = false;
        //}
        //else if (time2 > 1f)
        //{
        //    Message.SetActive(true);
        //    time2 = 0.0f;
        //    MessageShow = true;
        //}
        //if(time3 > 10f)
        //{
        //    Time.timeScale = 0.0f;
        //    PassOrNot = false;
        //    Message.SetActive(false);
        //    success.SetActive(true);
        //    GameObject.FindObjectOfType<CameraFollow>().ShowCursor();

        //}
        //if (TimeStop == false)
        //{
        //    timecount += Time.deltaTime;
        //}
        //else if(TimeStop == true)
        //{
        //    timecount = timecount;
        //}
        //if (timecount >= 90f)
        //{
        //    timecount = 0f;
        //    Day++;
        //    DayText.text = Day.ToString();
        //}
        //ChangeChoose();
        ////UseProp();
        //if (Day >= 8)
        //{
        //    FailState();
        //}
        Targetbar.fillAmount = Count.Instance.Crystal / TargetNeed;
        Crystal.text = ((int)Count.Instance.Crystal).ToString();
        
        if (PassOrNot&&runtime<1)
        {
            time2 += Time.deltaTime;
            time3 += Time.deltaTime;
        }
        else if(!PassOrNot && runtime<1)
        {
            if (time2 > 0.5f && MessageShow == true)
            {
                Message.SetActive(false);
                MessageShow = false;
            }
            else if (time2 > 1f)
            {
                Message.SetActive(true);
                time2 = 0.0f;
                MessageShow = true;
            }
            if (time3 > 10f)
            {
                Time.timeScale = 0.0f;
                PassOrNot = false;
                Message.SetActive(false);
                //success.SetActive(true);
                //GameObject.FindObjectOfType<CameraFollow>().ShowCursor();

            }
            if (TimeStop == false)
            {
                timecount += Time.deltaTime;
            }
            else if (TimeStop == true)
            {
                timecount = timecount;
            }
            if (timecount >= 180f)
            {
                timecount = 0f;
                Day--;
                
            }
            DayText.text = Day.ToString();
            //UseProp();
            if (Day <= 0)
            {
                FailState();
            }
        }
        
    }

    #region CreatUI

    void CreatBackageObj(int nums)
    {
        for (int i = 0; i < nums; i++)
        {
            GameObject go = Resources.Load("TestProp") as GameObject;
            go.name = "P" + i;
            Vector2 Pos = new Vector2(-525 + (i * 175), 0);
            Vector2 thisPos = new Vector2(this.transform.GetComponent<RectTransform>().position.x, this.transform.GetComponent<RectTransform>().position.y);
            Prop = Instantiate(go, thisPos + Pos, Quaternion.identity, this.transform);
            Props.Add(Prop);
        }

    }
    void CreatTestProp(int nums)
    {
        for (int i = 0; i < nums; i++)
        {
            GameObject go = Resources.Load("TestProp") as GameObject;
            go.name = "P" + i;
            //Vector2 Pos = new Vector2(-525 + (i * 175), 0);
            Vector2 thisPos = new Vector2(Props[i].transform.GetComponent<RectTransform>().position.x, Props[i].transform.GetComponent<RectTransform>().position.y);
            Prop = Instantiate(go, thisPos, Quaternion.identity, this.transform.GetChild(i + 1));
            //Props.Add(Prop);
        }
        Prop = null;

    }

    #endregion

    #region UIControl

    void ChangeChoose()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Choose < (Props.Count - 1))
            {
                Choose++;
            }
            else
            {
                if (Choose == Props.Count - 1)
                {
                    Choose = 0;
                }
            }
            //ChangeBackageUI();
            //Debug.LogWarning(Props[Choose].name + "Has" + Count.Instance.number[Choose].ToString());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Choose > 0)
            {
                Choose--;
            }
            else if (Choose == 0)
            {
                //if (Choose == 0)
                //{
                //    Choose = (Props.Count - 1);
                //}
                Choose = (Props.Count - 1);
            }
            //Debug.Log(Choose);
            //ChangeBackageUI();
            //Debug.LogWarning(Props[Choose].name);
        }
    }

    void ChangeBackageUI()
    {
        if (Sign.active == false || LightSign.active == false)
        {
            Sign.SetActive(true);
            LightSign.SetActive(true);
        }
        Sign.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.x,
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.y - 550
                                                                         );
        LightSign.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.x,
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.y
                                                                         );
    }

    #endregion

    #region UseProp

    private void UseProp()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
            
        //}
        Debug.Log("Click");
        /*if (PropEnough())
        {
            //DoAnim

            //Creat
            Instantiate(RealProp(), Player.GetComponent<NewPlayerController>().PutPos.position + new Vector3(0, 3, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("This IS Empty");
        }*/
    }

    public string PropInfo()
    {
        return Props[Choose].GetComponent<PropInfoControl>().ImageInfoName;
    }

    public bool PropEnough()
    {
        //return Props[Choose].GetComponent<PropInfoControl>().num > 0 ? true : false;
        if(Choose==0)
        {
            return Count.Instance.number[4] > 0 ? true : false;
        }
        else
        {
            return Count.Instance.number[Choose-1] > 0 ? true : false;
        }
        
    }

    GameObject RealProp()
    {
        GameObject go = Resources.Load<GameObject>("TestProps/" + Props[Choose].GetComponent<PropInfoControl>().ImageInfoName);
        return go;
    }

    #endregion

    public void FailState()
    {
        GameObject.FindObjectOfType<CameraFollow>().UnlockMouse();
        Time.timeScale = 0.0f;
        fail.SetActive(true);
    }
    public void SuccessState()
    {
        MessageText.text = "朝下一區邁進!";
        PassOrNot = true;
        Message.SetActive(true);
        MessageShow = true;
        //Time.timeScale = 0.0f;
        //success.SetActive(true);
    }
    public void Retry()
    {


        //Application.LoadLevel(Application.loadedLevel);
        //Application.LoadLevel("MainMenuScene"); 
        MaoGame.Instance.NextStage = 0;
        MaoGame.Instance.TurnStage = true;
    }
}
