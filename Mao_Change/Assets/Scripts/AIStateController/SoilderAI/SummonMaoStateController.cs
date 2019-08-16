using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMaoStateController : SoilderAIStateController
{

    /// <summary>
    /// Summon S1
    /// </summary>

    //碰撞體陣列
    private Collider[] colliders;
    //計時器
    private float timer = 0;
    //鄰居列表
    public List<GameObject> neighbors;
    //無須每畫面進行檢測，該變數設定檢測時間的間隔
    public float checkInterval = 1.2f;
    //設定鄰域半徑
    public float detectRadius = 200f;
    //設定檢測哪一層的遊戲物件
    public LayerMask layersChecked;

    public GameObject SignRange;
    Vector3 pos;

    private int currentSummonNum = 0;
    private int SummonNum = 5;

    private enum SummonState
    {
        Null,
        IDLE,
        DEAD,
    }
    private SummonState m_SummonState = SummonState.Null;

    protected override void FixUpdateAI()
    {
        Role.transform.position = pos;
        StateAction();
        AnimationFinishResetPosition();
    }

    public override void Intialize(AIGameManger _aIGameManger)
    {
        neighbors = new List<GameObject>();
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 1;
        wanderTime = Random.Range(1, 2);
        WaitTime = Random.Range(5, 10);
        pos = Role.transform.position;
        PfbTypeName = "S1";
        m_SummonState = SummonState.Null;
    }

    protected override void StateAction()
    {
        switch (m_SummonState)
        {
            case (SummonState.Null):
                {
                    m_SummonState = SummonState.IDLE;

                }
                break;
            case (SummonState.IDLE):
                {
                    MotionIdle(WaitTime);
                }
                break;

            case (SummonState.DEAD):
                {
                    MotionDead();
                }
                break;

            default:

                break;
        }
    }

    protected override void MotionIdle(float w_time)
    {
        timer += Time.deltaTime;
        if (timer > checkInterval)
        {
            //audioController.PlaySound(0,.1f);
            particileControl.Play(2, 3F, Role.transform.position);
            aIGameManger.SummonSoilderAI(this, Role, detectRadius);
            currentSummonNum += 1;
            timer = 0;
        }

        if (currentSummonNum >= SummonNum)
        {
            m_SummonState = SummonState.DEAD;
        }

    }

    public override void MotionDead()
    {
        audioController.PlaySound(1);
        //aIGameManger.StartCoroutineEvent(DissloveShader(.3f));
        aIGameManger.SummonEnd(this, Role);
        base.MotionDead();
    }
}
