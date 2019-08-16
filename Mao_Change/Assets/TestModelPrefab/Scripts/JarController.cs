using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JarController : MonoBehaviour {
    //
    protected ObjInfo objInfo;
    //
    protected int JarMotion;
    protected bool IsCreat = false;
    //
    public GameObject TimeSliderCanvas;
    private Slider m_slider;
    private float t = 0;

    private float AutoReturnTime = 15;
    private float CurrentReturnTime = 0;
    [HideInInspector] public bool IsReturn = false;
    private SoilderAIStateController m_stctrl;
    public string SoilderName = "";

    public AIGameManger aIGameManger;

    public GameObject SMOKE;
    [Header("罐子爆破")]
    public Animator m_anim;
    private const string ExplosionClip = "Explosion";
    private AudioController audioController;
    private bool EffectBool;

    // Use this for initialization
    void Start () {
        m_slider = TimeSliderCanvas.transform.GetChild(0).GetComponent<Slider>();
        objInfo = GetComponent<ObjInfo>();
        switch(objInfo.Type)
        {
            case "PickUp":
                JarMotion = 0;
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                break;

            case "Put":
                JarMotion = 1;
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                TimeSliderCanvas.SetActive(true);

                break;
        }
        audioController = GetComponent<AudioController>();
        Destroy(this.gameObject, 30f);
		
	}
	
	// Update is called once per frame
	void Update () {
		if(JarMotion==1&&IsCreat==false)
        {
            CurrentTime(30);
        }
        else if(JarMotion == 1 && IsCreat == true)
        {
            CurrentReturnTime += Time.deltaTime;
            if(CurrentReturnTime>AutoReturnTime)
            {
                IsReturn = true;
                //Destroy(J_Solider.gameObject, 0.5f);
                //m_stctrl.m_aiState = AIStateController.AIState.DEAD;
                Debug.Log("changestate");
                Destroy(this.gameObject);
            }
        }


	}

    private void CreatSoilder(string Name )
    {
        //GameObject SoilderPfb;
        //SoilderPfb = Resources.Load(Name) as GameObject;
        ////SoilderPfb.GetComponent<AIStateController>().AddCompoments();
        //SoilderPfb.transform.position = this.transform.position;
        //SoilderPfb.transform.rotation = Quaternion.identity;
        //J_Solider = Instantiate(SoilderPfb);
        //m_stctrl = J_Solider.GetComponent<AIStateController>();
        Vector3 pos = this.transform.position + new Vector3(0, 0.5f, 0);
        Quaternion spawnRotation = new Quaternion();
        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
        SoilderAIStateController ai;
        switch(Name)
        {
            case ("S0"):
                for(int i=0;i<5; i++)
                {
                    Vector3 p;
                    switch(i)
                    {
                        case 0:
                            p = pos + this.transform.forward;
                            CreatBaseMao(Name, p, spawnRotation);
                            break;
                        case 1:
                            p = pos - this.transform.forward;
                            CreatBaseMao(Name, p, spawnRotation);
                            break;
                        case 2:
                            p = pos + transform.right;
                            CreatBaseMao(Name, p, spawnRotation);
                            break;
                        case 3:
                            p = pos - transform.right;
                            CreatBaseMao(Name, p, spawnRotation);
                            break;
                        case 4:
                            p = pos;
                            CreatBaseMao(Name, p, spawnRotation);
                            break;

                    }
                }
                
                //ai = new LittleMaoStateController();

                //aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, Quaternion.identity);
                //m_stctrl = ai;

                break;

            case ("S1"):
                //ai = new SummonMaoStateController();
                //aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, Quaternion.identity);
                //m_stctrl = ai;
                CreatSummonMao(Name, pos, spawnRotation);
                break;

            case ("S2"):
                //ai = new BombMaoStateController();
                //aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, Quaternion.identity);
                //m_stctrl = ai;
                CreatBombMao(Name, pos, spawnRotation);
                break;

            case ("S3"):
                //ai = new HeadMaoStateController();
                //aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, Quaternion.identity);
                //m_stctrl = ai;
                CreatHeadMao(Name, pos, spawnRotation);
                break;

            case ("S4"):
                //ai = new BattleMaoStateController();
                //aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, Quaternion.identity);
                //Debug.Log(this.transform.position + " / " + ai.Role.transform.position);
                //m_stctrl = ai;
                CreatStrongMao(Name, pos, spawnRotation);
                break;

            default:
                break;

        }
        
    }

    private void CreatBaseMao(string Name,Vector3 pos,Quaternion q)
    {
        LittleMaoStateController ai = new LittleMaoStateController();
        aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, q);
    }
    private void CreatStrongMao(string Name,Vector3 pos, Quaternion q)
    {
        BattleMaoStateController ai = new BattleMaoStateController();
        aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, q);
    }

    private void CreatSummonMao(string Name, Vector3 pos, Quaternion q)
    {
        SummonMaoStateController ai = new SummonMaoStateController();
        aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, q);
    }

    private void CreatHeadMao(string Name, Vector3 pos, Quaternion q)
    {
        HeadMaoStateController ai = new HeadMaoStateController();
        aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos,q);
    }

    private void CreatBombMao(string Name, Vector3 pos, Quaternion q)
    {
        BombMaoStateController ai = new BombMaoStateController();
        aIGameManger.CreatSoilderRole(ai, "Props/" + Name, pos, q);
    }


    private void CurrentTime( int speed)
    {
        
        if (t<m_slider.maxValue)
        {
            m_slider.value = t;
            t += (Time.deltaTime) * speed;
            if (t > (m_slider.maxValue - 35F))
            {

                PlayEffect();
                //GetComponent<ParticileControl>().Play(0, 1.5F);
            }
        }
        else
        {
            //PlayEffect();
            //GetComponent<ParticileControl>().Play(0, 1.5F);
            IsCreat = true;
            
            CreatSoilder(SoilderName);
            //
           
            //
            TimeSliderCanvas.SetActive(false);
        }
    }

    private void PlayEffect()
    {
        if(!EffectBool)
        {
            EffectBool = true;
            audioController.PlayAudioClip(0, 2);
            m_anim.Play(ExplosionClip, 0, 0);
            SMOKE.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(JarMotion==0 && !IsCreat)
        //{
        //    IsCreat = true;
        //    CreatSoilder(SoilderName);
        //    gameObject.GetComponent<BoxCollider>().isTrigger = false;
        //}
    }
}
