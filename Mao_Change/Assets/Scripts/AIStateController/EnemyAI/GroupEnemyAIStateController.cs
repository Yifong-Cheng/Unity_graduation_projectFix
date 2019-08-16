using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupEnemyAIStateController : AIStateController
{
    public enum GroupAIState
    {
        Null,
        IDLE,
        ATTACK,
        DEFEND,
        COLLAPSE,
        DEAD
    }

    public GroupAIState m_aiState = GroupAIState.Null;

    public GameObject Tower;

    protected float currentDesPatchTime=9;
    protected float DesPatchTime = 25;
    protected int BeeNum;
    protected int CurrentBeeNum;
    protected float CreatNewBeeTime=35f;
    protected float currentCreatNewBeeTime;

    protected bool IsCollapse;

    protected Transform[] CreatPos;

    public GameObject Explosion;

    public List<BeeMonsterStateController> beeMonsterStateControllers = new List<BeeMonsterStateController>();

    public StageChallange stageChallange;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        aIGameManger = _aIGameManger;
        SetObjInfo();
        //player = GameObject.Find("Player");
        audioController = Role.GetComponent<AudioController>();
        //nav = Role.GetComponent<NavMeshAgent>();
        //targetPos = Role.transform.GetChild(2).GetComponent<CapsuleCollider>();
        //targetPos.transform.parent = null;
        //ResetTargetPos(targetPos);
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        if(Role.transform.Find("Explosion")!=null)
        {
            Explosion = Role.transform.Find("Explosion").gameObject;
        }
        
        //m_anim = Role.transform.GetChild(1).GetComponent<Animator>();
        m_anim = ModelPrefab.GetComponent<Animator>();
        m_anim = null;
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        m_aiState = GroupAIState.IDLE;
        SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        Tower = GameObject.FindObjectOfType<StageChallange>().gameObject;
        stageChallange = GameObject.FindObjectOfType<StageChallange>();
    }

    public void PassInitialize(AIGameManger _aIGameManger)
    {
        aIGameManger = _aIGameManger;
        SetObjInfo();
        ModelPrefab = Role.transform.GetChild(1).gameObject;
        if (Role.transform.Find("Explosion") != null)
        {
            Explosion = Role.transform.Find("Explosion").gameObject;
        }
        stageChallange = GameObject.FindObjectOfType<StageChallange>();
        //audioController = Role.GetComponent<AudioController>();
        p_health = Role.GetComponent<PlayerHealth>();
        p_health.stateController = this;
        p_health.currenthp -= 999;
        m_aiState = GroupAIState.DEAD;
        //SetValue();
        particileControl = Role.GetComponent<ParticileControl>();
        MotionCollapse();

    }

    public override void SetValue()
    {
        //base.SetValue();
        LookRadious = 30f;
        Debug.Log(Role.name);
        AttackTime = 3f;
        BeeNum = p_health.currenthp * (1);
        CurrentBeeNum = BeeNum;
        Debug.Log("This house TottleMax has " + CurrentBeeNum + "Bee");
        DesPatchTime = Random.Range(15, 25);
        CreatPos = Role.transform.Find("CreatPos").GetComponentsInChildren<Transform>();
    }

    protected override void FixUpdateAI()
    {
        StateAction();
        if(m_objInfo!=null)
        {
            m_objInfo.State = m_aiState.ToString();
        }

        currentCreatNewBeeTime += Time.deltaTime;
        if(currentCreatNewBeeTime>CreatNewBeeTime)
        {
            if(CurrentBeeNum<BeeNum)
            {
                CurrentBeeNum += 1;
            }
            else
            {

            }
            
        }

    }

    protected override void StateAction()
    {
        switch(m_aiState)
        {
            case GroupAIState.Null:
                {
                    
                    m_aiState = GroupAIState.IDLE;
                }
                break;

            case GroupAIState.IDLE:
                {


                    MotionIdle();
                }
                break;

            case GroupAIState.ATTACK:
                {
                    MotionAttack();
                }
                break;

            case GroupAIState.DEFEND:
                {

                }
                break;

            case GroupAIState.DEAD:
                {
                    MotionCollapse();
                    //MotionDead();
                    
                }
                break;

            case GroupAIState.COLLAPSE:
                {
                    MotionCollapse();
                    MotionDead();
                }
                break;

            default:
                break;
        }
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

    public virtual void MotionIdle()
    {
        currentDesPatchTime += Time.deltaTime;
        if (CheckEnemy())
        {
            
            if(currentDesPatchTime>DesPatchTime)
            {
                currentDesPatchTime = 0;
                DesPatch(theTarget);
            }            
        }

        if (currentDesPatchTime > DesPatchTime)
        {
            currentDesPatchTime = 0;
            DesPatch();
        }

        if (BeAttack)
        {
            m_aiState = GroupAIState.ATTACK;
        }
    }

    public virtual void MotionAttack()
    {
        DefendDesPatch(theTarget);
        m_aiState = GroupAIState.IDLE;
    }

    public virtual void MotionDead()
    {
        //Role.GetComponent<FracturedChunk>().enabled = true;
        //play particle
        //m_aiState = GroupAIState.COLLAPSE;
        IsEnable = false;
    }

    public virtual void MotionCollapse()
    {

        if(!IsCollapse)
        {
            
            //if (stageChallange.ChallangeStart && !stageChallange.IsPass)
            //{

            //    aIGameManger.StartCoroutineEvent(Revenge(1f));
            //    if (Explosion != null)
            //    {
            //        particileControl.Play(2, 5, Role.transform.position + new Vector3(0, 2, 0));
            //        ModelPrefab.SetActive(false);
            //        Explosion.SetActive(true);
            //    }
            //}
            //else if (!stageChallange.ChallangeStart && !stageChallange.IsPass)
            //{
            //    if (Explosion != null)
            //    {
            //        particileControl.Play(2, 5, Role.transform.position + new Vector3(0, 2, 0));
            //        ModelPrefab.SetActive(false);
            //        Explosion.SetActive(true);
            //    }
            //}
            //else if (stageChallange.ChallangeStart && stageChallange.IsPass)
            //{
            //    if (Explosion != null)
            //    {
            //        particileControl.Play(2, 5, Role.transform.position + new Vector3(0,2,0));
            //        ModelPrefab.SetActive(false);
            //        Explosion.SetActive(true);
            //    }
            //}
            IsCollapse = true;
            //for (int i = 0; i < CurrentBeeNum; i++)
            //{
            //    int creatnum = Random.Range(0, CreatPos.Length);
            //    Vector3 P = CreatPos[creatnum].position;
            //    BeeMonsterStateController enemyai = new BeeMonsterStateController();
            //    aIGameManger.CreatGroupRoleRevenge(enemyai, "Enemy/" + "BeeM", P, Quaternion.identity, this);
            //}

            aIGameManger.StartCoroutineEvent(Revenge(1f,CurrentBeeNum));
            if (Explosion != null)
            {
                ModelPrefab.SetActive(false);
                Explosion.SetActive(true);
            }
            MotionDead();
        }

        
    }

    

    private IEnumerator Revenge(float delayTime,int num)
    {
        if (stageChallange.ChallangeStart && !stageChallange.IsPass)
        {
            for (int i = 0; i < num; i++)
            {
                int creatnum = Random.Range(0, CreatPos.Length);
                Vector3 P = CreatPos[creatnum].position;
                BeeMonsterStateController enemyai = new BeeMonsterStateController();
                aIGameManger.CreatGroupRoleRevenge(enemyai, "Enemy/" + "BeeM", P, Quaternion.identity, this);
                enemyai.targetPos.transform.position = Tower.transform.position;
            }
        }
        yield return new WaitForSeconds(delayTime);

        yield return new WaitForSeconds(delayTime);
        for (int i = 0; i < beeMonsterStateControllers.Count; i++)
        {
            if(beeMonsterStateControllers[i].IsEnable)
            {
                //beeMonsterStateControllers[i].p_health.currenthp -= 999;
                //beeMonsterStateControllers[i].targetPos.transform.position = Tower.transform.position;
                beeMonsterStateControllers[i].m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                beeMonsterStateControllers[i].House = null;
                beeMonsterStateControllers[i].targetPos.transform.position = Tower.transform.position;
            }
            

        }
        yield return new WaitForSeconds(delayTime/2);
        
    }

    public virtual void DesPatch(GameObject target)
    {
        
        CurrentBeeNum -= 1;
        //Vector3 P = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Role.transform.position + Role.transform.forward*5;
        int creatnum = Random.Range(0, CreatPos.Length);
        Vector3 P = CreatPos[creatnum].position;
        BeeMonsterStateController enemyai = new BeeMonsterStateController();
        aIGameManger.CreatGroupRole(enemyai, "Enemy/" + "BeeM", P, Quaternion.identity,this,target);
    }
    public virtual void DesPatch()
    {
        CurrentBeeNum -= 1;
        //Vector3 P = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Role.transform.position + Role.transform.forward * 5;
        int creatnum = Random.Range(0, CreatPos.Length);
        Vector3 P = CreatPos[creatnum].position;
        BeeMonsterStateController enemyai = new BeeMonsterStateController();
        aIGameManger.CreatGroupRole(enemyai, "Enemy/" + "BeeM", P, Quaternion.identity, this);
    }
    public virtual void DefendDesPatch(GameObject target)
    {
        int nums = Random.Range(2, 4);
        if(CurrentBeeNum >= nums)
        {
            CurrentBeeNum -= nums;
            for (int i = 0; i < nums; i++)
            {
                //Vector3 P = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Role.transform.position + Role.transform.forward * 5;
                int creatnum = Random.Range(0, CreatPos.Length);
                Vector3 P = CreatPos[creatnum].position;
                BeeMonsterStateController enemyai = new BeeMonsterStateController();
                aIGameManger.CreatGroupRole(enemyai, "Enemy/" + "BeeM", P, Quaternion.identity, this, target);
            }
        }
        else
        {
            Debug.Log("NotEnough Bee");
        }
    }

    public void RemoveFromHouse(BeeMonsterStateController bee)
    {
        beeMonsterStateControllers.Remove(bee);
    }
}
