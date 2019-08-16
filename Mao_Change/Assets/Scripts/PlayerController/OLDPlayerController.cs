using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OLDPlayerController : MonoBehaviour {
    //角色控制
    //private PlayerMachine playerMachine;
    private const string TemporaryLayer = "TempCast";
    private int TemporaryLayerIndex;
    private float camRayLength = 100f;
    public GameObject FrontSight;
    private GameObject FrontSightImg;
    private const string FrontSightGameObjName = "FrontSight";
    private Quaternion quaternion;
    public float FrontLength;
    private float LocalRotate;
    //準心座標
    private Vector3 hitend;
    //實體模型
    private GameObject Model;
    //縮放範圍
    public float maxZoomSize=0.13f, minZoomSize = 0.03f;
    public float ZoomRange = 0.1f;
    //準心原旋轉決度
    private float localRotate;
    //準心
    private GameObject FollowerInfoObjs;
    private GameObject Followers;
    private int FollowerTypeCount;
    private int TotalFollerType;
    private List<GameObject> FollowerList = new List<GameObject>();
    public List<GameObject> AttackerGameObjs = new List<GameObject>();
    //識別碼
    private string EnemyTypeString = "Enemy";
    private string TaskTypeString = "Task";
    //角色動畫
    private Animator m_anim;
    //背包素材
    public BackageInfo m_bgInfo = new BackageInfo();
    public GameObject BackageUI;
    public Text Energy_Text;
    public Text Mat_0_Text;
    public Text Mat_1_Text;
    public Text Mat_2_Text;
    //

    public int ReturnPosIndex = 0;

    private void Awake()
    {
        //
        TemporaryLayerIndex = LayerMask.NameToLayer(TemporaryLayer);
        //
        FrontLength = 2.5f;
        GameObject FrontSightGameObj = Resources.Load(FrontSightGameObjName) as GameObject;
        FrontSightGameObj.transform.position = this.transform.position;
        FrontSightGameObj.name = FrontSightGameObjName;
        Instantiate(FrontSightGameObj);
        //實體模型
        Model = transform.GetChild(0).gameObject;
        m_anim = Model.GetComponent<Animator>();
        //追隨者顯示表
        FollowerInfoObjs = GameObject.Find("FollowerInfoObjs");
        TotalFollerType = FollowerInfoObjs.transform.childCount;
        Followers = GameObject.Find("Followers");

        //取得角色移動數值
        //playerMachine = GetComponent<PlayerMachine>();

        //素材初始化
        m_bgInfo.Instance.Enegy = 100;
        //Debug.Log( m_bgInfo.Instance.Enegy);

    }
    private void Start()
    {
        //準心
        FrontSight = GameObject.Find(FrontSightGameObjName+"(Clone)").gameObject;
        //FrontSight = Model.transform.GetChild(4).gameObject;
        quaternion = FrontSight.transform.GetChild(0).transform.rotation;
        FrontSightImg = FrontSight.transform.GetChild(0).gameObject;

        //追隨者顯示表內容
        for (int i=0;i< TotalFollerType; i++)
        {
            FollowerInfoObjs.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition
            = new Vector3(FollowerInfoObjs.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition.x - i * 400, 0, 0);

            FollowerList.Add(FollowerInfoObjs.transform.GetChild(i).gameObject);
        }
        FollowerTypeCount = 0;

    }

    // Update is called once per frame
    void Update () {
        FrontSightMove();
        FrontSightZoom();
        FrontSightClick();
        FrontSightTriggerEvent();

        SoilderChange();
        DoAnim();
    }

    private void DoAnim()
    {
        float input_x = Input.GetAxis("Horizontal");
        float input_y = Input.GetAxis("Vertical");
        float totalInput = (input_x + input_y);

        //m_anim.SetFloat("Walk", totalInput * 5);
       // m_anim.SetFloat("Walk", playerMachine.moveDirection.magnitude);
        

    }

    #region 轉向->PlayerInputManger
    //public void Turning()
    //{
    //    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit floorHit;

    //    if (Physics.Raycast(camRay, out floorHit, camRayLength, TemporaryLayerIndex))
    //    {
    //        // Create a vector from the player to the point on the floor the raycast from the mouse hit.
    //        Vector3 playerToMouse = floorHit.point - transform.position;

    //        // Ensure the vector is entirely along the floor plane.
    //        playerToMouse.y = 0f;

    //        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
    //        //Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
    //        // Set the player's rotation to this new rotation.
    //        Model.transform.rotation = Quaternion.LookRotation(playerToMouse);

    //    }
    //}
    #endregion

    #region FrontSight準心
    private void FrontSightMove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit);
        hitend = hit.point;
        //FrontSight.transform.rotation = quaternion;
        FrontSight.transform.position = new Vector3(hitend.x, hitend.y + (1.2f), hitend.z);
        FrontSight.transform.rotation = Model.transform.rotation;

        //------------------------------------------------------

        //FrontSight.transform.position = new Vector3(Model.transform.position.x, Model.transform.position.y, Model.transform.position.z+50);
        //FrontSight.transform.position = new Vector3( Model.transform.forward.x*20* Model.transform.rotation.y, Model.transform.forward.y+1, Model.transform.forward.z*20* Model.transform.rotation.y);
        //FrontSight.transform.rotation = Model.transform.rotation;
        //FrontSight.transform.position = new Vector3(Model.transform.position.x, Model.transform.position.y+1.2f, Model.transform.position.z);
        //FrontSight.transform.position = Input.mousePosition;
        //FrontSight.transform.rotation = Model.transform.rotation;

    }

    private void FrontSightZoom()
    {
        float ZoomSize = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            if(FrontSight.transform.localScale.x<=maxZoomSize)
            {
                FrontSight.transform.localScale += new Vector3(ZoomRange, 0, ZoomRange) * ZoomSize;
                if(FrontSight.transform.localScale.x > maxZoomSize)
                {
                    FrontSight.transform.localScale = new Vector3(maxZoomSize, 0, maxZoomSize);
                }
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            if (FrontSight.transform.localScale.x > minZoomSize)
            {
                FrontSight.transform.localScale += new Vector3(ZoomRange, 0, ZoomRange) * ZoomSize;
                if (FrontSight.transform.localScale.x < minZoomSize)
                {
                    FrontSight.transform.localScale = new Vector3(minZoomSize, 0, minZoomSize);
                }
            }
        }

    }
    private void FrontSightClick()
    {
        Collider m_collider = FrontSight.GetComponent<Collider>();
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.E))
        {
            m_collider.enabled = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            m_collider.enabled = false;
        }
        else
        {
            //m_collider.enabled = false;
        }

    }

    //
    private void FrontSightTriggerEvent()
    {
        
        localRotate = Model.transform.localRotation.y;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach(RaycastHit hit in hits)
        {
            ObjInfo objinfo = hit.transform.gameObject.GetComponent<ObjInfo>();
            if(objinfo != null )
            {
                ColliderTypeEvent(objinfo.Type,hit.transform.gameObject);

                //如果為怪物或任務則立起
                if(objinfo.Type == EnemyTypeString || objinfo.Type == TaskTypeString)
                {
                    FrontSightImg.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    FrontSightImg.transform.rotation = quaternion;
                }

                if (localRotate % 360 > 0 && localRotate % 360 <= 90)
                {
                    FrontSight.transform.position = new Vector3(hit.transform.position.x - FrontLength, hit.transform.position.y, hit.transform.position.z - FrontLength);
                }
                else if (localRotate % 360 > 90 && localRotate % 360 <= 180)
                {
                    FrontSight.transform.position = new Vector3(hit.transform.position.x - FrontLength, hit.transform.position.y, hit.transform.position.z + FrontLength);
                }
                else if (localRotate % 360 > 180 && localRotate % 360 <= 270)
                {
                    FrontSight.transform.position = new Vector3(hit.transform.position.x + FrontLength, hit.transform.position.y, hit.transform.position.z + FrontLength);
                }
                else if (localRotate % 360 > 270 && localRotate % 360 <= 360)
                {
                    FrontSight.transform.position = new Vector3(hit.transform.position.x + FrontLength, hit.transform.position.y, hit.transform.position.z - FrontLength);
                }

            }
            else
            {
                FrontSightImg.transform.rotation = quaternion;
            }
        }
    }
    #region  射擊判斷物
    public void ColliderTypeEvent(string type,GameObject TriggerObj)
    {
        switch(type)
        {
            case "Task":

                break;

            case "LittleBoss":
                if (Input.GetMouseButtonDown(0))
                {
                    AttackerSoilder(TriggerObj);
                    
                }

                break;

            case "Soilder":
                //FrontSightClick();
                //防禦用
                //好像又不用...
                break;

            case "Player":
                {
                    if (Input.GetMouseButtonDown(0))
                        OpenBackageUI();
                }
                break;

            case "Factory":
                {
                    FactoryViewer viwer = TriggerObj.GetComponent<FactoryViewer>();
                    if (Input.GetMouseButtonDown(0))
                    {
                        viwer.ShowFactoryUi();
                    }
                }

                break;

            case "Energy":
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        SoilderCarryThing(TriggerObj);
                        //AttackerSoilder(TriggerObj);
                    }
                }
                break;

            case "TheHole":
                {
                    //Debug.Log("HitTheHole");
                    HoleController holeController = TriggerObj.GetComponent<HoleController>();
                    if (Input.GetMouseButtonDown(0) && holeController.TakeBreak==false&&holeController.IsWorking==false)
                    {
                        SoilderAutoCreat(TriggerObj);
                        //AttackerSoilder(TriggerObj);
                    }
                }
                break;

            default:

                break;
        }
    }

    private void SoilderReturn(string profession)
    {

    }

    #endregion
    #endregion


    #region 追隨者內容顯示與切換

    //更改目前準備派出小兵種類之介面
    private void SoilderProfessionUI()
    {
        for (int i = 0; i < TotalFollerType; i++)
        {
            //FollowerInfoObjs.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition
            //= new Vector3(FollowerInfoObjs.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition.x - i * 400, 0, 0);
            if (i == FollowerTypeCount)
            {
                FollowerList[i].gameObject.GetComponent<RectTransform>().anchoredPosition
                = new Vector3(0, 0, 0);
            }
            else if (i < TotalFollerType && i != FollowerTypeCount)
            {
                FollowerList[i].gameObject.GetComponent<RectTransform>().anchoredPosition
                = new Vector3(-400, 0, 0);
            }
            //else if (i == TotalFollerType && i != FollowerTypeCount)
            //{
            //    FollowerList[i].gameObject.GetComponent<RectTransform>().anchoredPosition
            //    = new Vector3(-400, 0, 0);
            //}
            //else if(i == TotalFollerType && i == FollowerTypeCount)
            //{
            //    FollowerList[i].gameObject.GetComponent<RectTransform>().anchoredPosition
            //    = new Vector3(0, 0, 0);
            //}
            

        }
    }

    private void SoilderChange()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //清空列表
            AttackerGameObjs.Clear();

            //更改目前準備派出小兵種類之介面ID
            if(FollowerTypeCount<TotalFollerType && FollowerTypeCount != (TotalFollerType - 1))
            {
                FollowerTypeCount++;
            }
            else if(FollowerTypeCount == (TotalFollerType-1))
            {
                FollowerTypeCount = 0;
            }


            //Debug.Log(FollowerTypeCount);

            //更改目前準備派出小兵種類之介面
            SoilderProfessionUI();

            //將即將派出種類之小兵加入攻擊者列表
            AddAttackersToList(FollowerTypeCount);
        }
    }

    //當重複收放小兵時須定期更新
    public void UpdateFollowerTeamList()
    {
        AttackerGameObjs.Clear();
        AddAttackersToList(FollowerTypeCount);
    }

    //將即將用來派出的小兵加入列表預備使用
    private void AddAttackersToList(int SoilderType)
    {
        int TypeAttackers = Followers.transform.GetChild(SoilderType).childCount;
        if(TypeAttackers<=0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < TypeAttackers; i++)
            {
                AttackerGameObjs.Add(Followers.transform.GetChild(SoilderType).GetChild(i).gameObject);
            }
        }
        
    }

    //從攻擊者列表中取最後一位派出
    private void AttackerSoilder(GameObject Target)
    {
        
        int Num = (AttackerGameObjs.Count);
        if (Num<=0 || AttackerGameObjs==null)
        {
            //可以做放煙花
            return;
        }
        else
        {
            AttackerGameObjs[Num-1].GetComponent<AIStateController>().theTarget = Target;
            //AttackerGameObjs[Num-1].GetComponent<AIStateController>().m_aiState = AIStateController.AIState.ATTACK;
            RemoveTheLastAttacker();

        }
            
    }

    private void SoilderCarryThing(GameObject Target)
    {
        int Num = (AttackerGameObjs.Count);
        if (Num <= 0 || AttackerGameObjs == null)
        {
            //可以做放煙花
            return;
        }
        else
        {
            AttackerGameObjs[Num - 1].GetComponent<AIStateController>().theTarget = Target;
            //AttackerGameObjs[Num - 1].GetComponent<AIStateController>().m_aiState = AIStateController.AIState.CARRY;
            RemoveTheLastAttacker();

        }
    }

    private void SoilderAutoCreat(GameObject Target)
    {
        int Num = (AttackerGameObjs.Count);
        if (Num <= 0 || AttackerGameObjs == null)
        {
            //可以做放煙花
            return;
        }
        else
        {
            AttackerGameObjs[Num - 1].GetComponent<AIStateController>().theTarget = Target;
            //AttackerGameObjs[Num - 1].GetComponent<AIStateController>().m_aiState = AIStateController.AIState.AutoCreat;
            RemoveTheLastAttacker();

        }
    }

    private void RemoveTheLastAttacker()
    {
        int AttackerCount = AttackerGameObjs.Count;
        AttackerGameObjs.RemoveAt(AttackerCount - 1);
    }

    #endregion

    #region 材料收集



    #endregion

    #region 材料介面

    public void OpenBackageUI()
    {
        BackageUI.SetActive(true);
        Energy_Text.text = m_bgInfo.Instance.Enegy.ToString();
        Mat_0_Text.text = m_bgInfo.Instance.Mat0.ToString();
        Mat_1_Text.text = m_bgInfo.Instance.Mat1.ToString();
        Mat_2_Text.text = m_bgInfo.Instance.Mat2.ToString();
        Time.timeScale = 0;
    }

    public void CloseBackageUI()
    {
        Time.timeScale = 1;
        BackageUI.SetActive(false);
    }

    #endregion
}
