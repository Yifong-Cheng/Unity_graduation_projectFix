using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaoStateController : SoilderAIStateController
{

    /// <summary>
    /// Tank S4
    /// </summary>
    AudioSource Audio;
    public override void Intialize(AIGameManger _aIGameManger)
    {
        proprity = HeavyProprity;
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 4;
        SetValue();
        Audio = Role.GetComponent<AudioSource>();
    }

    protected override void FixUpdateAI()
    {
        RolePos = Role.transform.position;
        StateAction();
        AnimationFinishResetPosition();
        CallHelp();

        if(BeAttack)
        {
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
        }
    }

    public override void SetValue()
    {
        Debug.Log("stopdis = " + nav.stoppingDistance);
        StopDistance = nav.stoppingDistance;
        AttackTime = 3f;
        WaitTime = Random.Range(20, 30);
        AttackDistance = nav.stoppingDistance + 5f;
        ChaseDistance = nav.stoppingDistance * 5;
        LookRadious = nav.stoppingDistance + 3f;
        PfbTypeName = "S4";
    }

    protected override void MotionIdle(float w_time)
    {
        //base.MotionIdle(w_time);
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget,theTarget.transform.position))
            {
                Debug.Log("Enemy IS In AttackRange");
                CallParner();
                m_aiState = AIState.ATTACK;
            }
            else
            {
                Debug.Log("SeeEnemy Start Chase");
                m_aiState = AIState.Chase;
            }

        }
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime > w_time)
        {
            Debug.Log("Start Wander");
            m_aiState = AIState.Wander;
        }
        if (!CheckAnimationIsIdle())
        {
            m_anim.SetBool("Walk", false);
        }
        else
        {
            Debug.Log("play another idle");
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Debug.Log("start random");
                int animrand = Random.Range(0, 3);
                Debug.Log("num is " + animrand);
                switch (animrand)
                {
                    case 0:
                        PlayAnim("Idle_0",-1.0f);
                        break;

                    case 1:
                        PlayAnim("Idle_1",-1.0f);
                        break;

                    default:
                        PlayAnim("Idle",-1.0f);
                        break;
                }
            }

        }
    }
    public override void MotionWander(CapsuleCollider t)
    {
        base.MotionWander(t);
    }
    public override void MotionWalk(GameObject target)
    {
        base.MotionWalk(target);
    }
    public override void MotionAttack(GameObject target)
    {
        base.MotionAttack(target);
    }
    public override void Attack()
    {
        Vector3 pos = new Vector3(Role.transform.position.x, 0.5f, Role.transform.position.z) + Role.transform.forward*3;
        particileControl.Play(2, 3F, pos);
        Audio.Play();
        base.Attack();
        //audioController.PlaySound(1);
    }

    public override void MotionQueue()
    {
        base.MotionQueue();

    }

    public override void MotionDead()
    {
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        //aIGameManger.StartCoroutineEvent(DissloveShader(.3f));
        base.MotionDead();
        //audioController.PlaySound(2);
    }
}
