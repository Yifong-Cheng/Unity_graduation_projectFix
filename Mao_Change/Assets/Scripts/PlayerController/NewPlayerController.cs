using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class NewPlayerController : MonoBehaviour {

    //角色動畫
    public Animator m_anim;
    public GameObject FlagPlayer;

    private BoxBackageUIControl bc;

    public Transform ThrowPos;
    public Transform PutPos;
    private GameObject Model;
    public Transform hand;
    public GameObject FrontObj;

    public int ReturnPosIndex = 0;

    public GameObject[] Tubs;
    public GameObject ShootPos;
    public GameObject sobj;

    private ParticileControl particileControl;
    [SerializeField]
    bool IsFirsttime = false;

    private AIGameManger aIGameManger;
    private TreeManger treeManger;

    public LoadAndSave _data;
    public MaoGame maogame;

    public List<HoleControl> holeList;
    public GameObject AttackTutorial;

    //teachbool
    private bool FirstPickUP = true;
    private bool TwicePickUP = true;
    private bool FirstPutSeed = false;
    public bool FirstPutJar = false;

    private AudioController audioController;
    public GameObject gun;
    public Transform BoostPos;
    private FixedUpdateFollow updateFollow;

    private float currentCreatSmoke = 0;
    private const float WalkSmokeTime = .5f;
    private const float RunSmokeTime = .2f;

    private float currentExitTime = 0;

    private void Awake()
    {
        particileControl = GetComponent<ParticileControl>();
        //m_anim = transform.GetChild(0).GetComponent<Animator>();
        m_anim = GetComponent<Animator>();

        bc = GameObject.FindObjectOfType<BoxBackageUIControl>();
        aIGameManger = GameObject.FindObjectOfType<AIGameManger>();
        treeManger = GameObject.FindObjectOfType<TreeManger>();

        GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().Follow = this.transform;
        GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().LookAt = this.transform.GetChild(0).transform;
        GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().m_AnimatedTarget = m_anim;
        audioController = GetComponent<AudioController>();
        updateFollow = gun.GetComponent<FixedUpdateFollow>();
    }
    private void FixedUpdate()
    {
        DoAnim();

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 8, transform.forward, out hit, 8))
        {
            if (hit.collider.tag == "Enemy")
            {
                currentExitTime = 0;
                Debug.Log("SeeEnemy" + ": " + hit.transform.name);
                AttackTutorial.SetActive(true);

            }
            else
            {
                currentExitTime += Time.deltaTime;
                if(currentExitTime>1.5f)
                {
                    AttackTutorial.SetActive(false);
                }
            }
        }
    }

    public void PlayAnim(string name, float begintime)
    {
        m_anim.Play(name, 0, begintime);
    }

    public void DoBoost()
    {
        m_anim.Play("Boost", 0, 0);
        updateFollow.enabled = true;
        updateFollow.Boost();
        gun.GetComponent<Animator>().Play("Anim", 0, 0);
        particileControl.ContinePlay(1, 1.5F, BoostPos.position,.5f);
    }

    private IEnumerator PlayShoot()
    {
        if (m_anim.GetLayerWeight(1) == 1)
        {
            yield return null;
        }
        else
        {
            m_anim.SetLayerWeight(1, 1);
            m_anim.Play("Shoot", 1, 0);
            updateFollow.enabled = true;
            updateFollow.StartCoroutine(updateFollow.Shoot());
            gun.GetComponent<Animator>().Play("Anim", 0, -.5f);
            yield return new WaitForSeconds(1f);
            m_anim.SetLayerWeight(1, 0);
        }

    }

    private void DoAnim()
    {
        ////////////////////////////////////////NEED RECODING////////////////////////
        //float input_x = Input.GetAxis("Horizontal");
        //float input_y = Input.GetAxis("Vertical");
        //float totalInput = (input_x + input_y);

        if (Input.GetMouseButtonDown(0) && bc.PropInfo() != "GUN")
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                //if (holeList.Count > 0)
                //{
                //    if (bc.PropEnough())
                //    {
                //        if (FirstPutSeed == false && teach.Instance.TeachLevel == 50)
                //        {
                //            teach.Instance.TeachLevel++;
                //            FirstPutSeed = true;
                //        }
                //        PlayAnim("Put", 0);
                //        updateFollow.enabled = false;
                //        updateFollow.ReturnPos();
                //        string Path = bc.PropInfo();
                //        switch (Path)
                //        {
                //            case "S0":
                //                Count.Instance.number[0]--;
                //                break;
                //            case "S1":
                //                Count.Instance.number[1]--;
                //                break;
                //            case "S2":
                //                Count.Instance.number[2]--;
                //                break;
                //            case "S3":
                //                Count.Instance.number[3]--;
                //                break;
                //            case "S4":
                //                Count.Instance.number[4]--;
                //                break;

                //            default:
                //                break;
                //        }
                //        PutSeed(Path);
                //    }
                //}
                //else
                //{
                //    //Debug.LogWarning( "Bc prop is " +bc.Props[bc.Choose].name + "choose = " + bc.Choose + "is" + bc.PropEnough());
                //    if (bc.PropEnough())
                //    {
                //        if (FirstPutJar == false && teach.Instance.SkipTeach == false)
                //        {
                //            teach.Instance.TeachLevel = 63;
                //            FirstPutJar = true;
                //        }

                //        PlayAnim("Put", 0);
                //        //Debug.LogWarning(bc.PropInfo());
                //        string Path = bc.PropInfo();
                //        switch (Path)
                //        {
                //            case "S0":
                //                Count.Instance.number[0]--;
                //                break;
                //            case "S1":
                //                Count.Instance.number[1]--;
                //                break;
                //            case "S2":
                //                Count.Instance.number[2]--;
                //                break;
                //            case "S3":
                //                Count.Instance.number[3]--;
                //                break;
                //            case "S4":
                //                Count.Instance.number[4]--;
                //                break;

                //            default:
                //                break;
                //        }

                //        PutJar(Path);
                //    }
                //}


                if (holeList.Count > 0)
                {
                    
                    if (bc.PropEnough())
                    {
                        PlayAnim("Put", 0);
                        updateFollow.enabled = false;
                        updateFollow.ReturnPos();
                        string Path = bc.PropInfo();
                        switch (Path)
                        {
                            case "S0":
                                Count.Instance.number[0]--;
                                break;
                            case "S1":
                                Count.Instance.number[1]--;
                                break;
                            case "S2":
                                Count.Instance.number[2]--;
                                break;
                            case "S3":
                                Count.Instance.number[3]--;
                                break;
                            case "S4":
                                Count.Instance.number[4]--;
                                break;

                            default:
                                break;
                        }
                        PutSeed(Path);
                    }
                }
                else
                {
                    if (bc.PropEnough())
                    {
                        PlayAnim("Put", 0);
                        string Path = bc.PropInfo();
                        switch (Path)
                        {
                            case "S0":
                                Count.Instance.number[0]--;
                                break;
                            case "S1":
                                Count.Instance.number[1]--;
                                break;
                            case "S2":
                                Count.Instance.number[2]--;
                                break;
                            case "S3":
                                Count.Instance.number[3]--;
                                break;
                            case "S4":
                                Count.Instance.number[4]--;
                                break;

                            default:
                                break;
                        }

                        PutJar(Path);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && bc.PropInfo() == "GUN")
        {
            if(m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                //PlayAnim("Shoot",0f);
                StartCoroutine(PlayShoot());
                ShootObj(.2f);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (FrontObj != null && TwicePickUP == true  && teach.Instance.teachcanvas.activeInHierarchy == false)
        //    {
        //        PlayAnim("PickUp", 0);
        //        TwicePickUP = false;
        //        PickUp();
                
        //    }
        //    else if (FrontObj != null && TwicePickUP == true && teach.Instance.TeachLevel != 41 && teach.Instance.teachcanvas.activeInHierarchy == false)
        //    {
        //        PlayAnim("PickUp", 0);
        //        TwicePickUP = false;
        //        PickUp();
                
        //    }
        //    else if (FrontObj != null && teach.Instance.teachcanvas.activeInHierarchy == false)
        //    {
        //        PlayAnim("PickUp", 0);
        //        PickUp();
        //    }
        //}

        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            currentCreatSmoke += Time.deltaTime;
            if(currentCreatSmoke>WalkSmokeTime)
            {
                currentCreatSmoke = 0;
                particileControl.Play(2, 1, transform.position + new Vector3(0, .5f, 0));
            }
            audioController.PlaySound(3);
        }
        else if(m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            currentCreatSmoke += Time.deltaTime;
            if (currentCreatSmoke > RunSmokeTime)
            {
                currentCreatSmoke = 0;
                particileControl.Play(2, 1, transform.position+new Vector3(0,.5f,0));
            }
            audioController.StopIndexSound(3);
            audioController.PlaySound(0);
        }
        else
        {
            audioController.StopSound(3);
            audioController.StopSound(0);
        }

    }

    #region 材料介面

    public void OpenBackageUI()
    {
        //BackageUI.SetActive(true);
        //Energy_Text.text = m_bgInfo.Instance.Enegy.ToString();
        //Mat_0_Text.text = m_bgInfo.Instance.Mat0.ToString();
        //Mat_1_Text.text = m_bgInfo.Instance.Mat1.ToString();
        //Mat_2_Text.text = m_bgInfo.Instance.Mat2.ToString();
        Time.timeScale = 0;
    }

    public void CloseBackageUI()
    {
        Time.timeScale = 1;
        //BackageUI.SetActive(false);
    }

    #endregion

    #region 罐子

    public void ThrowJar()
    {
        GameObject go = Resources.Load("Jar") as GameObject;
        go.transform.position = ThrowPos.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.GetComponent<BoxCollider>().isTrigger = false;
        go.GetComponent<Rigidbody>().isKinematic = false;
        go.GetComponent<ObjInfo>().Type = "Throw";

        GameObject Jar =  Instantiate(go);
        Jar.GetComponent<Rigidbody>().AddForce(ThrowPos.transform.forward * 10, ForceMode.Impulse);
        Jar.transform.Rotate(0, 2, 0);
    }

    public void PickUp()
    {
        updateFollow.enabled = false;
        updateFollow.ReturnPos();
        particileControl.Play(0, 1.02F,this.transform.position+transform.forward*2);
        if (FrontObj != null)
        {
            ObjInfo info;
            info = FrontObj.GetComponent<ObjInfo>();
            FrontObj = null;
            CheckFrontObj(info);
        }
            
    }

    public void PutSeed(string sln)
    {
        GameObject go = Resources.Load<GameObject>("SeedMao/"+sln);
        //go.transform.position = PutPos.transform.position;
        //go.transform.rotation = Quaternion.identity;
        GameObject.Instantiate(go, PutPos.transform.position+new Vector3(0,0.8f,0),Quaternion.identity);
    }

    

    private void CheckFrontObj(ObjInfo info)
    {
        if(info!=null)
        {
            //audioController.PlaySound(2);
            if (info.Type == "Crystal")
            {
                Count.Instance.Crystal += 5.0f;
                Destroy(info.gameObject);
            }
            else
            {
                switch (info.FollowerTypeID)
                {
                    case 0:
                        {
                            //Debug.Log("是茅仔");
                            Count.Instance.number[0]++;
                            Destroy(info.gameObject);
                            MyTutorial.Instance.FirstPickS0();
                        }
                        break;

                    case 1:
                        {
                            //Debug.Log("是集合毛");
                            Count.Instance.number[1]++;
                            Destroy(info.gameObject);
                            MyTutorial.Instance.FirstPickS1();
                        }
                        break;

                    case 2:
                        {
                            //Debug.Log("是炸彈");
                            Count.Instance.number[2]++;
                            Destroy(info.gameObject);
                            MyTutorial.Instance.FirstPickS2();
                        }
                        break;

                    case 3:
                        {
                            //Debug.Log("是頭腳毛");
                            Count.Instance.number[3]++;
                            Destroy(info.gameObject);
                            MyTutorial.Instance.FirstPickS3();
                        }
                        break;

                    case 4:
                        {
                            //Debug.Log("是壯壯");
                            Count.Instance.number[4]++;
                            Destroy(info.gameObject);
                            MyTutorial.Instance.FirstPickS4();
                        }
                        break;
                }
            }
        }
        
    }

    public void PutJar(string sln)
    {

        GameObject go = Resources.Load("Jar") as GameObject;
        go.transform.position = PutPos.transform.position;
        go.transform.rotation = Quaternion.identity;
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<ObjInfo>().Type = "Put";
        go.GetComponent<JarController>().SoilderName = sln;
        go.GetComponent<JarController>().aIGameManger = aIGameManger;

        Instantiate(go);
        
    }

    #endregion

    public void HideAndShowTubs(int Showid)
    {
        for(int i=0;i<Tubs.Length;i++)
        {
            if(i==Showid)
            {
                Tubs[i].SetActive(true);
            }
            else
            {
                Tubs[i].SetActive(false);
            }
        }
    }

    public void HideAllTube()
    {
        for (int i = 0; i < Tubs.Length; i++)
        {
            Tubs[i].SetActive(false);
        }
    }

    public void ShootObj(float waittime)
    {
        //StartCoroutine(audioController.PlaySound(1, .5f));

        Invoke("Shoot", waittime);
        //Shoot();
    }

    private void Shoot()
    {
        GameObject go = Resources.Load<GameObject>("Bullet");

        sobj = Instantiate(go, ShootPos.transform.position, ShootPos.transform.rotation);

        sobj.GetComponent<Rigidbody>().AddForce(ShootPos.transform.forward * 8, ForceMode.Impulse);
        sobj.name = "Bullet";
        //particileControl.Play(1, 2.02F);
    }

    public bool CheckInRange()
    {
        
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("StageDoor"))
        {
            aIGameManger.TurnStageSave();
            treeManger.TurnStageSave();
            _data.m_datainfolist.playerdata.LastPos = (transform.position - 5*transform.forward);
            _data.m_datainfolist.stageData.RunTime += 1;
            maogame.NextStage = other.GetComponent<DoorInfo>().NextStage;
            maogame.TurnStage = true;
        }
    }
}
