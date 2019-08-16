using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowMaoControl : MonoBehaviour
{
    [Header("樹模型")]
    public GameObject Art;
    public Animator m_anim;
    public GameObject seed;
    public List<GameObject> ShootPos;
    public string StateInfo;
    public LoadAndSave data;
    public float currentTime,GrowTime,finishTime,creatTime;
    [Range(1,50)]
    public float waitTime;
    public int currentBoostIndex, TotalBoostindex;

    bool changeModel, IsChange,IsCreat;

    public int CurrentCreatNum;

    //public enum GrowState
    //{
    //    Wait =0,
    //    Prepare =1,
    //    Grow =2,
    //    Finish =3,
    //    Creat =4,
    //    Return =5,
    //}
    //public GrowState growState = GrowState.Return;
    public int growState;
    private int creatNum;
    private bool FirstPlay = false;

    private int runtime;
    private Renderer RenderGamObj;
    private float shaderdelaytime = .3f;
    private ParticileControl particileControl;

    private void Awake()
    {
        Art = this.gameObject;
        m_anim = Art.GetComponent<Animator>();
        //RenderGamObj = Art.transform.GetChild(2).GetComponent<Renderer>(); 
        particileControl = GetComponent<ParticileControl>();
    }
    // Use this for initialization
    void Start () {
        //currentTime = 0;
        //waitTime = 50f;
        GrowTime = 4f;
        finishTime = 2f;
        TotalBoostindex = 5;
	}
	
	// Update is called once per frame
	void Update () {
        StateInfo = growState.ToString();

        CheckState();
	}

    void CheckState()
    {
        currentTime += Time.deltaTime;
        switch (growState)
        {
            case 0:
                {
                    if(changeModel==true&&IsChange==false && currentTime<waitTime)
                    {
                        changeModel = false;
                        //HideAndShowMao(0);
                        Art.transform.localScale = new Vector3(.5f, .5f, .5f);
                    }

                    if(currentTime>waitTime)
                    {
                        changeModel = true;
                        IsChange = false;
                        currentTime = 0;
                        Art.transform.localScale = new Vector3(1, 1, 1);
                        ResetDissloveShader();
                        growState = 1;
                    }
                }
                break;

            case 1:
                {
                    if(currentBoostIndex==1&&!FirstPlay)
                    {
                        FirstPlay = true;
                        //m_anim.Play("Boost1");
                    }
                    if(currentBoostIndex>TotalBoostindex)
                    {
                        currentBoostIndex = 0;
                        m_anim.Play("Boost");
                        FirstPlay = false;
                        growState = 2;
                    } 
                }
                break;

            case 2:
                {
                    if (changeModel == true && IsChange == false && currentTime < GrowTime)
                    {
                        changeModel = false;
                        //HideAndShowMao(2);
                    }

                    if (currentTime > GrowTime)
                    {
                        changeModel = true;
                        IsChange = false;
                        currentTime = 0;
                        growState = 3;
                    }
                }
                break;

            case 3:
                {
                    if (changeModel == true && IsChange == false && currentTime < GrowTime)
                    {
                        changeModel = false;
                        //HideAndShowMao(1);
                    }

                    if (currentTime > finishTime)
                    {
                        changeModel = true;
                        IsChange = false;
                        currentTime = 0;
                        IsCreat = true;
                        growState = 4;
                    }
                }
                break;

            case 4:
                {
                    //docreat
                    //if(IsCreat && teach.Instance.PickupFirstTime == false && teach.Instance.TeachLevel == 38)
                    //{
                    //    IsCreat = false;
                    //    teach.Instance.PickupFirstTime = true;
                    //    creatNum = Random.Range(2, 6);
                    //    //CreatSeed(creatNum);
                    //    //StartCoroutine(DissloveShader(shaderdelaytime));
                    //    StartCoroutine(CreatSeed(.3f, creatNum));
                    //    teach.Instance.TeachLevel = 39;

                    //}
                    //else if (IsCreat && teach.Instance.PickupTwiceTime == false && teach.Instance.TeachLevel == 53)
                    //{
                    //    IsCreat = false;
                    //    teach.Instance.PickupTwiceTime = true;
                    //    creatNum = Random.Range(2, 6);
                    //    //CreatSeed(creatNum);
                    //    //StartCoroutine(DissloveShader(shaderdelaytime));
                    //    StartCoroutine(CreatSeed(.3f, creatNum));
                    //    teach.Instance.TeachLevel = 54;

                    //}
                    //else 
                    if (IsCreat)
                    {
                        IsCreat = false;
                        creatNum = Random.Range(2, 6);
                        //CreatSeed(creatNum);
                        //StartCoroutine(DissloveShader(shaderdelaytime));
                        StartCoroutine(CreatSeed(.3f, creatNum));
                    }
                    
                    //growState = 5;
                }
                break;

            case 5:
                {
                    changeModel = true;
                    IsChange = false;
                    currentTime = 0;
                    Art.transform.localScale = new Vector3(1, 1, 1);
                    //ResetDissloveShader();
                    growState = 0;
                }
                break;
        }
    }

    public void ScaleModel()
    {
        if(growState==1)
        {
            float boostrange = currentBoostIndex * 0.1f;
            Art.transform.localScale = new Vector3(1 + boostrange, 1 + boostrange, 1 + boostrange);
        }
        else
        {
        }
    }

    void CreatSeed(int num)
    {
        particileControl.Play(0, 2, ShootPos[0].transform.parent.transform.position);
        for (int i=0;i<num;i++)
        {
            Instantiate(seed, ShootPos[i].transform.position, ShootPos[i].transform.rotation);
            seed.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(1, 5), ForceMode.Impulse);
        }
    }

    IEnumerator CreatSeed(float delayTime,int num)
    {
        particileControl.Play(0, 5, ShootPos[0].transform.parent.transform.position);
        while(CurrentCreatNum < num)
        {
            CurrentCreatNum++;
            Instantiate(seed, ShootPos[CurrentCreatNum].transform.position, ShootPos[CurrentCreatNum].transform.rotation);
            seed.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(1, 5), ForceMode.Impulse);
            yield return new WaitForSeconds(delayTime);
        }
        yield return new WaitForSeconds(delayTime);
        CurrentCreatNum = 0;
        yield return new WaitForSeconds(delayTime * 2);
        m_anim.Play("Idle");
        growState = 5;
        //if (CurrentCreatNum<num)
        //{
        //    yield return new WaitForSeconds(delayTime);
        //    Instantiate(seed, ShootPos[CurrentCreatNum].transform.position, ShootPos[CurrentCreatNum].transform.rotation);
        //    seed.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(1, 5), ForceMode.Impulse);
        //    CurrentCreatNum++;
        //    StartCoroutine(CreatSeed(delayTime, num));
        //}
        //else
        //{
        //    yield return new WaitForSeconds(delayTime);
        //    CurrentCreatNum = 0;
        //    yield return new WaitForSeconds(delayTime*2);
        //    m_anim.Play("Idle");
        //    growState = 5;
        //}
        
    }

    #region shader

    
    public IEnumerator DissloveShader(float waitTime)
    {
        if (RenderGamObj != null)
        {
            float buff = .3f;
            if (runtime < 1)
            {
                yield return new WaitForSeconds(waitTime * 2);
            }
            runtime += 1;
            if (RenderGamObj != null)
            {
                if (RenderGamObj.material.GetFloat("_DissolveValue") < 1)
                {
                    //RenderGamObj.gameObject.transform.localScale += new Vector3(buff * runtime, buff * runtime, buff * runtime);
                    RenderGamObj.material.SetFloat("_DissolveValue", buff * runtime);
                    yield return new WaitForSeconds(waitTime);
                    StartCoroutine(DissloveShader(waitTime));
                }
                else
                {
                    yield return null;
                    StopAllCoroutines();
                }
            }
            else
            {
                yield return null;
            }
            
        }
    }

    private void ResetDissloveShader()
    {
        if (RenderGamObj != null)
        {
            //RenderGamObj.gameObject.transform.localScale = new Vector3(1, 1, 1);
            RenderGamObj.material.SetFloat("_DissolveValue", 0);
        }
    }

    #endregion

}
