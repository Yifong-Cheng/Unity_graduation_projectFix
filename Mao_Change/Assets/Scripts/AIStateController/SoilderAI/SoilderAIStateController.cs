using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoilderAIStateController : AIStateController
{
    public enum AIState
    {
        Null,
        IDLE,
        WALK,
        Chase,
        Wander,
        ATTACK,
        DEFEND,
        FLY,
        Avoid,
        DEAD

    }
    private float walkCurrentTime;
    private float walkFixTime = 10f;
    public AIState m_aiState = AIState.Null;
    public Selectable selectable;
    
    protected Renderer[] RenderGamObjs;
    protected int runtime;
    public override void Intialize(AIGameManger _aIGameManger)
    {
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        selectable = Role.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectable.ai = this;
        }
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        particileControl = Role.GetComponent<ParticileControl>();
        Debug.Log("ModelName" + ModelPrefab.name);
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.Proprity = proprity;
        m_aiState = AIState.IDLE;
        SetValue();
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
    }

    protected override void FixUpdateAI()
    {
        RolePos = Role.transform.position;
        StateAction();
        AnimationFinishResetPosition();
        CallHelp();
    }

    protected override void StateAction()
    {
        if (m_objInfo.IsDead == true)
        {
            m_aiState = AIState.DEAD;
        }
        m_objInfo.State = m_aiState.ToString();

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

            case AIState.Wander:
                {
                    MotionWander(targetPos);
                }

                break;

            case AIState.WALK:
                {
                   //1
                    MotionWalk(targetPos.gameObject);
                }


                break;

            case AIState.Chase:
                {
                    MotionChase(theTarget);
                }

                break;

            case AIState.FLY:
                {
                    //1
                    MotionFly();
                }


                break;

            case AIState.ATTACK:
                {
                    //1
                    MotionAttack(theTarget);
                }


                break;

            case AIState.DEFEND:
                {
                    //1
                    MotionDefend();
                }

                break;

            case AIState.Avoid:
                {
                    if (currentAvoidTime < AvoidTime)
                    {
                        MotionAvoid(theTarget);
                    }
                    else
                    {
                        currentAvoidTime = 0;
                        m_aiState = AIState.IDLE;
                    }
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

    #region AIMotion(AI動作)

    protected virtual bool CheckEnemy()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Role.transform.position, LookRadious, Role.transform.forward, out hit, LookRadious))
        {
            if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("CombineBefore")|| hit.collider.CompareTag("CombineAfter"))
            {
                Debug.Log("SeeEnemy" + ": " + hit.transform.name);
                theTarget = hit.transform.gameObject;
                return true;
            }

        }
        return false;
    }

    //protected virtual bool CheckEnemy()
    //{
    //    Checktimer += Time.deltaTime;
    //    if (Checktimer > checkInterval)
    //    {
    //        //neighbors.Clear();

    //        Aroundcolliders = Physics.OverlapSphere(Role.transform.position, detectRadius, layersChecked);
    //        for (int i = 0; i < Aroundcolliders.Length; i++)
    //        {
    //            if (Aroundcolliders[i].CompareTag("Enemy"))
    //            {
    //                //neighbors.Add(Aroundcolliders[i].gameObject);
    //                Debug.Log("SeeEnemy" + ": " + Aroundcolliders[i].transform.name);
    //                theTarget = Aroundcolliders[i].transform.gameObject;
    //                return true;
    //            }

    //        }
    //        Checktimer = 0;
    //    }
    //    return false;
    //}

    protected virtual bool EnemyIsAround(float dis, GameObject target,Vector3 tagetPos)
    {
        if (target != null)
        {

            return Vector3.Distance(Role.transform.position, tagetPos) < dis ? true : false;
        }
        else
            return false;

    }

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
        if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=1)
        {
            ModelPrefab.transform.localPosition = Vector3.zero;
            ModelPrefab.transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
        }
    }

    protected virtual void CallHelp()
    {
        if (p_health.currenthp < 4)
        {
            //Debug.Log("NeedHelp");
            aIGameManger.SummonSoilderAIHelp(this, Role, 30);
        }
    }
    protected virtual void CallParner()
    {
        aIGameManger.SummonSoilderAIHelp(this, Role, 30);
    }

    protected virtual void MotionIdle(float w_time)
    {
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget,theTarget.transform.position))
            {
                //Debug.Log("Enemy IS In AttackRange");
                CallParner();
                m_aiState = AIState.ATTACK;
            }
            else
            {
                //Debug.Log("SeeEnemy Start Chase");
                m_aiState = AIState.Chase;
            }

        }
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime > w_time)
        {
            //Debug.Log("Start Wander");
            m_aiState = AIState.Wander;
        }
        if (!CheckAnimationIsIdle())
        {
            m_anim.SetBool("Walk", false);
            //m_anim.SetBool("Attack", false);
            //PlayAnim("Idle");
        }
        else
        {
            //Debug.Log("play another idle");
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //Debug.Log("start random");
                //int animrand = Random.Range(0, 4);
                //Debug.Log("num is " + animrand);
                //switch (animrand)
                //{
                //    case 0:
                //        PlayAnim("Idle_0");
                //        break;

                //    case 1:
                //        PlayAnim("Idle_1");
                //        break;

                //    case 2:
                //        PlayAnim("Idle_2");
                //        break;
                //    case 3:
                //        PlayAnim("Idle");
                //        break;

                //    default:
                //        PlayAnim("Idle");
                //        break;
                //}
                PlayAnim("Idle",-1.0f);
            }
                
        }

        if (BeAttack)
        {
            BeAttack = false;
            if (theTarget != null)
            {
                //Debug.Log("Beattack  attacker's name = " + theTarget.name);
            }
            else
            {
                //Debug.Log("Target is dead");
            }

            m_aiState = AIState.ATTACK;
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
    public virtual void MotionWander(CapsuleCollider t)
    {
        currentWaitTime = 0;
        //currentWanderTime += Time.deltaTime;
        //if (currentWanderTime > wd_time)
        //{
        //    currentWaitTime = 0;
        //    currentWanderTime = 0;

        //    m_aiState = AIState.IDLE;
        //}
        nav.SetDestination(t.gameObject.transform.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) <= StopDistance)
        {
            ResetTargetPos(targetPos);
            m_aiState = AIState.IDLE;
        }

        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget,theTarget.transform.position))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                //PlayAnim("Walk");
                CallParner();
                m_aiState = AIState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                m_aiState = AIState.Chase;
            }
        }
        m_anim.SetBool("Walk", true);
        //PlayAnim("Walk");
    }

    private void ResetTargetPos(CapsuleCollider t)
    {
        Vector3 center = t.transform.position;

        float range_x = Random.Range(-10, 10);
        float range_z = Random.Range(-10, 10);

        Vector3 newTargetPos = new Vector3(center.x + range_x, center.y, center.z + range_z);
        //Vector3 newTargetPos = GetSpawnPosition(15);

        targetPos.transform.position = newTargetPos;
    }

    Vector3 GetSpawnPosition(float spawnSize)
    {
        Vector3 spawnPosition = new Vector3();
        float startTime = Time.realtimeSinceStartup;
        bool test = false;
        while (test == false)
        {
            Vector2 spawnPositionRaw = Random.insideUnitCircle * spawnSize;
            spawnPosition = new Vector3(spawnPositionRaw.x, 0, spawnPositionRaw.y) + targetPos.transform.position;
            test = !Physics.CheckSphere(spawnPosition, 0.75f);
            if (Time.realtimeSinceStartup - startTime > 0.5f)
            {
                Debug.Log("Time out placing Minion!");
                return Vector3.zero;
            }
        }
        return spawnPosition;
    }

    private void ResetTargetPosFar(CapsuleCollider t)
    {
        Vector3 center = t.transform.position;

        float range_x = Random.Range(-20, 20);
        float range_z = Random.Range(-20, 20);

        Vector3 newTargetPos = new Vector3(center.x + range_x, center.y, center.z + range_z);
        //Vector3 newTargetPos = GetSpawnPosition(20);
        targetPos.transform.position = newTargetPos;
    }

    public virtual void MotionWalk(GameObject target)
    {
        //if(Vector3.Distance(RolePos,target.transform.position)<StopDistance)
        //{
        //    m_anim.SetBool("Walk", false);
        //    m_aiState = AIState.IDLE;
        //}
        //else
        //{

        //}

        //Vector3 Dir = target.transform.position - Role.transform.position;
        //Dir.y = 0;
        //Role.transform.rotation = Quaternion.LookRotation(Dir);
        walkCurrentTime += Time.deltaTime;
        if (walkCurrentTime > walkFixTime)
        {
            //Debug.LogWarning("walkstate to long !!");
            walkCurrentTime = 0;
            ResetTargetPosFar(targetPos);
            m_anim.SetBool("Walk", false);
            m_aiState = AIState.IDLE;
        }

        if (Vector3.Distance(target.transform.position, Role.transform.position) < StopDistance)
        {
            m_anim.SetBool("Walk", false);
            m_aiState = AIState.IDLE;
        }
        else
        {
            nav.SetDestination(target.transform.position);
            m_anim.SetBool("Walk", true);
            //PlayAnim("Walk");
        }
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget,theTarget.transform.position))
            {
                //Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                CallParner();
                m_aiState = AIState.ATTACK;
            }
            else
            {
                //Debug.Log("Wander See Enemy Start Chase");
                m_aiState = AIState.Chase;
            }
        }
    }

    public virtual void MotionChase(GameObject target)
    {
        if(target!=null)
        {
            if (EnemyIsAround(AttackDistance, theTarget,theTarget.transform.position))
            {
                //Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                CallParner();
                m_aiState = AIState.ATTACK;
            }

            if (Vector3.Distance(Role.transform.position, target.transform.position) > ChaseDistance)
            {
                //Debug.Log("Enemy TooFar");
                theTarget = null;
                ////m_anim.SetBool("Walk", false);
                m_aiState = AIState.IDLE;
            }
            else
            {
                m_anim.SetBool("Walk", true);
                //PlayAnim("Walk");
                Vector3 pos = target.transform.position;
                //Vector3 pos = target.transform.position - target.transform.forward * 2;
                nav.SetDestination(pos);
            }
        }
        else
        {
            //m_anim.SetBool("Attack", false);
            m_anim.SetBool("Walk", false);
            ResetTargetPosFar(targetPos);
            m_aiState = AIState.WALK;
        }
        
    }

    public virtual void MotionRun()
    {
    }

    public virtual void MotionAttack(GameObject target)
    {
        //target.GetComponent<ObjInfo>().IsDead == true
        if (target == null)
        {
            //Debug.Log("Enemy Is Dead");
            theTarget = null;
            //m_anim.SetBool("Attack", false);
            ResetTargetPos(targetPos);
            m_aiState = AIState.IDLE;
        }
        else
        {
            if (currentTime > AttackTime)
            {
                //Debug.Log("DoAttack 0.0");
                currentTime = -1.5f;
                Attack();
            }
            else if (currentTime < AttackTime)
            {
                if(BeAttack)
                {
                    currentTime = 0;
                }
                currentTime += Time.deltaTime;

                //m_anim.SetBool("Walk", true);
                //m_anim.SetBool("Attack", false);
                if (audioController != null)
                {
                    //audioController.StopSound(0);
                }
            }
            else if (currentTime < 0)
            {
                Vector3 backpos = Role.transform.position + target.transform.forward * 4.5f;
                nav.SetDestination(backpos);
                m_anim.SetBool("Walk", true);
                //m_anim.SetBool("Attack", false);
                PlayAnim("Walk",0);
            }

            Vector3 Dir = target.transform.position - Role.transform.position;
            Dir.y = 0;
            Role.transform.rotation = Quaternion.LookRotation(Dir);

            if (!EnemyIsAround(AttackDistance, target,theTarget.transform.position))
            {
                //Debug.Log("Start Chase Target");
                //m_anim.SetBool("Attack", false);
                m_aiState = AIState.Chase;
            }
        }
    }

    public virtual void MotionAvoid(GameObject target)
    {
        currentAvoidTime += Time.deltaTime;
    }

    public virtual void MotionFly()
    { }

    public virtual void MotionDefend()
    { }

    public virtual void MotionDead()
    {
        /*
        Destroy(gameObject, 3f);
        */
        //GetComponent<PlayerHealth>().currenthp -= 100;
        nav.isStopped = true;
        m_objInfo.IsDead = true;
        particileControl.Play(1, 2F, Role.transform.position + new Vector3(0,Role.GetComponent<CharacterController>().height+2,0));
        //m_anim.SetBool("IsDead", true);
        PlayAnim("Dead",-1.0f);
        aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
        aIGameManger.ReleaseSoilderAI(this, 5f);
    }


    public virtual void MotionAutoReturn()
    { }

    #endregion

    #region 組合動作
    public virtual void MotionQueue()
    { }

    public virtual void Attack()
    {
        Vector3 fowardpos = Role.transform.position - Role.transform.forward * 2;
        nav.SetDestination(fowardpos);
        Role.transform.LookAt(theTarget.transform);
        m_anim.SetBool("Walk", false);
        //m_anim.SetBool("Attack", true);
        PlayAnim("Attack",0f);
        UnderTakeDamage(1, theTarget);
    }

    public virtual void UnderTakeDamage(int damage, GameObject target)
    {

        //Debug.Log("TargetOriginalHealth_BeforeTakeDamage : " + target.GetComponent<PlayerHealth>().currenthp);

        //int ep = Random.Range(0, 80);
        //target.GetComponent<PlayerHealth>().TakeDamage(damage, ep, this, proprity);
        //Debug.Log("UnderTakeDamage" + "_Target Name : " + target.name + " /Damage /" + damage);
        //Debug.Log("TargetNewHealth : " + target.GetComponent<PlayerHealth>().currenthp);
        target.GetComponent<PlayerHealth>().TakeDamage(damage, this, proprity);
    }

    public virtual void MotionTask()
    { }

    #endregion

    #region PlayShader

    //public virtual IEnumerator DissloveShader(float waitTime)
    //{
    //    if (RenderGamObjs != null)
    //    {
    //        float buff = .1f;
    //        if (runtime < 1)
    //        {
    //            yield return new WaitForSeconds(waitTime * 2);
    //        }
    //        runtime += 1;
    //        for (int i = 0; i < RenderGamObjs.Length; i++)
    //        {
    //            if(RenderGamObjs[i]!=null)
    //            {
    //                if (RenderGamObjs[i].materials[0].GetFloat("_DissolveValue") < 1)
    //                {
    //                    RenderGamObjs[i].materials[0].SetFloat("_DissolveValue", buff * runtime);
    //                    yield return new WaitForSeconds(waitTime);
    //                    aIGameManger.StartCoroutineEvent(DissloveShader(waitTime));
    //                }
    //                else
    //                {
    //                    yield return null;
    //                }
    //            }
    //            else
    //            {
    //                yield return null;
    //            }
    //        }


    //    }
    //}
    public virtual IEnumerator DissloveShader(float waitTime)
    {
        yield return new WaitForSeconds(2 * waitTime);
        float buff = .3f;
        float t = 0;
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            while (t < 1)
            {
                if (RenderGamObjs != null)
                {
                    t += (Time.deltaTime / 2);
                    RenderGamObjs[i].materials[0].SetFloat("_DissolveValue", t);
                    yield return null;
                }
            }
        }
    }

    public virtual IEnumerator HitShader(float waitTime)
    {
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            if(RenderGamObjs[i]!=null)
            {
                RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 1);
            }
            
        }
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            if(RenderGamObjs[i]!=null)
            {
                RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
            }
        }

    }

    #endregion
}
