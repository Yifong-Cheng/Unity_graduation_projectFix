using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeMonsterControl : EnemyAIStateController {

    private void Awake()
    {
        SetValue();
    }


    public override void Intialize(AIGameManger _aIGameManger)
    {
        base.Intialize( _aIGameManger);
    }

    protected override void MotionIdle(float w_time)
    {
        base.MotionIdle(w_time);
    }

    public override void MotionWalk(GameObject target)
    {
        base.MotionWalk(target);
    }

    public override void MotionWander(CapsuleCollider t, float wd_time)
    {
        base.MotionWander(t, wd_time);
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void MotionDead()
    {
        base.MotionDead();
    }
}
