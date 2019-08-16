using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeMonsterStateController : EnemyAIStateController
{
    public bool TimeToReturn;
    private float currentReturnTime;
    private float ReturnTime = 35;

    public GroupEnemyAIStateController House;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        base.Intialize(_aIGameManger);
    }

    public override void DefendIntialize(AIGameManger _aIGameManger)
    {
        //base.DefendIntialize(_aIGameManger);
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        //ResetTargetPos(targetPos);
        //targetPos.transform.position = House.Tower.transform.position;
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        //m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        CanEscape = false;
        SetValue();
        Tower = GameObject.FindObjectOfType<StageChallange>().gameObject;
        particileControl = Role.GetComponent<ParticileControl>();
        targetPos.transform.position = Tower.transform.position;
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
        DropProp = false;
        EventManager.StartListening("DeadEnemy", DeadEnemy);
    }

    public override void SetValue()
    {
        Debug.Log(Role.name + "'s Stopdis = " + nav.stoppingDistance);
        AttackTime = 3f;
        AttackDistance = nav.stoppingDistance + 5f;
        ChaseDistance = nav.stoppingDistance * 5;
        LookRadious = nav.stoppingDistance + 3f;
        wanderTime = 0.5f;
        WaitTime = 15;
        currentWaitTime = 10;
        StopDistance = nav.stoppingDistance;
    }

    protected override void FixUpdateAI()
    {
        base.FixUpdateAI();
        CheckReturnHouse();
        if (BeAttack)
        {
            particileControl.Play(2, 5, Role.transform.position);
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
        }
    }

    protected override void StateAction()
    {
        base.StateAction();
    }

    public void DoChaseTarget(GameObject target)
    {
        targetPos.transform.position = target.transform.position;
        nav.SetDestination(targetPos.transform.position);
    }

    public override void MotionDead()
    {
        if(House!=null)
        {
            House.RemoveFromHouse(this);
        }
        
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        //aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
        MyTutorial.Instance.FirstillsBEEM();
        base.MotionDead();
        
    }

    protected override void MotionIdle(float w_time)
    {
        base.MotionIdle(w_time);
        RunReturnTime();
    }

    public override void MotionWander(CapsuleCollider t, float wd_time)
    {
        base.MotionWander(t, wd_time);

        RunReturnTime();
    }

    private void RunReturnTime()
    {
        currentReturnTime += Time.deltaTime;
        if (currentReturnTime > ReturnTime)
        {
            TimeToReturn = true;
        }
    }

    public void CheckReturnHouse()
    {
        if(House!=null)
        {
            if (TimeToReturn)
            {
                nav.SetDestination(House.Role.transform.position);
                if (BeAttack)
                {
                    TimeToReturn = false;
                    currentReturnTime = 0;
                }
                if (Vector3.Distance(Role.transform.position, House.Role.transform.position) < 5)
                {
                    aIGameManger.ReleaseEnemyAI(this, 1);
                }
            }
        }
    }

    public override void Attack()
    {
        audioController.PlaySound(0);
        base.Attack();
    }
}
