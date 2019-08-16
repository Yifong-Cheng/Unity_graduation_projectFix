using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombineEnemyAIStateController : AIStateController
{
    public enum CombineEnemyState
    {
        Wait,
        IDLE,
        WALK,
        Chase,
        Wander,
        Avoid,
        ATTACK,
        DEFEND,
        Escape,
        DEAD,
        FindCombinePos,
        StartCOMBINE,
        IsCombine,
        CombineOther,
    }
    public bool CanEscape, CanCombine;
    public CombineEnemyState m_aiState;

    //----
    public Collider[] Aroundcolliders;

    public float Checktimer = 0;

    public List<CombineEnemyAIStateController> neighbors = new List<CombineEnemyAIStateController>();

    public float checkInterval = 0.3f;

    public float detectRadius = 10f;

    public LayerMask layersChecked;
    //----
    public bool IsGroup;
    private float combineTime = 2f;
    private float currentCombineTime = 0;

    protected string ChangePfbSln = "";

    public GameObject originator;

    public bool DropProp;

    protected Renderer[] RenderGamObjs;
    protected int runtime;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        proprity = MediumProprity;
        DropProp = true;
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.Proprity = proprity;
        m_aiState = CombineEnemyState.IDLE;
        CanEscape = true;
        CanCombine = true;
        SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        //layersChecked = LayerMask.NameToLayer("Combine");
        DropProp = true;
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
        EventManager.StartListening("DeadEnemy", DeadEnemy);
    }

    protected override void FixUpdateAI()
    {
        StateAction();
        //AnimationFinishResetPosition();
        CheckDoEscape();
        if(CanCombine)
        {
            if (theTarget != null && !IsGroup)
            {
                Debug.Log("is working check");
                aIGameManger.CheckCombine(this);
            }
        }
        AnimationFinishResetPosition();
    }

    public virtual void DefendIntialize(AIGameManger _aIGameManger)
    {
        proprity = MediumProprity;
        aIGameManger = _aIGameManger;
        SetObjInfo();
        player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        nav = Role.GetComponent<NavMeshAgent>();
        targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        targetPos.transform.parent = null;
        ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.Proprity = proprity;
        CanEscape = false;
        CanCombine = true;
        //CanCombine = false;
        SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        //layersChecked = LayerMask.NameToLayer("Combine");
        DropProp = false;
        RenderGamObjs = ModelPrefab.transform.GetComponentsInChildren<Renderer>();
        EventManager.StartListening("DeadEnemy", DeadEnemy);
    }
    protected void AnimationFinishResetPosition()
    {
        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                ModelPrefab.transform.localPosition = Vector3.zero;
                ModelPrefab.transform.localRotation = Role.transform.rotation;
            }
        }

    }

    protected virtual void DeadEnemy()
    {
        p_health.currenthp -= 999;
    }

    protected override void StateAction()
    {
        if (m_objInfo.IsDead == true)
        {
            m_aiState = CombineEnemyState.DEAD;
        }
        m_objInfo.State = m_aiState.ToString();

        switch (m_aiState)
        {
            case CombineEnemyState.Wait:
                {
                    nav.Stop();
                    nav.ResetPath();
                    targetPos.transform.position = Role.transform.position + Role.transform.forward * 3;
                    SetCombinePos();
                    CheckArrive();
                }
                break;

            case CombineEnemyState.IDLE:
                {
                    //1
                    MotionIdle(WaitTime);
                }

                break;

            case CombineEnemyState.Wander:
                {
                    MotionWander(targetPos, wanderTime);
                }

                break;

            case CombineEnemyState.WALK:
                {
                    //1
                    MotionWalk(targetPos.gameObject);
                }


                break;

            case CombineEnemyState.Chase:
                {
                    MotionChase(theTarget);
                }

                break;

            case CombineEnemyState.ATTACK:
                {
                    //1
                    MotionAttack(theTarget);
                }


                break;

            case CombineEnemyState.FindCombinePos:
                {
                    MotionFindCombinePos(targetPos.gameObject);
                }
                break;

            case CombineEnemyState.StartCOMBINE:
                {
                    StartCombine();
                }
                break;

            case CombineEnemyState.IsCombine:
                {
                    //
                    m_aiState = CombineEnemyState.ATTACK;
                }
                break;

            case CombineEnemyState.CombineOther:
                {
                    aIGameManger.ReleaseCombineEnemyAI(this, .5f);
                }
                break;

            case CombineEnemyState.DEFEND:
                {
                    //1
                    MotionDefend(targetPos.transform.gameObject);
                }

                break;

            case CombineEnemyState.Avoid:
                {
                    if (currentAvoidTime < AvoidTime)
                    {
                        MotionAvoid(theTarget);
                    }
                    else
                    {
                        currentAvoidTime = 0;
                        m_aiState = CombineEnemyState.IDLE;
                    }

                }

                break;

            case CombineEnemyState.Escape:
                {
                    MotionEscape(theTarget);
                }

                break;

            case CombineEnemyState.DEAD:
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
            if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_aiState = CombineEnemyState.ATTACK;
            }
            else
            {
                Debug.Log("SeeEnemy Start Chase");
                m_aiState = CombineEnemyState.Chase;
            }

        }
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime > w_time)
        {
            Debug.Log("Start Wander");
            //DigOutGround();
            m_aiState = CombineEnemyState.Wander;
        }

        if (BeAttack)
        {
            BeAttack = false;
            if (theTarget != null)
            {
                Debug.Log("Beattack  attacker's name = " + theTarget.name);
            }
            else
            {
                Debug.Log("Target is dead");
            }

            m_aiState = CombineEnemyState.ATTACK;
        }
    }

    //徘徊
    public virtual void MotionWander(CapsuleCollider t, float wd_time)
    {
        //currentWanderTime += Time.deltaTime;
        //if (currentWanderTime > wd_time)
        //{
        //    currentWaitTime = 0;
        //    currentWanderTime = 0;
        //    m_anim.SetBool("Walk", false);
        //    m_aiState = CombineEnemyState.IDLE;
        //}
        //else
        //{
        //    nav.SetDestination(t.gameObject.transform.position);
        //    if (Vector3.Distance(targetPos.transform.position, Role.transform.position) < 2)
        //    {
        //        ResetTargetPos(targetPos);
        //        m_anim.SetBool("Walk", false);
        //        m_aiState = CombineEnemyState.IDLE;
        //    }

        //    if (CheckEnemy())
        //    {
        //        if (EnemyIsAround(AttackDistance, theTarget))
        //        {
        //            Debug.Log("Enemy IS In AttackRange");
        //            m_anim.SetBool("Walk", false);
        //            m_aiState = CombineEnemyState.ATTACK;
        //        }
        //        else
        //        {
        //            Debug.Log("Wander See Enemy Start Chase");
        //            m_aiState = CombineEnemyState.Chase;
        //        }
        //    }
        //    m_anim.SetBool("Walk", true);
        //}

        //--------------

        currentWaitTime = 0;
        nav.SetDestination(t.gameObject.transform.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) <= StopDistance)
        {
            ResetTargetPos(targetPos);
            m_anim.SetBool("Walk", false);
            m_aiState = CombineEnemyState.IDLE;
        }

        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                //CallParner();
                m_aiState = CombineEnemyState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                m_aiState = CombineEnemyState.Chase;
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
            if (EnemyIsAround(AttackDistance, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = CombineEnemyState.ATTACK;
            }

            if (Vector3.Distance(Role.transform.position, target.transform.position) > ChaseDistance)
            {
                Debug.Log("Enemy TooFar");
                theTarget = null;
                m_anim.SetBool("Walk", false);
                m_aiState = CombineEnemyState.IDLE;
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
            m_aiState = CombineEnemyState.IDLE;
        }
    }

    public virtual void MotionRun()
    {
    }

    public virtual void MotionAttack(GameObject target)
    {
        if (target == null)
        {
            Debug.Log("Enemy Is Dead");
            theTarget = null;
            m_aiState = CombineEnemyState.IDLE;
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

            if (!EnemyIsAround(AttackDistance, target))
            {
                Debug.Log("Start Chase Target");
                m_aiState = CombineEnemyState.Chase;
            }
        }
    }

    public virtual void MotionAvoid(GameObject target)
    {
        currentAvoidTime += Time.deltaTime;
    }

    public virtual void MotionDefend(GameObject target)
    {
        m_anim.SetBool("Walk", true);
        nav.SetDestination(target.transform.position);
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = CombineEnemyState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                m_aiState = CombineEnemyState.Chase;
            }
        }
    }

    public virtual void MotionDead()
    {
        nav.Stop();
        m_objInfo.IsDead = true;
        particileControl.Play(1, 2F, Role.transform.position + new Vector3(0, Role.GetComponent<CharacterController>().height + 2, 0));
        //m_anim.SetBool("IsDead", true);
        m_anim.SetBool("Walk", false);
        PlayAnim("Dead", -1.0f);
        aIGameManger.StartCoroutineEvent(DissloveShader(.5f));
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
        
        aIGameManger.ReleaseCombineEnemyAI(this, 10);
    }
    public void StopListen()
    {
        EventManager.StopListening("DeadEnemy", DeadEnemy);
    }
    public virtual void CreatCrystal(int num)
    {
        for(int i=0;i<num;i++)
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

    //public virtual void Attack()
    //{
    //    m_anim.SetBool("Walk", false);
    //    PlayAnim("Attack", 0f);
    //    Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
    //    nav.SetDestination(fowardpos);
    //    if(!IsGroup)
    //    {
    //        UnderTakeDamage(1, theTarget);
    //    }
    //    else
    //    {
    //        UnderTakeDamage(4, theTarget);
    //    }

    //}

    public virtual void Attack()
    {
        m_anim.SetBool("Walk", false);
        //m_anim.SetBool("Attack", true);
        PlayAnim("Attack", 0f);
        Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
        nav.SetDestination(fowardpos);
        if (theTarget.CompareTag("Player"))
        {
            int num = Random.Range(0, 4);
            int StoleNum = Random.Range(1, 3);
            if (Count.Instance.number[num] > StoleNum)
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
        if (!target.CompareTag("StageChallange"))
        {
            int ep = Random.Range(30, 80);
            if(IsGroup)
            {
                //target.GetComponent<PlayerHealth>().TakeDamage(damage, ep, this, HeavyProprity);
                target.GetComponent<PlayerHealth>().TakeDamage(damage, this, HeavyProprity);
            }
            else
            {
                //target.GetComponent<PlayerHealth>().TakeDamage(damage, ep, this, MediumProprity);
                target.GetComponent<PlayerHealth>().TakeDamage(damage, this, MediumProprity);
            }
            
            Debug.Log("UnderTakeDamage");
        }
        else
        {
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
                m_aiState = CombineEnemyState.Escape;
            }
        }
    }

    public virtual void MotionEscape(GameObject target)
    {
        if(target!=null)
        {
            Vector3 pos = target.transform.forward * 5 + Role.transform.position;
            targetPos.transform.position = pos;
            nav.SetDestination(targetPos.transform.position);
            nav.speed = 2f;
        }
        else
        {
            //nav.Stop();
            nav.isStopped = true;
            nav.SetDestination(targetPos.transform.position);
        }
        
    }

    #endregion

    #region combine

    private void CheckCombine()
    {
        if (CanCombine)
        {
            if (!IsGroup)
            {
                Checktimer += Time.deltaTime;
                if (Checktimer > checkInterval)
                {
                    neighbors.Clear();
                    Debug.Log("Neighbor Clear!!");
                    Aroundcolliders = Physics.OverlapSphere(Role.transform.position, detectRadius, layersChecked);
                    for (int i = 0; i < Aroundcolliders.Length; i++)
                    {
                        if (Aroundcolliders[i].CompareTag("CombineBefore"))
                        {
                            //neighbors.Add(Aroundcolliders[i].gameObject);
                            Debug.Log("SEE PARNER" + ": " + Aroundcolliders[i].transform.name);
                            theTarget = Aroundcolliders[i].transform.gameObject;

                        }

                    }
                    Checktimer = 0;
                    Debug.Log("Neighbor Num is" + Aroundcolliders.Length);
                }
                if (Aroundcolliders.Length >= 4)
                {
                    for (int i = 0; i < Aroundcolliders.Length; i++)
                    {
                        Aroundcolliders[i].transform.GetComponent<ObjInfo>().m_ai.IsGroup = true;
                        Debug.Log(Aroundcolliders[i].name);
                        Aroundcolliders[i].transform.GetComponent<ObjInfo>().m_ai.nav.SetDestination(this.Role.transform.position);
                        if (Aroundcolliders[i].gameObject != Role.gameObject)
                        {
                            //Aroundcolliders[i].gameObject.GetComponent<StateController>().Model.GetComponent<Animator>().SetBool("Walk", true);
                            GameObject.Destroy(Aroundcolliders[i].gameObject, Mathf.Abs(Vector3.Distance(Aroundcolliders[i].transform.position, Role.transform.position)) / (nav.speed * 2));
                            //Aroundcolliders[i].gameObject.GetComponent<StateController>().ReleaseWayPoints();
                            //Debug.Log(" DD " + Mathf.Abs(Vector3.Distance(Aroundcolliders[i].transform.position, controller.transform.position)) / controller.enermyStats.moveSpeed);
                            //Debug.Log(" DD " + Mathf.Abs(Vector3.Distance(Aroundcolliders[i].transform.position, controller.transform.position)) / (controller.enermyStats.moveSpeed * 2));
                            //Debug.Log(" DD " + Mathf.Abs(Vector3.Distance(Aroundcolliders[i].transform.position, controller.transform.position)) / controller.enermyStats.moveSpeed / 2);
                            //Debug.Log(" DD " + Mathf.Abs(Vector3.Distance(Aroundcolliders[i].transform.position, controller.transform.position)) / 2);
                        }
                        else
                        {
                            nav.Stop();
                        }
                    }
                    //return true;
                    Role.tag = "CombineAfter";
                    m_aiState = CombineEnemyState.StartCOMBINE;
                }

                else
                {
                    Debug.Log("false" + neighbors.Count);
                    //return false;
                }
            }
            else
            {
                Debug.Log("IsGroup Cant Combine again");
            }
        }
    }

    protected void StartCombine()
    {
        currentCombineTime += Time.deltaTime;
        if (currentCombineTime > combineTime)
        {
            proprity = HeavyProprity;
            //changemodel
            ChangeModel();
            DestoryOtherNeighbor();
            p_health.Proprity = proprity;
            m_aiState = CombineEnemyState.IsCombine;

        }
    }

    protected void DestoryOtherNeighbor()
    {
        for (int i = 0; i < 4; i++)
        {
            p_health.currenthp += neighbors[i].p_health.currenthp;
            neighbors[i].m_aiState = CombineEnemyState.CombineOther;
        }
        neighbors.Clear();

    }

    public virtual void ChangeModel()
    {
        IsGroup = true;
        GameObject go = Resources.Load<GameObject>(ChangePfbSln);
        go.name = "CA";
        GameObject.Destroy(ModelPrefab.gameObject);
        ModelPrefab = GameObject.Instantiate(go, Role.transform.position, Role.transform.rotation, Role.transform);
        m_anim = ModelPrefab.GetComponent<Animator>();
        //Snap = ModelPrefab.transform.GetChild(0).gameObject;
    }

    public virtual void SetCombinePos()
    {
        for (int i = 0; i < 4; i++)
        {
            neighbors[i].nav.SetDestination(this.targetPos.transform.position);
            neighbors[i].targetPos.transform.position = this.targetPos.transform.position;
        }
    }
    protected int arrivedNum = 1;
    public bool IsArrive;
    protected virtual void CheckArrive()
    {
        for(int i=0;i<4;i++)
        {
            if(Vector3.Distance( neighbors[i].Role.transform.position,Role.transform.position)< 10 && !neighbors[i].IsArrive)
            {
                neighbors[i].IsArrive = true;
                arrivedNum += 1;
            }
        }
        if(arrivedNum>4)
        {
            m_aiState = CombineEnemyState.StartCOMBINE;
        }
    }

    public virtual void MotionFindCombinePos(GameObject target)
    {
        if(originator!=null)
        {
            GoTargetPos(target);
        }
        else
        {
            IsGroup = false;
            m_aiState = CombineEnemyState.IDLE;
        }
        
    }

    public virtual void GoTargetPos(GameObject target)
    {
        nav.SetDestination(target.transform.position);
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
        while(t<1)
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
