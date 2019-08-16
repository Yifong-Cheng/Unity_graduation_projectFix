using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStateController : AIStateController
{
    public GameObject Tower;
    public bool CanEscape = true;
    public bool sneak;

    public bool DropProp;

    protected Renderer[] RenderGamObjs;
    protected int runtime;
    public enum EnemyAIState
    {
        IDLE,
        WALK,
        Chase,
        Wander,
        ATTACK,
        DEFEND,
        FLY,
        Avoid,
        Escape,
        DEAD
    }
    public EnemyAIState m_aiState = EnemyAIState.IDLE;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        proprity = GentleProprity;
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        //m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.Proprity = proprity;
        m_aiState = EnemyAIState.IDLE;
        CanEscape = true;
        SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        DropProp = true;
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
        //EventManager.StartListening("DeadEnemy", DeadEnemy);
    }

    public virtual void DefendIntialize(AIGameManger _aIGameManger)
    {
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        //m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.Proprity = proprity;
        CanEscape = false;
        SetValue();
        Tower = GameObject.FindObjectOfType<StageChallange>().gameObject;
        particileControl = Role.GetComponent<ParticileControl>();
        DropProp = false;
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
        
        EventManager.StartListening("DeadEnemy", DeadEnemy);
    }

    protected virtual void DeadEnemy()
    {
        p_health.currenthp -= 999;
    }
    protected override void FixUpdateAI()
    {
        StateAction();
        //InputProcess();
        AnimationFinishResetPosition();
        CheckDoEscape();
    }

    protected void CallParner()
    {
        aIGameManger.SummonEnemyAIHelp(this, Role, 20);
    }

    protected virtual void DoAttackTower()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Role.transform.position, LookRadious, Role.transform.forward, out hit, LookRadious))
        {
            if (hit.collider.GetComponent<StageChallange>()!=null)
            {
                Debug.Log("SeeTower" + ": " + hit.transform.name);
                theTarget = hit.transform.gameObject;
                
            }

        }
        if (Vector3.Distance(Role.transform.position, Tower.transform.position) < 5)
        {
            UnderTakeDamage(2, Tower);
        }

    }

    protected void AnimationFinishResetPosition()
    {
        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Sneak"))
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                ModelPrefab.transform.localPosition = Vector3.zero;
            }
        }

    }
    protected override void StateAction()
    {
        if (m_objInfo.IsDead == true)
        {
            m_aiState = EnemyAIState.DEAD;
        }
        m_objInfo.State = m_aiState.ToString();

        switch (m_aiState)
        {
            case EnemyAIState.IDLE:
                {
                    //1
                    MotionIdle(WaitTime);
                }

                break;

            case EnemyAIState.Wander:
                {
                    MotionWander(targetPos, wanderTime);
                }

                break;

            case EnemyAIState.WALK:
                {
                    //1
                    MotionWalk(targetPos.gameObject);
                }


                break;

            case EnemyAIState.Chase:
                {
                    MotionChase(theTarget);
                }

                break;

            case EnemyAIState.FLY:
                {
                    //1
                    MotionFly();
                }


                break;

            case EnemyAIState.ATTACK:
                {
                    //1
                    MotionAttack(theTarget);
                }


                break;

            case EnemyAIState.DEFEND:
                {
                    //1
                    MotionDefend(targetPos.transform.gameObject);
                    //MotionDefend(GameObject.FindObjectOfType<StageChallange>().gameObject);
                }

                break;

            case EnemyAIState.Avoid:
                {
                    if (currentAvoidTime < AvoidTime)
                    {
                        MotionAvoid(theTarget);
                    }
                    else
                    {
                        currentAvoidTime = 0;
                        m_aiState = EnemyAIState.IDLE;
                    }

                }

                break;

            case EnemyAIState.Escape:
                {
                    MotionEscape(theTarget);
                }

                break;

            case EnemyAIState.DEAD:
                {
                    //1
                    MotionDead();
                }

                break;

            default:
                break;
        }

        if(BeAttack)
        {
            particileControl.Play(0, 2, Role.transform.position + Role.transform.forward +new Vector3(0,1,0));
        }
    }

    #region AIMotion(AI動作)
    protected virtual bool CheckEnemy()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Role.transform.position, LookRadious, Role.transform.forward, out hit, LookRadious))
        {
            if (hit.collider.CompareTag("Soilder") || hit.collider.CompareTag("StageChallange") || hit.collider.CompareTag("Player"))
            {
                Debug.Log("SeeEnemy" + ": " + hit.transform.name);
                theTarget = hit.transform.gameObject;
                return true;
            }

        }
        return false;
    }

    protected virtual bool EnemyIsAround(float dis, GameObject target)
    {
        if (target != null)
        {
            return Vector3.Distance(Role.transform.position, target.transform.position) < dis ? true : false;
        }
        else
            return false;

    }
    protected virtual bool TowerIsAround(float dis, GameObject target)
    {
        if (target != null && target.CompareTag("StageChallange"))
        {
            return Vector3.Distance(Role.transform.position, target.transform.position) < dis ? true : false;
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

    protected virtual void MotionIdle(float w_time)
    {
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget) || !TowerIsAround(30, theTarget))
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
            currentWaitTime = 0;
            m_aiState = EnemyAIState.Wander;
        }
        if(!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            m_anim.SetBool("Walk", false);
            PlayAnim("Idle",-1.0f);
        }
        
    }

    //徘徊
    public virtual void MotionWander(CapsuleCollider t, float wd_time)
    {
        currentWanderTime += Time.deltaTime;
        //if (currentWanderTime > wd_time)
        //{
        //    currentWaitTime = 0;
        //    currentWanderTime = 0;
        //    m_anim.SetBool("Walk", false);
        //    m_aiState = EnemyAIState.IDLE;
        //}
        //else
        //{
        //    nav.SetDestination(t.gameObject.transform.position);
        //    if (Vector3.Distance(targetPos.transform.position, Role.transform.position) < 2)
        //    {
        //        ResetTargetPos(targetPos);
        //        m_anim.SetBool("Walk", false);
        //        m_aiState = EnemyAIState.IDLE;
        //    }

        //    if (CheckEnemy())
        //    {
        //        if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, theTarget))
        //        {
        //            Debug.Log("Enemy IS In AttackRange");
        //            m_anim.SetBool("Walk", false);
        //            m_aiState = EnemyAIState.ATTACK;
        //        }
        //        else
        //        {
        //            Debug.Log("Wander See Enemy Start Chase");
        //            m_aiState = EnemyAIState.Chase;
        //        }
        //    }
        //    m_anim.SetBool("Walk", true);
        //}
        nav.SetDestination(t.gameObject.transform.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) < 2)
        {
            ResetTargetPos(targetPos);
            m_anim.SetBool("Walk", false);
            m_aiState = EnemyAIState.IDLE;
        }

        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = EnemyAIState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                m_aiState = EnemyAIState.Chase;
            }
        }
        m_anim.SetBool("Walk", true);
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
        Debug.Log("Check if rolePos had value");
        Vector3 Dir = target.transform.position - RolePos;
        Dir.y = 0;
        Role.transform.rotation = Quaternion.LookRotation(Dir);

        nav.SetDestination(target.transform.position);
        m_anim.SetBool("Walk", true);
    }

    public virtual void MotionChase(GameObject target)
    {
        if (target != null)
        {
            if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, target))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = EnemyAIState.ATTACK;
            }

            if (Vector3.Distance(Role.transform.position, target.transform.position) > ChaseDistance && !target.CompareTag("StageChallange"))
            {
                Debug.Log("Enemy TooFar" + (" the distance :") + Vector3.Distance(Role.transform.position, target.transform.position));
                theTarget = null;
                m_anim.SetBool("Walk", false);
                m_aiState = EnemyAIState.IDLE;
            }
            else
            {
                m_anim.SetBool("Walk", true);
                nav.SetDestination(target.transform.position);
            }
        }
        else
        {
            theTarget = null;
            //m_anim.SetBool("Attack", false);
            m_anim.SetBool("Walk", false);
            m_aiState = EnemyAIState.IDLE;
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
            Debug.Log("Enemy Is Dead");
            theTarget = null;
            //m_anim.SetBool("Attack", false);
            m_aiState = EnemyAIState.IDLE;
        }
        else
        {
            if (currentTime > AttackTime)
            {
                Debug.Log("DoAttack 0.0");
                currentTime = 0;

                Attack();
            }
            else if (currentTime < AttackTime)
            {
                currentTime += Time.deltaTime;
                Vector3 backpos = Role.transform.position + target.transform.forward * 5;
                nav.SetDestination(backpos);
                //Debug.Log("BackPos : " + backpos);

                //m_anim.SetBool("Attack", false);
                if (audioController != null)
                {
                    //audioController.StopSound(0);
                }
            }
            else if (currentTime < 0)
            {
                Vector3 backpos = Role.transform.position - target.transform.forward * 4.5f;
                nav.SetDestination(backpos);
                m_anim.SetBool("Walk", true);
            }

            Vector3 Dir = target.transform.position - Role.transform.position;
            Dir.y = 0;
            Role.transform.rotation = Quaternion.LookRotation(Dir);

            if (!EnemyIsAround(AttackDistance, target) && !TowerIsAround(30, target))
            {
                Debug.Log("Start Chase Target");
                //m_anim.SetBool("Attack", false);
                m_aiState = EnemyAIState.Chase;
            }
            
        }
    }

    public virtual void MotionAvoid(GameObject target)
    {
        currentAvoidTime += Time.deltaTime;
    }

    public virtual void MotionFly()
    { }

    public virtual void MotionDefend(GameObject target)
    {
        m_anim.SetBool("Walk", true);
        nav.SetDestination(target.transform.position);
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, target))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = EnemyAIState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                m_aiState = EnemyAIState.Chase;
            }
        }
    }

    public virtual void MotionDead()
    {
        particileControl.Play(1, 2, Role.transform.position + new Vector3(0, Role.GetComponent<CharacterController>().height + 2, 0));
        nav.Stop();
        m_objInfo.IsDead = true;
        //m_anim.SetBool("IsDead", true);
        PlayAnim("Dead",-1.0f);
        aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
        if (DropProp)
        {
            CreatCrystal(1);
        }
        //EventManager.StopListening("DeadEnemy", DeadEnemy);
        aIGameManger.ReleaseEnemyAI(this, 5f);
    }
    public void StopListen()
    {
        EventManager.StopListening("DeadEnemy", DeadEnemy);
    }
    public virtual void CreatCrystal(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 P = Role.transform.position + Role.transform.forward * num;
            aIGameManger.CreatItem("Props/" + "Crystal", P, Quaternion.identity);
        }

    }

    public virtual void MotionAutoReturn()
    { }

    #endregion

    #region 組合動作
    public virtual void MotionQueue()
    { }

    public virtual void Attack()
    {
        m_anim.SetBool("Walk", false);
        //m_anim.SetBool("Attack", true);
        PlayAnim("Attack",0f);
        Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
        nav.SetDestination(fowardpos);
        if(theTarget.CompareTag("Player"))
        {
            int num = Random.Range(0, 4);
            int StoleNum = Random.Range(1, 3);
            if(Count.Instance.number[num]> StoleNum)
            {
                Count.Instance.number[num]--;
            }
            theTarget = null;
            
        }
        else
        {
            UnderTakeDamage(1, theTarget);
        }
        
    }

    public virtual void UnderTakeDamage(int damage, GameObject target)
    {
        if(!target.CompareTag("StageChallange"))
        {
            //int ep = Random.Range(0, 80);
            //target.GetComponent<PlayerHealth>().TakeDamage(damage, ep, this, proprity);
            target.GetComponent<PlayerHealth>().TakeDamage(damage, this, proprity);
            //Debug.Log("UnderTakeDamage");
        }
        else
        {
            Debug.Log(target.name);
            target.GetComponent<PlayerHealth>().TakeDamage(damage, 100);
        }
        
    }

    public virtual void CheckDoEscape()
    {
        if (CanEscape)
        {
            if (p_health.currenthp <= 5f)
            {
                Debug.Log(Role.name + "start escape");
                //m_anim.SetBool("Attack", false);
                m_anim.SetBool("Walk", true);
                CanEscape = false;
                m_aiState = EnemyAIState.Escape;
                //MotionEscape(theTarget);
            }
        }

    }

    public virtual void MotionEscape(GameObject target)
    {
        if(target!=null)
        {
            Debug.Log(Role.name + "Is DoEscape");
            Vector3 pos = target.transform.forward * 5 + Role.transform.position;
            targetPos.transform.position = pos;
            nav.SetDestination(targetPos.transform.position);
            nav.speed = 2f;
        }
        else
        {
            nav.Stop();
        }
        
    }

    #endregion

    #region shader

    //public virtual IEnumerator DissloveShader(float waitTime)
    //{
    //    if (RenderGamObjs != null)
    //    {
    //        float buff = .3f;
    //        if (runtime < 1)
    //        {
    //            yield return new WaitForSeconds(waitTime * 2);
    //        }
    //        runtime += 1;
    //        for (int i = 0; i < RenderGamObjs.Length; i++)
    //        {
    //            if (RenderGamObjs[i] != null)
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
        //yield return new WaitForSeconds(2 * waitTime);
        //float buff = .3f;
        //float t=0;
        //for (int i = 0; i < RenderGamObjs.Length; i++)
        //{
        //    while (t < 1)
        //    {
        //        if (RenderGamObjs != null)
        //        {
        //            t += (Time.deltaTime/2);
        //            RenderGamObjs[i].materials[0].SetFloat("_DissolveValue",t);
        //            yield return null;
        //        }
        //    }
        //}

        //yield return new WaitForSeconds(2 * waitTime);
        //float buff = .3f;
        //float t = 0;
        //for (int i = 0; i < RenderGamObjs.Length; i++)
        //{
        //    while (t < 1)
        //    {
        //        t += Time.deltaTime;
        //        if (RenderGamObjs != null)
        //        {
        //            //t += (Time.deltaTime / 2);
        //            RenderGamObjs[i].materials[0].SetFloat("_DissolveValue", t);
        //            yield return null;
        //        }
        //    }
        //}

        yield return new WaitForSeconds(2 * waitTime);
        float buff = .3f;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            for (int i = 0; i < RenderGamObjs.Length; i++)
            {
                if (RenderGamObjs != null)
                {
                    //t += (Time.deltaTime / 2);
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
            if (RenderGamObjs[i] != null)
            {
                RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 1);
            }

        }
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            if (RenderGamObjs[i] != null)
            {
                RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
            }
        }

    }

    #endregion
}
