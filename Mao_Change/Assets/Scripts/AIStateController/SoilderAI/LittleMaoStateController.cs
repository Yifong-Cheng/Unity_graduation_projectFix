using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMaoStateController : SoilderAIStateController
{

    /// <summary>
    /// LittleSoilder_S0
    /// </summary>
    /// <param name="_aIGameManger"></param>

    //protected override void FixUpdateAI()
    //{
    //    InputProcess();
    //    StateAction();
    //}
    //AudioSource Audio;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        proprity = GentleProprity;
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 0;
        SetValue();
        //Audio = Role.GetComponent<AudioSource>();
        audioController = Role.GetComponent<AudioController>();
        //audioController.PlaySound(0,1);
    }

    public override void SetValue()
    {
        base.SetValue();
        StopDistance = nav.stoppingDistance;
        AttackTime = 2f;
        AttackDistance = 5f;
        LookRadious = 8f;

        //WaitTime = Random.Range(3, 8);
        WaitTime = Random.Range(15f,30f);
        PfbTypeName = "S0";
    }

    protected override void FixUpdateAI()
    {
        base.FixUpdateAI();
        if(BeAttack)
        {
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
        }
    }

    protected override void MotionIdle(float w_time)
    {
        base.MotionIdle(w_time);
    }

    public override void MotionAttack(GameObject target)
    {
        base.MotionAttack(target);
    }

    public override void MotionDead()
    {
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        //aIGameManger.StartCoroutineEvent(DissloveShader(.3f));
        base.MotionDead();

    }

    public override void MotionWander(CapsuleCollider t)
    {
        base.MotionWander(t);
    }
    public override void Attack()
    {
        //m_anim.Play("Attack");
        //Audio.Play();
        audioController.PlayAudioClip(1, 2);
        //audioController.PlaySound(1);
        base.Attack();

    }


}
