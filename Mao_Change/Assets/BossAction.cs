using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAction : AIStateController {
    
    
    private Vector3 initialPosition;
    private GameObject ChangePosition;
    public GameObject ChangedTo;

    public float wanderRadius;
    public float alertRadius;
    public float defendRadius;
    public float chaseRadius;

    public float attackRange;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    public float actRestTme;            
    private float lastActTime;          

    private float diatanceToPlayer;
    private float diatanceToInitial;
    private Quaternion targetRotation;

    private bool is_Warned = false;
    private bool is_Running = false;
    private bool is_Changed = false;
    private bool tired = false;

    private float TriedTime = 10f;
    private float timecount = 0.0f;
    
    private float ThinkingTime;
    //生命、mp
    private float Hp = 100;
    private float mp = 0;
    private const string proprity = "Heavy";

    //public GameObject[] Monster = new GameObject[3];

    public enum MonsterState
    {
        STAND,
        WALK,
        CHASE,
        ATTACK,
        TRIED,
        DEAD,
        RETURN
    }
    public MonsterState currentState = MonsterState.STAND;

    public override void Intialize(AIGameManger _aIGameManger)
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
        initialPosition = ModelPrefab.transform.position;
        m_anim = ModelPrefab.GetComponent<Animator>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        currentState = MonsterState.STAND;
        SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        //layersChecked = LayerMask.NameToLayer("Combine");
    }
    public override void SetValue()
    {
        //Debug.Log(Role.name + "'s Stopdis = " + nav.stoppingDistance);
        AttackTime = 3f;
        AttackDistance = nav.stoppingDistance + 5f;
        ChaseDistance = nav.stoppingDistance * 5;
        LookRadious = nav.stoppingDistance + 3f;
        wanderTime = 0.5f;
        WaitTime = Random.Range(5, 10);
        //StopDistance = nav.stoppingDistance;
        StopDistance = 20f;
    }

    protected override void FixUpdateAI()
    {
        StateAction();
        Debug.Log(currentState.ToString());
       
    }
    protected override void StateAction()
    {
        if(currentState != MonsterState.TRIED)
        {
            mp += Time.deltaTime;
        }
        if (Hp <= 70f && tired == false)
        {
            currentState = MonsterState.TRIED;
            tired = true;
            
        }
        if (Hp <= 0.0f)
        {
            BoxBackageUIControl.Instance.SuccessState();
        }
        if (m_objInfo.IsDead == true)
        {
            currentState = MonsterState.DEAD;
        }
        m_objInfo.State = currentState.ToString();
        switch (currentState)
        {
            case MonsterState.STAND:
                MotionIdle(WaitTime);
                break;
            case MonsterState.WALK:
                MotionWander(targetPos, wanderTime);
                break;
            case MonsterState.CHASE:
                MotionChase(theTarget);
                break;
            case MonsterState.ATTACK:
                MotionAttack(theTarget);
                break;
            case MonsterState.TRIED:
                MotionTried();
                break;
            case MonsterState.RETURN:
                MotionReturn();
                break;

        }
    }
        

    protected virtual void MotionIdle(float w_time)
    {
        m_anim.SetBool("Walk", false);
        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget) || TowerIsAround(30, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                currentState = MonsterState.ATTACK;
            }
            else
            {
                Debug.Log("SeeEnemy Start Chase");
                currentState = MonsterState.CHASE;
            }

        }
        currentWaitTime += Time.deltaTime;
        ReturnCheck();
        if (currentWaitTime > w_time)
        {
            Debug.Log("Start Wander");
            //DigOutGround();
            currentState = MonsterState.WALK;
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

            currentState = MonsterState.ATTACK;
        }
    }
    public virtual void MotionWander(CapsuleCollider t, float wd_time)
    {
        targetRotation = Quaternion.LookRotation(t.gameObject.transform.position - ModelPrefab.transform.position, Vector3.up);        
        ModelPrefab.transform.rotation = Quaternion.Slerp(ModelPrefab.transform.rotation, targetRotation, turnSpeed);        
        currentWaitTime = 0;
        nav.SetDestination(t.gameObject.transform.position);
        if (Vector3.Distance(targetPos.transform.position, Role.transform.position) <= 5f)
        {
            m_anim.SetBool("Walk", false);
            ResetTargetPos(targetPos);
            
            currentState = MonsterState.STAND;
        }

        if (CheckEnemy())
        {
            if (EnemyIsAround(AttackDistance, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                
                //CallParner();
                currentState = MonsterState.ATTACK;
            }
            else
            {
                Debug.Log("Wander See Enemy Start Chase");
                currentState = MonsterState.CHASE;
            }
        }
        m_anim.SetBool("Walk", true);
        

    }
    /*public virtual void MotionWalk(GameObject target)
    {
        Debug.Log("Check if rolePos had value");
        Vector3 Dir = target.transform.position - RolePos;
        Dir.y = 0;
        Role.transform.rotation = Quaternion.LookRotation(Dir);

        nav.SetDestination(target.transform.position);
        m_anim.SetBool("Walk", true);
    }*/
    public virtual void MotionChase(GameObject target)
    {
        
        if (target != null)
        {
            initialPosition = target.transform.position;
            if (EnemyIsAround(AttackDistance, theTarget))
            {
                Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                //m_anim.SetTrigger("Idle");
                currentState = MonsterState.ATTACK;
            }

            if (Vector3.Distance(Role.transform.position, target.transform.position) > ChaseDistance)
            {
                Debug.Log("Enemy TooFar");
                theTarget = null;
                m_anim.SetBool("Walk", false);
                //m_anim.SetTrigger("Idle");
                currentState = MonsterState.STAND;
            }
            else
            {
                m_anim.SetBool("Walk", true);
                //m_anim.SetTrigger("Walk");
                nav.SetDestination(target.transform.position);
                nav.speed = nav.speed + 2f;
            }
        }
        else
        {
            theTarget = null;
            //m_anim.SetBool("Attack", false);
            m_anim.SetBool("Walk", false);
            currentState = MonsterState.STAND;
        }
    }
    public virtual void MotionAttack(GameObject target)
    {
        if (target == null)
        {
            Debug.Log("Enemy Is Dead");
            theTarget = null;
            currentState = MonsterState.STAND;
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
                    audioController.StopSound(0);
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
                currentState = MonsterState.CHASE;
            }
        }
    }
    public virtual void MotionTried()
    {
        float diatanceToChange = Vector3.Distance(ModelPrefab.transform.position, ChangePosition.transform.position);
        
        if (timecount <= TriedTime)
        {
            timecount += Time.deltaTime;
            nav.Stop();
            m_anim.SetBool("tried", true);
        }
        else if(timecount > TriedTime)
        {
            nav.ResetPath();
            m_anim.SetBool("Walk", true);
            nav.SetDestination(ChangePosition.transform.position);
            nav.Resume();
        }
        if (diatanceToChange <= 0.5f)
        {
            /*currentState = MonsterState.STAND;
            is_Changed = true;*/
            m_anim.SetBool("Walk", true);
            currentState = MonsterState.WALK;

        }
    }
    public virtual void MotionReturn()
    {
        
        targetRotation = Quaternion.LookRotation(initialPosition - ModelPrefab.transform.position, Vector3.up);
        diatanceToInitial = Vector3.Distance(ModelPrefab.transform.position, initialPosition);
        nav.SetDestination(initialPosition);
        Role.transform.rotation = Quaternion.Slerp(ModelPrefab.transform.rotation, targetRotation, turnSpeed);
        m_anim.SetBool("Walk", true);
        if(diatanceToInitial < 50f)
        {
            targetPos.transform.position = initialPosition;
            ResetTargetPos(targetPos);
            currentState = MonsterState.WALK;
        }
    }
    public virtual void Attack()
    {
        int RadomAction = Random.Range(1, 100);
        if (RadomAction >= 1 && RadomAction <= 70)
        {
            m_anim.SetBool("Walk", false);
            PlayAnim("Attack", 0f);
            Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
            nav.SetDestination(fowardpos);
            UnderTakeDamage(1, theTarget);
            RadomAction = 0;

        }
        else if (RadomAction >= 71 && RadomAction <= 89)  //範圍攻擊
        {
            m_anim.SetBool("Walk", false);
            PlayAnim("Attack", 0f);
            Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
            nav.SetDestination(fowardpos);
            UnderTakeDamage(1, theTarget);
            RadomAction = 0;
        }
        else if (RadomAction >= 90 && RadomAction <= 100 && mp >= 20)
        {
            m_anim.SetBool("Walk", false);
            PlayAnim("Idle", 0f);
            Vector3 fowardpos = Role.transform.position + Role.transform.forward * 3;
            nav.SetDestination(fowardpos);
            UnderTakeDamage(1, theTarget);
            RadomAction = 0;
            /*for (int i = 0; i < 3; i++)
            {
               
                Instantiate(test.gameObject, Monster[i].transform.position, Quaternion.identity);
            }*/

            mp = mp - 20f;
            RadomAction = 0;

        }
        
        
    }
    private void ResetTargetPos(CapsuleCollider t)
    {
        Vector3 center = t.transform.position;

        float range_x = Random.Range(2, 10);
        float range_z = Random.Range(2, 10);

        Vector3 newTargetPos = new Vector3(center.x + range_x, center.y, center.z + range_z);

        targetPos.transform.position = newTargetPos;
    }
    protected virtual bool CheckEnemy()
    {
        RaycastHit hit;
        if (Physics.SphereCast(Role.transform.position, LookRadious, Role.transform.forward, out hit, LookRadious))
        {
            if (hit.collider.CompareTag("Soilder") || hit.collider.CompareTag("StageChallange"))
            {
                Debug.Log("SeeEnemy" + ": " + hit.transform.name);
                theTarget = hit.transform.gameObject;
                return true;
            }

        }
        return false;
    }
    public virtual void ReturnCheck()
    {
        diatanceToInitial = Vector3.Distance(ModelPrefab.transform.position, initialPosition);


        if (diatanceToInitial > ChaseDistance)
        {
            currentState = MonsterState.RETURN;
        }
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
    public virtual void UnderTakeDamage(int damage, GameObject target)
    {
        if (!target.CompareTag("StageChallange"))
        {
            //int ep = Random.Range(0, 80);
            //target.GetComponent<PlayerHealth>().TakeDamage(damage, ep, this, proprity);
            target.GetComponent<PlayerHealth>().TakeDamage(damage, this, proprity);
            //Debug.Log("UnderTakeDamage");
        }
        else
        {
            target.GetComponent<PlayerHealth>().TakeDamage(damage, 100);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Change" && tired == true)
        {
            ModelPrefab.transform.position = ChangedTo.transform.position;
            initialPosition = ChangedTo.gameObject.GetComponent<Transform>().position;
            
        }
    }

}
