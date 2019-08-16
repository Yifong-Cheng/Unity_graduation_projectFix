using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Playables;

public class MyTutorial : MonoBehaviour
{
    private static MyTutorial _instance;
    public static MyTutorial Instance
    {
        get { return _instance; }
    }

    public bool PickS0, PickS1, PickS2, PickS3, PickS4;
    public bool illsNS, illsCB, illsBM;
    public bool firstGame;

    public GameObject TutorialCanvas;
    public Button btn;
    [Header("告訴玩家教學探索還有多少")]
    public GameObject ShowTutorialProcess;
    private Text currentProcess, totalProcess;
    private LoadAndSave data;
    private bool IsPass;
    private int RunTime;
    public int boolCount;
    private int totalBool = 6;
    private bool EasyToturialEnd;
    private CameraFollow cameraFollow;

    private bool FistPickUP = false;

    [Header("SyringeTimeLine")]
    public GameObject TimeLineSyringe;
    public PlayableDirector playableDirector;

    private MyTutorial[] myTutorials;
    private void Awake()
    {
        data = GameObject.Find("Stage" + 1).GetComponent<LoadAndSave>();
        IsPass = data.m_datainfolist.stageData.Pass;
        RunTime = data.m_datainfolist.stageData.RunTime;
        //_instance = this;
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            myTutorials = GameObject.FindObjectsOfType<MyTutorial>();
            for (int i = 0; i < myTutorials.Length; i++)
            {
                if (myTutorials[i] != _instance)
                {
                    Destroy(myTutorials[i].gameObject);
                }
                else
                {
                    DontDestroyOnLoad(_instance);
                }
            }
        }
        //cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
        ShowTutorialProcess = GameObject.Instantiate(Resources.Load<GameObject>("Tutorial/" + "ShowTutorialProcess"));
        currentProcess = ShowTutorialProcess.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>();
        currentProcess.text = "1";
        totalProcess = ShowTutorialProcess.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>();
        totalProcess.text = " / " + totalBool.ToString();
        ShowTutorialProcess.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        if (!IsPass)
        {
            //ShowControl();
            boolCount++;
        }
        else
        {

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !EasyToturialEnd)
        {
            EasyToturialEnd = true;
            Destroy(ShowTutorialProcess);

            //0428
            StartCoroutine(PlaySyringeTimeLine());
        }
    }

    public void Close(Button btn)
    {

        //if (teach.Instance.TeachLevel == 41)
        //{
        //    teach.Instance.TeachLevel++;

        //    boolCount++;
        //    if (boolCount >= totalBool && !EasyToturialEnd)
        //    {
        //        EasyToturialEnd = true;
        //        StageChallange stageChallange = GameObject.FindObjectOfType<StageChallange>();
        //        stageChallange.Startevent = true;
        //        //stageChallange.EasyChallange();
        //    }
        //    if (ShowTutorialProcess != null)
        //    {
        //        currentProcess.text = boolCount.ToString();
        //        ShowTutorialProcess.SetActive(true);
        //    }
        //    Time.timeScale = 1;
        //    btn = null;
        //    cameraFollow.LockMouse();
        //    Destroy(TutorialCanvas);
        //}
        //else if (teach.Instance.TeachLevel == 58)
        //{
        //    teach.Instance.TeachLevel++;

        //    boolCount++;
        //    if (boolCount >= totalBool && !EasyToturialEnd)
        //    {
        //        EasyToturialEnd = true;
        //        StageChallange stageChallange = GameObject.FindObjectOfType<StageChallange>();
        //        stageChallange.Startevent = true;
        //        //stageChallange.EasyChallange();
        //    }
        //    if (ShowTutorialProcess != null)
        //    {
        //        currentProcess.text = boolCount.ToString();
        //        ShowTutorialProcess.SetActive(true);
        //    }
        //    Time.timeScale = 1;
        //    btn = null;
        //    cameraFollow.LockMouse();
        //    Destroy(TutorialCanvas);
        //}
        //else
        //{
        //    boolCount++;
        //    if (boolCount >= totalBool && !EasyToturialEnd)
        //    {
        //        EasyToturialEnd = true;
        //        Destroy(ShowTutorialProcess);

        //        //0428
        //        StartCoroutine(PlaySyringeTimeLine());

        //    }
        //    if(ShowTutorialProcess!=null)
        //    {
        //        currentProcess.text = boolCount.ToString();
        //        ShowTutorialProcess.SetActive(true);
        //    }

        //    Time.timeScale = 1;
        //    btn = null;
        //    cameraFollow.LockMouse();
        //    Destroy(TutorialCanvas);
        //}

        boolCount++;
        if (boolCount >= totalBool && !EasyToturialEnd)
        {
            EasyToturialEnd = true;
            Destroy(ShowTutorialProcess);

            //0428
            StartCoroutine(PlaySyringeTimeLine());

        }
        if (ShowTutorialProcess != null)
        {
            currentProcess.text = boolCount.ToString();
            ShowTutorialProcess.SetActive(true);
        }

        Time.timeScale = 1;
        btn = null;
        cameraFollow.LockMouse();
        Destroy(TutorialCanvas);
    }

    private IEnumerator PlaySyringeTimeLine()
    {
        if(TimeLineSyringe!=null&& playableDirector!=null)
        {
            TimeLineSyringe.SetActive(true);
            playableDirector.Play();
        }
        yield return null;
        //stageChallange.EasyChallange();
        //if (teach.Instance.FirstMonster == false)
        //{
        //    teach.Instance.SkipTeach = false;
        //    teach.Instance.TeachLevel = 153;
        //}
    }

    public void ShowControl()
    {
        GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
        TutorialCanvas = GameObject.Instantiate(go);
        TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "CONTROL_F");
        firstGame = true;
        btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => Close(btn));
        Time.timeScale = 0;
        cameraFollow.UnlockMouse();
    }

    public void FirstPickS0()
    {
        if (!PickS0)
        {
            if (!FistPickUP)
            {
                //if (teach.Instance.SkipTeach == false)
                //{
                //    teach.Instance.TeachLevel = 41;
                //}
                FistPickUP = true;
            }
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "S0");
            PickS0 = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }

    public void FirstPickS1()
    {
        if (!PickS1)
        {
            if (!FistPickUP)
            {
                //if (teach.Instance.SkipTeach == false)
                //{
                //    teach.Instance.TeachLevel = 41;
                //}
                FistPickUP = true;
            }
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "S1");
            PickS1 = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }

    public void FirstPickS2()
    {
        if (!PickS2)
        {
            if (!FistPickUP)
            {
                //if (teach.Instance.SkipTeach == false)
                //{
                //    teach.Instance.TeachLevel = 41;
                //}
                FistPickUP = true;
            }
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "S2");
            PickS2 = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }

    public void FirstPickS3()
    {
        if (!PickS3)
        {
            if (!FistPickUP)
            {
                //if (teach.Instance.SkipTeach == false)
                //{
                //    teach.Instance.TeachLevel = 41;
                //}
                FistPickUP = true;
            }
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "S3");
            PickS3 = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }

    public void FirstPickS4()
    {
        if (!PickS4)
        {
            if (!FistPickUP)
            {
                //if (teach.Instance.SkipTeach == false)
                //{
                //    teach.Instance.TeachLevel = 41;
                //}
                FistPickUP = true;
            }
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "S4");
            PickS4 = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }

    public void FirstillsNS()
    {
        if (!illsNS)
        {
            //if (teach.Instance.SkipTeach == false)
            //{
            //    teach.Instance.TeachLevel = 58;
            //}
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "NS");
            illsNS = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }
    public void FirstillsCB()
    {
        if (!illsCB)
        {
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "CB");
            illsCB = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }
    public void FirstillsBEEM()
    {
        if (!illsBM)
        {
            cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            GameObject go = Resources.Load<GameObject>("Tutorial/" + "TutorialCanvas");
            TutorialCanvas = GameObject.Instantiate(go);
            TutorialCanvas.transform.Find("ShowImg").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Tutorial/" + "BEEM");
            illsBM = true;
            btn = TutorialCanvas.transform.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => Close(btn));
            Time.timeScale = 0;
            cameraFollow.UnlockMouse();
        }

    }
}
