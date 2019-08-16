using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;

public class teach : MonoBehaviour
{
    public GameObject teachcanvas;
    public PlayableDirector playableDirector;
    [Header("文字")]
    public Text TeachText;
    public Text Target;
    //public RectTransform TeachTextTransfrom;
    public int TeachLevel = 0;
    [Header("對話框點")]
    public GameObject image;
    public float time = 0.0f;
    public float time2 = 0.0f;
    public bool imageActive = true;
    private bool TimeToZero = false;
    private int Spacetimes = 0;
    private bool mousebool = false;
    public GameObject Player;

    public GameObject TeachImage;

    public GameObject ExitTeach;
    public GameObject StartTeach;
    public GameObject teaching;
    public GameObject NotTeach;

    public GameObject TeachTarget;
    public GameObject syringeteachImage;

    public bool SkipTeach = false;

    private CameraFollow camera;

    public int stagenum;
    private bool DoTeach;
    private int runtime;
    public bool PickupFirstTime = false;
    public bool PickupTwiceTime = false;
    public bool PutFirstTime = false;
    public bool Final = false;

    public bool FirstMonster = false;
    public bool FirstFinsh = false;


    public bool Fight = false;
    public bool Create = false;

    public int Item = 1;

    public bool LODTeach = false;

    public static teach Instance;

    public CinemachineFreeLook freeLook;


    //public GameObject SpaceTeachTimeLine;
    //public PlayableDirector m_SpaceTeachTimeLine;
    public GameObject PutTeachTimeLine;
    public PlayableDirector m_PutTeachTimeLine;

    public GameObject guid;
    //private NewPlayerController controller;

    public bool IsOnTeach;

    /*
    private void Awake()
    {
        //PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(PlayerTransform.name);
        //controller = GameObject.FindObjectOfType<NewPlayerController>();
        //
        Instance = this;
        camera = GameObject.FindObjectOfType<CameraFollow>();
        //freeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        playableDirector = this.GetComponent<PlayableDirector>();
        //Player = GameObject.FindGameObjectWithTag("Player");
        teachcanvas = GameObject.Find("Teach");
        TeachTarget = GameObject.Find("TeachTarget");
        syringeteachImage = GameObject.Find("syringeteachImage");
        syringeteachImage.SetActive(false);
        TeachTarget.SetActive(false);
        Target = TeachTarget.GetComponent<Text>();
        DoTeach = GameObject.Find("Stage" + stagenum).GetComponent<LoadAndSave>().m_datainfolist.stageData.Pass;
        runtime = GameObject.Find("Stage1").GetComponent<LoadAndSave>().m_datainfolist.stageData.RunTime;
        //SpaceTeachTimeLine = GameObject.Find("SpaceTeachTimeLine");
        //m_SpaceTeachTimeLine = SpaceTeachTimeLine.transform.GetChild(0).GetComponent<PlayableDirector>();
        //SpaceTeachTimeLine.SetActive(false);
        PutTeachTimeLine = GameObject.Find("PutTeachTimeLine");
        m_PutTeachTimeLine = PutTeachTimeLine.transform.GetChild(0).GetComponent<PlayableDirector>();
        PutTeachTimeLine.SetActive(false);
    }
    */

