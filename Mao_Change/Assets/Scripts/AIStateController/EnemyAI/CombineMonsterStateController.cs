using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMonsterStateController : CombineEnemyAIStateController {

    public GameObject Snap;
    public override void Intialize(AIGameManger _aIGameManger)
    {
        base.Intialize(_aIGameManger);
        ChangePfbSln = "Enemy/" + "CA";
        m_objInfo.Type = "Enemy";
        SetValue();
        Snap = ModelPrefab.transform.GetChild(0).gameObject;
        PfbTypeName = "CB";
        
    }
    public override void DefendIntialize(AIGameManger _aIGameManger)
    {
        base.DefendIntialize(_aIGameManger);
        ChangePfbSln = "Enemy/" + "CA";
        m_objInfo.Type = "Enemy";
        SetValue();
        Snap = ModelPrefab.transform.GetChild(0).gameObject;
        CanEscape = false;
        PfbTypeName = "CB";
        //RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
    }
    protected override void FixUpdateAI()
    {
        base.FixUpdateAI();
        RolePos = Role.transform.position;
        if (BeAttack)
        {
            particileControl.Play(2, 5, Role.transform.position);
            particileControl.Play(2, 5, Role.transform.position);
            particileControl.Play(2, 5, Role.transform.position);
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
        }
    }

    public override void SetValue()
    {
        base.SetValue();
        AttackTime = 1.3f;
        LookRadious = 5f;
        AttackDistance = 10f;
        ChaseDistance = 20f;
        wanderTime = Random.Range(1, 3);
        WaitTime = Random.Range(3, 8);
    }

    public override void Attack()
    {
        audioController.PlaySound(0);
        aIGameManger.StartCoroutineEvent(SnapClose(.8f));
        base.Attack();
        
    }
    private IEnumerator SnapClose(float waittime)
    {

        //Snap.GetComponent<IKSnapTest>().otherObj = theTarget;
        if(Snap!=null)
        {
            Snap.SetActive(true);
        }
        
        yield return new WaitForSeconds(waittime);
        if(Snap!=null)
        {
            Snap.SetActive(false);
        }
        
    }
    public override void ChangeModel()
    {
        base.ChangeModel();
        //Snap = ModelPrefab.transform.GetChild(0).transform.Find("Snap").gameObject;
        Snap = ModelPrefab.transform.GetChild(0).gameObject;
        Snap.SetActive(false);
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
    }

    public override void MotionDead()
    {
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
        //base.MotionDead();
        nav.Stop();
        m_objInfo.IsDead = true;
        particileControl.Play(1, 2F, Role.transform.position + new Vector3(0, Role.GetComponent<CharacterController>().height + 2, 0));
        //m_anim.SetBool("IsDead", true);
        m_anim.SetBool("Walk", false);
        //PlayAnim("Dead", -1.0f);
        m_anim.SetBool("IsDead", true);
        if (DropProp)
        {
            if (IsGroup)
            {
                CreatCrystal(5);
            }
            else
            {
                CreatCrystal(1);
            }
        }
        MyTutorial.Instance.FirstillsCB();
        aIGameManger.ReleaseCombineEnemyAI(this, 6);
        //audioController.PlaySound(1);
    }

    protected override void CheckArrive()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Vector3.Distance(neighbors[i].Role.transform.position, Role.transform.position) < 10 && !neighbors[i].IsArrive)
            {
                neighbors[i].IsArrive = true;
                arrivedNum += 1;
            }
        }
        if (arrivedNum > 4)
        {
            particileControl.Play(3, 5, Role.transform.position);
            m_aiState = CombineEnemyState.StartCOMBINE;
        }
        
    }

    
}
