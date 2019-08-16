using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAiStateController : SoilderAIStateController {

    public override void Intialize(AIGameManger _aIGameManger)
    {
        //base.Intialize( _aIGameManger);
        //aIGameManger = _aIGameManger;
        ////SetObjInfo();
        //player = GameObject.Find("Player");
        //audioController = Role.GetComponent<AudioController>();
        //nav = Role.GetComponent<NavMeshAgent>();
        //selectable = Role.GetComponent<Selectable>();
        //if (selectable != null)
        //{
        //    selectable.ai = this;
        //}
        //targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        //targetPos.transform.parent = null;
        //ResetTargetPos(targetPos);
        //ModelPrefab = Role.transform.GetChild(1).gameObject;
        //particileControl = Role.GetComponent<ParticileControl>();
        ////m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        //Debug.Log("ModelName" + ModelPrefab.name);
        //m_anim = ModelPrefab.GetComponent<Animator>();
        //p_health = Role.GetComponent<PlayerHealth>();
        //p_health.stateController = this;
        ////m_aiState = AIState.IDLE;
        ////layersChecked = LayerMask.NameToLayer("Enemy");
        //SetValue();
    }

    protected override void FixUpdateAI()
    {
        //base.FixUpdateAI();
        //RolePos = Role.transform.position;
        //StateAction();
        ////InputProcess();
        //AnimationFinishResetPosition();
        //CallHelp();
    }
}