    void Start()
    {
        //freeLook.m_XAxis.m_InputAxisName = "";
        //freeLook.m_YAxis.m_InputAxisName = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!DoTeach && runtime < 1)
        {
            time += Time.deltaTime;
            time2 += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Item--;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Item++;
            }
            if (Item > 6)
            {
                Item = 1;
            }
            if (Item < -4)
            {
                Item = 1;
            }
            if (teachcanvas.activeInHierarchy == true && LODTeach == false)
            {
                //Player.transform.GetChild(0).GetComponent<Animator>().Play("Teaching", 0, 0);
                Player.GetComponent<Animator>().Play("Teaching", 0, 0);
            }
            if (teaching.activeInHierarchy == true)
            {
                
            }
            if (time > 0.5f && imageActive == true)
            {
                image.SetActive(false);
                imageActive = false;
            }
            else if (time > 1f)
            {
                image.SetActive(true);
                time = 0.0f;
                imageActive = true;
            }
            
            if (Input.GetMouseButtonDown(0) && teachcanvas.activeInHierarchy == true)
            {
                TeachLevel++;
            }
            if (SkipTeach == false)
            {
                each();
                
            }
            if (MyTutorial.Instance.boolCount == 4 && Final == false)
            {
                TeachLevel = 71;
            }
        }

    }
    void each()
    {
        switch (TeachLevel)
        {
            //0430
            case -1:
                {
                    //do nothing
                }
                break;
            case 0:
                TeachText.text = "";
                TeachImage.SetActive(false);
                break;
            case 1:
                //Player.transform.GetChild(0).GetComponent<Animator>().Play("Teaching", 0, 0);
                //TeachText.text = "咳 、 咳 ， 聽 的 到 嗎 ？\n聽 好 了 ， 你 是 我 製 造 的 微 型 機 器 人 「 阿 鬚 」";
                TextChange("<b><color=#FF6F83>點一下滑鼠左鍵看下一頁對話</color></b>");
                ImageSwitch(null);
                break;
            case 2:
                TextChange("咳、咳,聽得到嗎?");
                break;
            case 3:
                TextChange("聽好了,你是我製造的微型機器人「阿鬚」。");
                break;
            case 4:
                TextChange("你的任務是消滅所有的入侵生物,\n復育此處生態,讓「毛」再次遍布全身。");
                break;
            case 5:
                TextChange("先跟你說明一下你的螢幕介面上的資訊。");
                break;
            case 6:
                TextChange("有看到左上角的日曆嗎?");
                break;
            case 7:
                TextChange("那個是剩餘的時間,\n要在期限之內完成毛的復育工作。");
                break;
            case 8:
                TextChange("左下角是你能夠使用的道具,\n排列在最前面的是目前手持的道具。");
                break;
            case 9:
                TextChange("右上角的小地圖能夠防止迷路。\n右下角[?]是操作說明。");
                break;
            case 10:
                
                TextChange("現在我們來教你怎麼看蛞蝓的治療進度吧。");
                break;
            case 11:
                TextChange("<b><color=#FF6F83>按下鍵盤「ESC」進入「選取模式」。</color></b>");

                break;
            case 12:
                teachcanvas.SetActive(false);
                TargetSwitch("按下ESC鍵");
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    LODTeach = true;
                    TeachLevel++;
                }
                break;
            case 13:
                TargetSwitch(null);
                teachcanvas.SetActive(true);
                TextChange("<b><color=#FF6F83>嘗試往下滾動滑鼠滾輪。</color></b>");
                break;
            case 14:
                teachcanvas.SetActive(false);
                TargetSwitch("滾動滑鼠滾輪");
                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
                break;
            case 15:
                TargetSwitch(null);
                teachcanvas.SetActive(true);
                TextChange("蛞蝓目前還光禿禿的呢。");
                break;
            case 16:
                //TextChange("<b><color=#FF6F83>在滾動一次滾輪回去。</color></b>");
                TextChange("那麼我們退出「選取模式」吧。");
                break;
            case 17:
                //TargetSwitch("滾動滑鼠滾輪");
                //teachcanvas.SetActive(false);
                TextChange("<b><color=#FF6F83>按下鍵盤「ESC」退出「選取模式」。</color></b>");
                break;
            case 18:
                //TargetSwitch(null);
                //TextChange("那麼我們退出「選取模式」吧。");
                TargetSwitch("按下ESC鍵以退出選取模式");
                teachcanvas.SetActive(false);
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TeachLevel++;
                    LODTeach = false;
                    TargetSwitch(null);
                }
                break;
            case 19:
                teachcanvas.SetActive(true);
                TextChange("在選取模式下，\n不滾動滑鼠滾輪則可指派小兵。");
                break;
            case 20:
                //TargetSwitch("按下ESC鍵以退出選取模式");
                //teachcanvas.SetActive(false);
                //if (Input.GetKeyUp(KeyCode.Escape))
                //{
                //    TeachLevel++;
                //    LODTeach = false;
                //    TargetSwitch(null);
                //}
                TextChange("可至右下角[?]獲取說明。");
                break;
            case 21:
                teachcanvas.SetActive(true);
                TextChange("接著看看周圍,\n確定你的顯示晶片沒有異常。");
                break;
            case 22:
                TextChange("<b><color=#FF6F83>移動滑鼠來轉動視角。</color></b>");
                ImageSwitch("mouse_teach");
                break;
            case 23:
                TargetSwitch("嘗試移動滑鼠");
                DoTime(2f);
                break;
            case 24:
                ImageSwitch(null);
                TargetSwitch(null);
                TextChange("看來畫面的顯示沒有問題,");
                break;
            case 25:
                TextChange("那你動動看身體,\n看有沒有什麼異常。");
                break;
            case 26:
                TextChange("<b><color=#FF6F83>嘗試以 WASD 移動角色。\n左SHIFT以奔跑。</color></b>");
                ImageSwitch("wasd_teach");
                break;
            case 27:
                TargetSwitch("嘗試移動角色");
                DoTime(2f);
                break;
            case 28:
                ImageSwitch(null);
                TargetSwitch(null);
                TextChange("沒什麼問題呢,\n那事不宜遲,");
                break;
            case 29:
                TextChange("我們馬上來進行復育工作吧!");
                break;
            case 30:
                TextChange("不遠處好像有珠毛,\n讓我們靠近點瞧一瞧。");
                break;
            case 31:
                
                //Player.transform.GetChild(0).GetComponent<Animator>().Play("TimeLine1", 0, 0);
                //Player.GetComponent<Animator>().Play("TimeLine1", 0, 0);
                
                //Player.SetActive(false);
                //SpaceTeachTimeLine.SetActive(true);
                //m_SpaceTeachTimeLine.Play();
                //Debug.LogWarning("is case 31");
                teachcanvas.SetActive(false);

                //0430
                //TeachLevel = -1;

                TeachLevel = 32;
                break;
            case 32:
                //SpaceTeachTimeLine.SetActive(false);
                //Player.SetActive(true);

                teachcanvas.SetActive(true);

                TextChange("<b><color=#FF6F83>移動至「毛珠」附近。</color></b>");
                break;
            case 33:
                TargetSwitch("移動至毛株");
                teachcanvas.SetActive(false);
                break;
            case 34:
                TargetSwitch(null);
                teachcanvas.SetActive(true);
                TextChange("靠近毛株時，可使用打氣筒為毛樹打氣。");

                break;
            case 35:
                TextChange("<b><color=#FF6F83>嘗試以 Q、E 切換至打氣筒。</color></b>");
                ImageSwitch("qe_teach");
                break;
            case 36:
                TargetSwitch("切換至打氣筒");
                teachcanvas.SetActive(false);
                if (Item == 1)
                {
                    teachcanvas.SetActive(true);
                    TeachLevel++;
                }
                break;
            case 37:
                TargetSwitch(null);
                TextChange("<b><color=#FF6F83>持續點擊 SPACE 鍵來打氣。</color></b>");
                ImageSwitch("space_teach");
                break;
            case 38:
                TargetSwitch("靠近毛株以SPACE鍵打氣");
                teachcanvas.SetActive(false);
                break;
            case 39:
                teachcanvas.SetActive(true);
                TextChange("有看到掉落的「毛果子」了嗎?\n用罐子收起來吧,之後會有用處的。");
                ImageSwitch(null);
                break;
            case 40:
                TextChange("<b><color=#FF6F83>用滑鼠左鍵把毛果子裝進罐子內。</color></b>");
                ImageSwitch("mouseright_teach");
                break;
            case 41:
                ImageSwitch(null);
                TargetSwitch(null);
                teachcanvas.SetActive(false);
                break;
            case 42:
                teachcanvas.SetActive(true);
                TextChange("拾起的毛果子可在無毛樹的毛孔上復育。");
                break;
            case 43:
                teachcanvas.SetActive(false);
                //Player.transform.GetChild(0).GetComponent<Animator>().Play("PutTeach", 0, 0);
                //Player.GetComponent<Animator>().Play("PutTeach", 0, 0);
                //Player.SetActive(false);

                PutTeachTimeLine.SetActive(true);
                m_PutTeachTimeLine.Play();

                //DoTime(2f);

                break;
            case 44:
                //PutTeachTimeLine.SetActive(false);
                Player.SetActive(true);
                TextChange("<b><color=#FF6F83>請移動至毛孔附近。</color></b>");
                break;
            case 45:
                TargetSwitch("移動至毛孔");
                teachcanvas.SetActive(false);
                break;
            case 46:
                teachcanvas.SetActive(true);
                TargetSwitch(null);
                TextChange("試著把獲得的毛果子種入毛孔內試試。");
                break;
            case 47:
                TextChange("<b><color=#FF6F83>使用 Q、E 鍵切換至毛果子。</color></b>");
                break;
            case 48:
                teachcanvas.SetActive(false);
                TargetSwitch("使用Q、E鍵切換至毛果子");
                if (Item != 1)
                {
                    teachcanvas.SetActive(true);
                    TeachLevel++;
                }
                break;
            case 49:
                ImageSwitch("mouseleft_teach");
                TextChange("在毛孔附近按下滑鼠右鍵將毛果子種入毛孔。");
                break;
            case 50:
                teachcanvas.SetActive(false);
                TargetSwitch("使用滑鼠右鍵將毛果子種入毛孔。");
                break;
            case 51:
                teachcanvas.SetActive(true);
                ImageSwitch(null);
                TargetSwitch(null);
                TextChange("這樣就成功復育了一珠毛了!");
                break;
            case 52:
                teachcanvas.SetActive(false);

                TextChange("那我們同樣幫他打氣,\n協助他成長吧!");
                break;
            case 53:
                teachcanvas.SetActive(false);
                break;
            case 54:
                if (Fight == false)
                {
                    teachcanvas.SetActive(true);
                    TextChange("接下來我們來清除敵人吧。");
                }
                else if(Fight == true)
                {
                    teachcanvas.SetActive(false);
                    TextChange(null);
                }
                break;
            case 55:
                TextChange("毛孔周遭會有偽裝的敵人");
                break;
            case 56:
                TextChange("<b><color=#FF6F83>切換至打氣筒,\n擊倒敵人吧!</color></b>");
                break;
            case 57:
                teachcanvas.SetActive(false);
                TeachLevel++;
                break;
            case 58:
                /*teachcanvas.SetActive(true);
                TargetSwitch(null);
                TextChange("單槍匹馬有些困難對吧?");*/
                teachcanvas.SetActive(false);
                break;
            case 59:
                if (Player.GetComponent<NewPlayerController>().FirstPutJar == false)
                {
                    teachcanvas.SetActive(true);
                    TargetSwitch(null);
                    TextChange("單槍匹馬有些困難對吧?\n我教你怎麼使用「毛仔」支援戰鬥吧!");
                    Fight = true;
                }
                else
                {
                    teachcanvas.SetActive(false);
                }
                break;
            case 60:
                TextChange("<b><color=#FF6F83>使用 Q、E 鍵切換至「毛果子」。</color></b>");
                break;
            case 61:
                TextChange("<b><color=#FF6F83>按下滑鼠右鍵放下罐子。</color></b>");
                break;
            case 62:
                teachcanvas.SetActive(false);
                TargetSwitch("切換至毛果子，並按下滑鼠右鍵召喚毛仔。");
                break;
            case 63:
                if (Create == false)
                {
                    teachcanvas.SetActive(true);
                    TargetSwitch(null);
                    TextChange("召喚出毛仔了,\n他們會協助你戰鬥的。");
                    Create = true;
                }
                else if(Create == true)
                {
                    teachcanvas.SetActive(false);
                }
                break;
            case 64:
                TextChange("我們試著指揮他們去戰鬥好了。");
                
                break;
            case 65:
                TextChange("<b><color=#FF6F83>首先進入選取模式。</color></b>");
                break;
            /*case 66:
                teachcanvas.SetActive(false);
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TeachLevel++;
                }
                break;*/
            case 66:
                
                TextChange("<b><color=#FF6F83>案住滑鼠左鍵選取範圍框選毛仔。</color></b>");
                break;
            /*case 68:
                teachcanvas.SetActive(false);
                break;*/
            case 67:
                
                TextChange("<b><color=#FF6F83>接著用滑鼠左鍵點擊目標地點。</color></b>");
                break;
            /*case 70:
                teachcanvas.SetActive(false);
                break;*/
            case 68:
                
                TextChange("<b><color=#FF6F83>最後退出「選取模式」</color></b>");
                break;
            /*case 72:
                teachcanvas.SetActive(false);
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TeachLevel++;
                }
                break;*/
            case 69:
                //teachcanvas.SetActive(true);
                TextChange("這樣他們就會移動至目標地點了。");
                break;
            case 70:
                teachcanvas.SetActive(false);
                break;
            case 71:
                teachcanvas.SetActive(true);
                TargetSwitch(null);
                Final = true;
                TextChange("當地圖探索完成時，\n我將開始施打藥劑。");
                break;
            case 72:
                TextChange("注射藥劑時,\n敵人就會想要來破壞針筒,");
                break;
            case 73:
                TextChange("請守護好針筒,\n並且留意上方的針筒血量。");
                break;
            case 74:
                teachcanvas.SetActive(true);
                TextChange("請避免讓針筒血量歸零。");
                break;
            case 75:
                teachcanvas.SetActive(false);
                TextChange("<b><color=#FF6F83>記得收集小兵與治療需在7天以內完成。</color></b>");
                break;
            case 76:
                teachcanvas.SetActive(false);
                TextChange("忘記如何操作時，\n可點擊右下角[?]來獲取說明。");
                break;
            case 77:
                TextChange("<b><color=#FF6F83>接下來就拜託你了。</color></b>");
                break;
            case 78:
                //Exit();
                teachcanvas.SetActive(false);
                break;
            case 150:
                teachcanvas.SetActive(true);
                TargetSwitch(null);
                TextChange("拾起的毛果子可在無毛樹的毛孔上復育。");
                break;
            case 151:
                TeachLevel = 43;
                break;
            case 152:
                TextChange("<b><color=#FF6F83>使用 Q、E 鍵切換至毛果子。\n使用滑鼠右鍵將果子種入毛孔。</color></b>");
                break;
            case 153:
                teachcanvas.SetActive(true);
                TargetSwitch(null);
                teaching.SetActive(false);
                syringeteachImage.SetActive(true);
                syringeteachImage.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/syringeTeach/syringe1_ui", typeof(Sprite));
                Time.timeScale = 0f;
                break;
            case 154:
                
                syringeteachImage.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/syringeTeach/syringe2_ui", typeof(Sprite));
                break;
            case 155:
                syringeteachImage.SetActive(false);
                teachcanvas.SetActive(false);
                Time.timeScale = 1.0f;
                FirstMonster = true;
                SkipTeach = true;

                break;
            case 156:
                teachcanvas.SetActive(true);
                teaching.SetActive(true);
                Time.timeScale = 0f;
                TextChange("藥效作用了，請在下一波敵人來臨前，多收集資源吧!");
                break;
            case 157:
                teachcanvas.SetActive(false);
                Time.timeScale = 1.0f;

                FirstFinsh = true;
                SkipTeach = true;
                break;
        }
    }
    public void DoTime(float MaxTime)
    {
        if (TimeToZero == false)
        {

            time2 = 0.0f;
            teachcanvas.SetActive(false);
            TimeToZero = true;
        }
        if (time2 > MaxTime)
        {
            TeachLevel++;
            teachcanvas.SetActive(true);
            TimeToZero = false;

        }
    }

    public void BoostFirsgtPlayFinish()
    {
        TeachLevel = 32;
        teachcanvas.SetActive(true);
        TimeToZero = false;
    }

    public void Exit()
    {
        SkipTeach = true;
        camera.LockMouse();
        StartTeach.SetActive(false);
        NotTeach.SetActive(false);
        ExitTeach.SetActive(false);
        teachcanvas.SetActive(false);
        //Destroy(this.gameObject);

    }
    public void TeachPlay()
    {
        camera.LockMouse();
        teaching.SetActive(true);
        ExitTeach.SetActive(false);
        StartTeach.SetActive(false);
        NotTeach.SetActive(false);
    }

    public void PlayState(int num)
    {
        TeachLevel = num;
    }
    public void TextChange(string text)
    {
        TeachText.text = text;
    }
    public void ImageSwitch(string ImageName)
    {
        if (ImageName != null)
        {
            TeachImage.SetActive(true);
            TeachImage.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/teach/" + ImageName, typeof(Sprite));
            TeachImage.GetComponent<Image>().SetNativeSize();
        }
        else
        {
            TeachImage.GetComponent<Image>().overrideSprite = null;
            TeachImage.SetActive(false);
        }
    }
    public void TargetSwitch(string targettext)
    {
        if (targettext != null)
        {
            TeachTarget.SetActive(true);
            Target.text = targettext;
        }
        else
        {
            Target.text = null;
            TeachTarget.SetActive(false);
            
        }
    }
    public void How2Play()
    {
        guid.SetActive(true);
    }
    public void CloseHow2Play()
    {
        guid.SetActive(false);
        
    }
    /*public void syringeTeach()
    {
        syringeteachImage.SetActive(true);
        syringeteachImage.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/syringeTeach/" + ImageName, typeof(Sprite));
    }*/
    /*public void CanvasSwith()
    {
        if (teachcanvas.activeInHierarchy == true)
        {
            teachcanvas.SetActive(false);
            
        }
        else
        {
            teachcanvas.SetActive(true);
        }
    }*/
    /*public void FindImageSwitch(string ImageName)
    {
        if (ImageName != null)
        {


            FindImage.SetActive(true);
            FindImage.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/teach/" + ImageName, typeof(Sprite));

        }
        else
        {
            FindImage.GetComponent<Image>().overrideSprite = null;
            FindImage.SetActive(false);
        }
    }
    public void Close()
    {
        FindImage.SetActive(false);
    }*/
}



