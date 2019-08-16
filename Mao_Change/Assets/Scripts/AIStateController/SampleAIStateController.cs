using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleAIStateController : MonoBehaviour {

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
    public float wanderTime;
    [Header("徘徊等待時間")]
    public float WaitTime;

    protected float currentAvoidTime = 0;
    protected float AvoidTime = 1.8f;

    //public Transform[] SeeView = new Transform[3];
    protected float LookRadious;
    protected float AttackDistance;
    protected float ChaseDistance;
    public GameObject Role;
    public string msg;
    public CapsuleCollider targetPos;
    public Transform P;
    protected NavMeshAgent nav;
    public Vector3 RolePos;
    public string PfbTypeName = "";
    protected ParticileControl particileControl;

    public bool BeAttack;

    public enum AIState
    {
        Null,
        IDLE,
        WALK,
        Wander,
        DEAD

    }

    private float walkCurrentTime;
    private float walkFixTime = 10f;
    public AIState m_aiState = AIState.Null;

    // Use this for initialization
    void Start () {
        Intialize();
    }
	// Update is called once per frame
	void Update () {
        FixUpdateAI();
	}

    public  void Intialize()
    {
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        particileControl = Role.GetComponent<ParticileControl>();
        //m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        Debug.Log("ModelName" + ModelPrefab.name);
        m_anim = ModelPrefab.GetComponent<Animator>();
        m_aiState = AIState.IDLE;
        //layersChecked = LayerMask.NameToLayer("Enemy");
        SetValue();
        P = GameObject.Find("RunPos").transform;
    }

    protected  void FixUpdateAI()
    {
        RolePos = Role.transform.position;
        StateAction();
        AnimationFinishResetPosition();
    }

    protected  void StateAction()
    {
        switch (m_aiState)
        {
            case AIState.Null:
                m_aiState = AIState.IDLE;
                break;
            case AIState.IDLE:
                {
                    //1
                    MotionIdle(WaitTime);
                }

                break;

            case AIState.WALK:
                {
                    MotionWalk(P);
                }
                break;

            case AIState.Wander:
                {
                    MotionWander(targetPos, wanderTime);
                }

                break;

            case AIState.DEAD:
                {
                    //1
                    MotionDead();
                }

                break;

            default:
                break;
        }
    }
    #region 加元件

    public virtual void SetValue()
    {
        Debug.Log(Role.name + "'s Stopdis = " + nav.stoppingDistance);
        AttackTime = 3f;
        AttackDistance = nav.stoppingDistance + 5f;
        ChaseDistance = nav.stoppingDistance * 5;
        LookRadious = nav.stoppingDistance + 3f;
        wanderTime = 0.5f;
        WaitTime = Random.Range(10,15);
        StopDistance = nav.stoppingDistance;
    }

    #endregion
    public void PlayAnim(string name, float begin)
    {
        m_anim.Play(name, 0, begin);
        //m_anim.Play(name, 0, 0f);
    }


    #region AIMotion(AI動作)

    protected void DoAnimation(string NextPlayAnimName, bool b, string LastPlayAnimName)
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName(LastPlayAnimName))
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                m_anim.SetBool(NextPlayAnimName, b);
            }
        }
    }

    protected void AnimationFinishResetPosition()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ModelPrefab.transform.localPosition = Vector3.zero;
            ModelPrefab.transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
        }
    }

    protected virtual void MotionIdle(float w_time)
    {
        //currentWaitTime += Time.deltaTime;

        //if (currentWaitTime > w_time)
        //{
        //    Debug.Log("Start Wander");
        //    m_aiState = AIState.Wander;
        //}
        //if (!CheckAnimationIsIdle())
        //{
        //    m_anim.SetBool("Walk", false);

        //}
        //else
        //{
        //    //Debug.Log("play another idle");
        //    if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        //    {
        //        //Debug.Log("start random");
        //        //int animrand = Random.Range(0, 4);
        //        //Debug.Log("num is " + animrand);
        //        //switch (animrand)
        //        //{
        //        //    case 0:
        //        //        PlayAnim("Idle_0");
        //        //        break;

        //        //    case 1:
        //        //        PlayAnim("Idle_1");
        //        //        break;

        //        //    case 2:
        //        //        PlayAnim("Idle_2");
        //        //        break;
        //        //    case 3:
        //        //        PlayAnim("Idle");
        //        //        break;

        //        //    default:
        //        //        PlayAnim("Idle");
        //        //        break;
        //        //}
        //        PlayAnim("Idle", -1.0f);
        //    }

        //}

        if(Vector3.Distance(P.position,Role.transform.position)>StopDistance*5)
        {
            m_aiState = AIState.WALK;
        }

        if (!CheckAnimationIsIdle())
        {
            m_anim.SetBool("Walk", false);

        }
        else
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                PlayAnim("Idle", -1.0f);
            }

        }
    }
    protected bool CheckAnimationIsIdle()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("BeAttack"))
            return true;
        else
            return false;
    }

    //徘徊
    public virtual void MotionWander(CapsuleCollider t, float wd_time)
    {
        currentWaitTime = 0;
        nav.SetDestination(t.gameObject.transform.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) <= StopDistance)
        {
            ResetTargetPos(targetPos);
            m_aiState = AIState.IDLE;
        }
        m_anim.SetBool("Walk", true);
        //PlayAnim("Walk");
    }

    public virtual void MotionWalk(Transform p)
    {
        nav.SetDestination(p.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) <= StopDistance*3)
        {
            //ResetTargetPos(targetPos);
            m_anim.SetBool("Walk", false);
            m_aiState = AIState.IDLE;
        }
        m_anim.SetBool("Walk", true);
    }

    private void ResetTargetPos(CapsuleCollider t)
    {
        Vector3 center = t.transform.position;

        float range_x = Random.Range(-10, 10);
        float range_z = Random.Range(-10, 10);

        Vector3 newTargetPos = new Vector3(center.x + range_x, center.y, center.z + range_z);

        targetPos.transform.position = newTargetPos;
    }
    private void ResetTargetPosFar(CapsuleCollider t)
    {
        Vector3 center = t.transform.position;

        float range_x = Random.Range(-20, 20);
        float range_z = Random.Range(-20, 20);

        Vector3 newTargetPos = new Vector3(center.x + range_x, center.y, center.z + range_z);

        targetPos.transform.position = newTargetPos;
    }

    public virtual void MotionDead()
    {
        nav.Stop();
        particileControl.Play(1, 2F, Role.transform.position);
        PlayAnim("Dead", -1.0f);
    }

    #endregion

}
