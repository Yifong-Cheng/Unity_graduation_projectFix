using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMonsterStateController : EnemyAIStateController
{

    //Monster NS

    private float height = 5f;
    private bool Isdisguuise, DigIn, DigOut;
    private delegate void DelegrateEvent();
    DelegrateEvent m_Delegrate;

    private Vector3 Pos;
    private float Digdistance = 1.5f;

    //private GameObject RenderGamObj;
    //private int runtime;

    //private GameObject AttackTutorial;

    protected override void FixUpdateAI()
    {
        StateAction();
        //InputProcess();
        CheckDoEscape();
        if(BeAttack)
        {
            if(!sneak)
            {
                sneak = true;
            }
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
            particileControl.Play(0, 5, Role.transform.position + Role.transform.forward + new Vector3(0,1,0));
            particileControl.Play(3, 5, Role.transform.position);
            BeAttack = false;
        }
        RolePos = Role.transform.position;
    }
    public override void Intialize(AIGameManger _aIGameManger)
    {
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Enemy";
        SetValue();
        m_anim.SetBool("Disguise", true);
        DigInGround();
        //RenderGamObj = ModelPrefab.transform.GetChild(5).gameObject;
        //m_Delegrate += MotionSneak;
        PfbTypeName = "NS";
        //AttackTutorial = Role.transform.Find("AttackTutorialCanvas").gameObject;
    }
    public override void DefendIntialize(AIGameManger _aIGameManger)
    {
        base.DefendIntialize(_aIGameManger);
        //EventManager.StartListening("DeadEnemy", DeadEnemy);
        m_objInfo.Type = "Enemy";
        SetValue();
        CanEscape = false;
        sneak = true;
        aIGameManger.StartCoroutineEvent(DigOutGround(.5f));
        //RenderGamObj = ModelPrefab.transform.GetChild(5).gameObject;
        PfbTypeName = "NS";
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

    protected override void MotionIdle(float w_time)
    {
        if (!sneak)
        {
            //m_Delegrate();
            MotionSneak();
        }
        else if (sneak)
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("SneakAttack"))
            {
                //controller.Model.transform.Rotate(0, 60, 0);
                aIGameManger.StartCoroutineEvent(DigOutGround(.5f));
                m_anim.SetBool("Disguise", false);
                m_anim.SetBool("Sneak", false);
                ModelPrefab.transform.rotation = Quaternion.EulerAngles(0, 180, 0);
                if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    ModelPrefab.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
                }
            }
            if (CheckEnemy())
            {
                if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, theTarget))
                {
                    Debug.Log("Enemy IS In AttackRange");
                    m_aiState = EnemyAIState.ATTACK;
                }
                else
                {
                    Debug.Log("SeeEnemy Start Chase");
                    m_aiState = EnemyAIState.Chase;
                }

            }
            currentWaitTime += Time.deltaTime;

            if (currentWaitTime > w_time)
            {
                Debug.Log("Start Wander");
                //DigOutGround();
                m_aiState = EnemyAIState.Wander;
            }
        }

        if (BeAttack)
        {
            aIGameManger.StartCoroutineEvent(DigOutGround(.5f));
            BeAttack = false;
            m_anim.SetBool("Disguise", false);
            m_anim.SetBool("Sneak", false);
            
            if (theTarget != null)
            {
                Debug.Log("Beattack  attacker's name = " + theTarget.name);
            }
            else
            {
                Debug.Log("Target is dead");
            }

            m_aiState = EnemyAIState.ATTACK;
        }


    }
    public override void MotionWalk(GameObject target)
    {
        base.MotionWalk(target);
    }

    public override void MotionDead()
    {
        
        MyTutorial.Instance.FirstillsNS();
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        //aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
        base.MotionDead();

        //audioController.PlaySound(1);
    }

    public override void MotionDefend(GameObject target)
    {
        m_anim.SetBool("Attack", false);
        m_anim.SetBool("Disguise", false);
        m_anim.SetBool("Sneak", false);
        base.MotionDefend(target);
    }

    public override void CheckDoEscape()
    {
        if (CanEscape)
        {

            if (p_health.currenthp <= 5f)
            {
                Debug.Log(Role.name + "start escape");
                m_anim.SetBool("Attack", false);
                m_anim.SetBool("Disguise", false);
                m_anim.SetBool("Sneak", false);
                aIGameManger.StartCoroutineEvent(DigOutGround(.5f));
                CanEscape = false;
                m_anim.SetBool("Walk", true);
                m_aiState = EnemyAIState.Escape;
                //MotionEscape(theTarget);
            }
        }
    }

    public override void MotionAttack(GameObject target)
    {
        base.MotionAttack(target);

        //Vector3 toTarget = target.transform.position - Role.transform.position;
        //float distance = toTarget.magnitude;
        //m_anim.SetFloat("Walk", distance - StopDistance);

        //if(Role.transform.position.y<theTarget.transform.position.y)
        //{
        //    Role.transform.position += new Vector3(0, 3*Time.deltaTime, 0);
        //}
        //else
        //{
        //    Role.transform.position = new Vector3(Role.transform.position.x, theTarget.transform.position.y, Role.transform.position.z);
        //}

        //if (distance < StopDistance && currentTime > AttackTime)
        //{
        //    //transform.forward = target.transform.forward;
        //    Attack();
        //}
        //else if (distance < StopDistance && currentTime < AttackTime)
        //{
        //    currentTime += Time.deltaTime;
        //    m_anim.SetBool("Attack", false);
        //    audioController.StopSound(0);
        //}

    }
    public override void Attack()
    {
        audioController.PlaySound(0);
        //m_anim.SetBool("Attack", true);
        //this.transform.position = new Vector3(this.transform.position.x, theTarget.transform.position.y, this.transform.position.z);
        base.Attack();
        //currentTime = 0;
        //
        //audioController.PlaySound(0);
    }

    private void MotionDisduise()
    {

    }

    //public void DigOutGround()
    //{
    //    //ModelPrefab.transform.position = Role.transform.position;
    //    particileControl.Play(2, 5, Role.transform.position);
    //    ModelPrefab.transform.localPosition = Vector3.zero;
    //}
    public IEnumerator DigOutGround(float delaytime)
    {
        sneak = true;
        //ModelPrefab.transform.position = Role.transform.position;
        particileControl.Play(2, 5, Role.transform.position);
        yield return new WaitForSeconds(delaytime*2);
        ModelPrefab.transform.localPosition = Vector3.zero;
    }

    public void DigInGround()
    {
        //ModelPrefab.transform.position = new Vector3(Role.transform.position.x, Role.transform.position.y - Digdistance, Role.transform.position.z);
        ModelPrefab.transform.localPosition = Vector3.zero + new Vector3(0,-Digdistance,0);
    }

    private void MotionSneak()
    {
        if (CheckSneakRange())
        {
            aIGameManger.StartCoroutineEvent( DigOutGround(.5f));
            sneak = true;
            //AttackTutorial.SetActive(true);
            if (theTarget.CompareTag("Player"))
            {
                Attack();
            }
            else
            {
                //theTarget.GetComponent<PlayerHealth>().TakeDamage(3, 100, this,proprity);
                theTarget.GetComponent<PlayerHealth>().TakeDamage(3, this, proprity);
            }
            m_anim.SetBool("Sneak", true);
            m_anim.SetBool("Disguise", false);
            m_anim.SetBool("Sneak", false);
            //m_Delegrate -= MotionSneak;
        }
        else
        {
            if (!sneak)
            {

            }
            //AttackTutorial.SetActive(false);
        }
    }

    private bool CheckSneakRange()
    {
        if (CheckEnemy() && !sneak)
        {

            return true;
        }
        else
        {
            return false;
        }

    }

    //private IEnumerator DissloveShader(float waitTime)
    //{
    //    float buff = .1f;
    //    if(runtime<1)
    //    {
    //        yield return new WaitForSeconds(waitTime*2);
    //    }
    //    if (RenderGamObj.GetComponent<Renderer>().materials[0].GetFloat("_DissolveValue") < 1)
    //    {
    //        runtime += 1;
    //        RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        yield return new WaitForSeconds(waitTime);
    //        aIGameManger.StartCoroutineEvent(DissloveShader(waitTime));
    //        //runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime); runtime += 1;
    //        //RenderGamObj.GetComponent<Renderer>().materials[0].SetFloat("_DissolveValue", buff * runtime);
    //        //yield return new WaitForSeconds(waitTime);
    //    }
    //    else
    //    {
    //        yield return null;
    //    }
    //}

    //public IEnumerator HitShader(float waitTime)
    //{
    //    RenderGamObj.GetComponent<Renderer>().materials[1].SetFloat("_Outline", 1);
    //    yield return new WaitForSeconds(waitTime);
    //    RenderGamObj.GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
    //}

}
