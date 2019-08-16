using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIStateController
{
    public bool IsEnable=true;
    protected GameObject player;
    public GameObject SummonTarget;
    protected ObjInfo m_objInfo;
    public GameObject theTarget;
    public Animator m_anim;
    public GameObject ModelPrefab;
    protected AudioController audioController;

    //停止距離
    protected float StopDistance = 10f;
    //計數時間
    protected float currentTime;
    //攻擊間隔
    protected float AttackTime = 3f;

    //wanderCurrentTime
    protected float currentWanderTime = 0;
    protected float currentWaitTime = 0;
    protected float wanderTime;
    protected float WaitTime;

    protected float currentAvoidTime = 0;
    protected float AvoidTime = 1.8f;

    //public Transform[] SeeView = new Transform[3];
    protected float LookRadious;
    protected float AttackDistance;
    protected float ChaseDistance;
    public GameObject Role;
    public string msg;
    public PlayerHealth p_health;
    public AIGameManger aIGameManger;
    public CapsuleCollider targetPos;
    protected NavMeshAgent nav;
    public Vector3 RolePos;
    public string PfbTypeName = "";
    protected ParticileControl particileControl;

    public bool BeAttack;

    public string proprity;
    protected const string HeavyProprity = "Heavy";
    protected const string MediumProprity = "Medium";
    protected const string GentleProprity = "Gentle";

    public void Update()
    {
        FixUpdateAI();
    }

    //初始化
    public abstract void Intialize(AIGameManger _aIGameManger);

    protected abstract void FixUpdateAI();

    protected abstract void StateAction();


    #region 加元件
    protected virtual void AddCompoments()
    {
        if (m_objInfo == null)
        {
            m_objInfo = Role.GetComponent<ObjInfo>();
            if (m_objInfo == null)
            {
                Role.AddComponent<ObjInfo>();
            }
        }
    }

    public virtual void SetValue()
    {
        //Debug.Log(Role.name + "'s Stopdis = " + nav.stoppingDistance);
        AttackTime = 3f;
        AttackDistance = nav.stoppingDistance + 5f;
        ChaseDistance = nav.stoppingDistance * 5;
        LookRadious = nav.stoppingDistance + 3f;
        wanderTime = 0.5f;
        WaitTime = 30f;
        StopDistance = nav.stoppingDistance;
    }

    #endregion

    protected virtual void SetObjInfo()
    {
        m_objInfo = Role.GetComponent<ObjInfo>();
    }

    protected void InputProcess()
    {
        if (Input.GetMouseButtonUp(0) == false)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            AIStateControllerDeBug aIStateControllerDeBug = Role.transform.GetChild(0).GetComponent<AIStateControllerDeBug>();
            if (aIStateControllerDeBug != null && hit.transform.gameObject == Role.gameObject && aIStateControllerDeBug.enabled == true)
            {
                //TestFunction
                OnClick();
            }
        }

    }

    public void OnDrawGizmos()
    {
        if (theTarget != null)
        {
            Gizmos.DrawWireSphere(Role.transform.position, 5);//distance
        }
        Gizmos.DrawSphere(Role.transform.position, LookRadious);
        Gizmos.color = Color.red;
    }

    public void OnClick()
    {
        ShowAIStateInfo();
    }
    public void PlayAnim(string name,float begin)
    {
        m_anim.Play(name, 0,begin);
        //m_anim.Play(name, 0, 0f);
    }
    public void ShowAIStateInfo()
    {
        GameObject AIInfoObj = Role.gameObject.transform.GetChild(0).gameObject;
        AIInfoObj.SetActive(true);
    }
    public void HideAIStateController(GameObject InfoUI)
    {
        InfoUI.SetActive(false);
    }
}
