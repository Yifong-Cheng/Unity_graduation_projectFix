using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMaoStateController : SoilderAIStateController
{
    /// <summary>
    /// FLYMao 
    /// </summary>

    public GameObject helix;
    public float hight = 3;

    protected override void FixUpdateAI()
    {
        InputProcess();
        StateAction();
    }


    public override void Intialize(AIGameManger _aIGameManger)
    {
        SetValue();
        base.Intialize( _aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 2;
        StopDistance = 10;
        AttackTime = 1.5f;
        hight = 3;

        wanderTime = Random.Range(1, 2);
        WaitTime = Random.Range(5, 10);
    }

    protected override void MotionIdle(float w_time)
    {

        base.MotionIdle(w_time);
        
        if(m_anim!=null)
        {
            m_anim.SetBool("Walk", true);
        }
        //
        if(audioController!=null)
        {
            audioController.StopSound(0);
        }
        //
    }

    public override void MotionWander(CapsuleCollider t)
    {
        base.MotionWander(t);
    }

    public override void MotionWalk(GameObject target)
    {
       
        if(Role.transform.position.y<= (hight+ target.transform.position.y))
        {
            Role.transform.position += new Vector3(0, 5f, 0) * Time.deltaTime;
        }
        else
        {
            base.MotionWalk(target);
        }
        
        MotionFly();

        //audioController.PlaySound(0);
    }

    public override void MotionFly()
    {
        //base.MotionFly();
        helix.transform.Rotate(0, 50, 0);
        //m_anim.SetFloat("FlySpeed", m_vehicle.velocity.magnitude);
    }

    public override void MotionAttack(GameObject target)
    {
        base.MotionAttack(target);


        Vector3 toTarget = target.transform.position - Role.transform.position;
        float distance = toTarget.magnitude;
        m_anim.SetFloat("FlySpeed", distance - StopDistance);

        if (distance < StopDistance && currentTime > AttackTime)
        {
            //transform.forward = target.transform.forward;
            Attack();
        }
        else if (distance < StopDistance && currentTime < AttackTime)
        {
            currentTime += Time.deltaTime;
            m_anim.SetBool("Attack", false);
            audioController.StopSound(1);
        }
    }

    public override void Attack()
    {
        //m_anim.Play("Attack");

        m_anim.SetBool("Attack", true);
        currentTime = 0;
        audioController.PlaySound(1);
    }

    public override void MotionDead()
    {
        //anim
        m_anim.SetBool("Dead", m_objInfo.IsDead);
        audioController.PlaySound(2);
        //drop
        Role.transform.position -= new Vector3(0, Role.transform.position.y, 0) * Time.deltaTime * 10;
        //destory
        base.MotionDead();
       
    }

}
