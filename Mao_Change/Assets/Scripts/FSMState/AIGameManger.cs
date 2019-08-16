using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIGameManger : MonoBehaviour
{
    public List<AIController> aIControllers = new List<AIController>();
    [Range(1, 5)]
    public int nums;

    public Transform CreatPOSCenter;
	
	public List<SoilderAIStateController> aiStateControllers = new List<SoilderAIStateController>();
    public List<EnemyAIStateController> enemyaiStateControllers = new List<EnemyAIStateController>();
    public List<CombineEnemyAIStateController> combineaiStateControllers = new List<CombineEnemyAIStateController>();
    public List<GroupEnemyAIStateController> groupEnemyAIStateControllers = new List<GroupEnemyAIStateController>();
    public List<BossAction> Boss = new List<BossAction>();
    private LoadAndSave datamanger;
    public int StageNum;

    public List<GameObject> BeeHouse ;
    [Header("對應上方beehouse")]
    public List<int> BeeHouseHp;

    private void Awake()
    {
        datamanger = GameObject.Find("Stage" + StageNum).GetComponent<LoadAndSave>();
        if (!datamanger.m_datainfolist.stageData.Pass)
        {
            if (BeeHouse.Count > 0)
            {
                for (int i = 0; i < BeeHouse.Count; i++)
                {
                    GroupEnemyAIStateController m_ai = new GroupEnemyAIStateController();
                    m_ai.Role = BeeHouse[i].gameObject;
                    m_ai.Intialize(this);

                    groupEnemyAIStateControllers.Add(m_ai);
                    Debug.Log(m_ai.Role.name);
                }
            }
        }
        else
        {
            if (BeeHouse.Count > 0)
            {
                for (int i = 0; i < BeeHouse.Count; i++)
                {
                    GroupEnemyAIStateController m_ai = new GroupEnemyAIStateController();
                    m_ai.Role = BeeHouse[i].gameObject;
                    m_ai.PassInitialize(this);

                    groupEnemyAIStateControllers.Add(m_ai);
                    Debug.Log(m_ai.Role.name);
                }
            }
        }
            
    }

    // Use this for initialization
    void Start () {
        //CreatTests(nums);
        //soilder
        if(datamanger.m_datainfolist.dataList!=null)
        {
            Debug.Log("TheSeconTime or more into scene" + datamanger.m_datainfolist.dataList.Count);
            datamanger.LoadSoilderData();
        }
        else
        {
            Debug.Log("IsEmpty or the FirstTime");
        }

        //monster
        if(datamanger.m_datainfolist.monsterdataList!=null)
        {
            //Debug.LogWarning("Load MonsterData");
            datamanger.LoadEnemyData();
        }
        else
        {
            Debug.Log("IsEmpty or the FirstTime");
        }
	}

    bool ShowDrawnumi;
	// Update is called once per frame
	void Update () {

        FixUpdated();

//#if UNITY_EDITOR
//        //UnityEditor.EditorApplication.isPlaying = false;
//        if (Input.GetKeyDown(KeyCode.LeftControl))
//        {
//            CreatTest(1);
//        }
//        else if (Input.GetKeyDown(KeyCode.RightControl))
//        {
//            CreatTest(0);
//        }

//        if (Input.GetKeyDown(KeyCode.LeftAlt))
//        {
//            CreatTests();
//        }
//#endif
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            CreatTest(0);
        }
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    ShowDrawnumi = true;
        //}
        //if(Input.GetKey(KeyCode.Z))
        //{
        //    ShowDrawnumi = false;
        //}
    }

    private void FixUpdated()
    {
        if (enemyaiStateControllers != null)
        {
            foreach (EnemyAIStateController aI in enemyaiStateControllers)
            {
                if (aI.IsEnable)
                {
                    if (aI.Role.GetComponent<PlayerHealth>().currenthp <= 0)
                    {
                        //ReleaseAI(aI);
                        aI.m_aiState = EnemyAIStateController.EnemyAIState.DEAD;
                    }
                    aI.Update();
                }
            }
            //List<EnemyAIStateController> enemyai = enemyaiStateControllers;
            //for(int i=0;i<enemyai.Count;i++)
            //{
            //    if (enemyai[i].Role.GetComponent<PlayerHealth>().currenthp <= 0)
            //    {
            //        //ReleaseAI(aI);
            //        enemyai[i].m_aiState = EnemyAIStateController.EnemyAIState.DEAD;
            //    }
            //    enemyai[i].Update();
            //}
        }

        int c = aiStateControllers.Count;
        if (aiStateControllers != null)
        {
            foreach (SoilderAIStateController aI in aiStateControllers)
            {
                if (aI.IsEnable)
                {
                    if (aI.Role.GetComponent<PlayerHealth>().currenthp <= 0)
                    {
                        //ReleaseAI(aI);
                        aI.m_aiState = SoilderAIStateController.AIState.DEAD;
                    }
                    aI.Update();
                }

            }

            //List<SoilderAIStateController> soilderai = aiStateControllers;
            //for(int i=0; i<soilderai.Count;i++)
            //{
            //    if (soilderai[i].Role.GetComponent<PlayerHealth>().currenthp <= 0)
            //    {
            //        //ReleaseAI(aI);
            //        soilderai[i].m_aiState = SoilderAIStateController.AIState.DEAD;
            //    }
            //    soilderai[i].Update();
            //}

        }

        if (combineaiStateControllers!= null)
        {
            foreach (CombineEnemyAIStateController aI in combineaiStateControllers)
            {
                if(aI.IsEnable)
                {
                    if (aI.Role.GetComponent<PlayerHealth>().currenthp <= 0)
                    {
                        //ReleaseAI(aI);
                        aI.m_aiState = CombineEnemyAIStateController.CombineEnemyState.DEAD;
                    }
                    aI.Update();
                }
                
            }
        }

        if(groupEnemyAIStateControllers!=null)
        {
            foreach(GroupEnemyAIStateController aI in groupEnemyAIStateControllers)
            {
                if(aI.IsEnable)
                {
                    if (aI.p_health.currenthp <= 0)
                    {
                        aI.m_aiState = GroupEnemyAIStateController.GroupAIState.DEAD;
                    }
                    aI.Update();
                }
                
            }
        }
        if (Boss != null)
        {
            foreach (BossAction aI in Boss)
            {
                if (aI.Role.GetComponent<PlayerHealth>().currenthp <= 0)
                {
                    //ReleaseAI(aI);
                    aI.currentState = BossAction.MonsterState.DEAD;
                }
                aI.Update();
            }
        }
    }

    void CreatTestAI(int nums )
    {
        for(int i=0;i<nums; i++)
        {
            AIController ai = new AIController();
            //ai set gameobj and model
            Debug.Log("GetModel ");
            GameObject go = Resources.Load<GameObject>("FSM/" + "TEnemy");
            ai.role = Instantiate(go);
            ai.role.transform.position = new Vector3(CreatPOSCenter.transform.position.x + Random.Range(-10, 30), 1, CreatPOSCenter.transform.position.z + Random.Range(-10, 30));
            Debug.Log("CreatModelSuccess ");
            ai.aIGameManger = this;
            ai.Start();
            aIControllers.Add(ai);
        }

        //Debug.Log("aIControllers CreatCount - " + aIControllers.Count);
    }

    void CreatTests()
    {
        int num = Random.Range(3, 8);
        for (int i = 0; i < num; i++)
        {
            Vector3 p = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)) + GameObject.FindGameObjectWithTag("Player").transform.position;
            LittleMonsterStateController enemyai = new LittleMonsterStateController();
            AutoCreatEnemyRole(enemyai, "Enemy/" + "NS", p, Quaternion.identity);
            enemyai.targetPos.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            enemyai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
        }

        //int num = Random.Range(90, 200);
        //for (int i = 0; i < num; i++)
        //{
        //    Vector3 p = new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000)) + new Vector3(0,1,0);
        //    BattleMaoStateController ai = new BattleMaoStateController();
        //    CreatSoilderRole(ai, "Props/" + "S4", p, Quaternion.identity);
        //    //SummonMaoStateController ai = new SummonMaoStateController();
        //    //CreatSoilderRole(ai, "Props/" + "S4", p, Quaternion.identity);
        //    //TestAiStateController ai = new TestAiStateController();
        //    //CreatSoilderRole(ai, "Props/" + "S4", p, Quaternion.identity);
        //    //ai.targetPos.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        //    //ai.m_aiState = SoilderAIStateController.AIState.IDLE;
        //}

    }
    void CreatTest(int type)
    {
        /*
        Debug.Log("aIControllers CreatCount - " + aIControllers.Count);
        AIController ai = new AIController();
        //ai set gameobj and model
        Debug.Log("GetModel ");
        GameObject go = Resources.Load<GameObject>("FSM/" + "TEnemy");
        ai.role = Instantiate(go);
        ai.role.transform.position = new Vector3(CreatPOSCenter.transform.position.x + Random.Range(-10, 30), 1, CreatPOSCenter.transform.position.z+Random.Range(-10, 30));
        ai.aIGameManger = this;
        ai.Start();
        aIControllers.Add(ai);
        ai = null;

        Debug.Log("aIControllers CreatSuccess the new count is - " + aIControllers.Count);
        */
        if(type==0)
        {
            Vector3 P = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + GameObject.FindGameObjectWithTag("Player").transform.position;
            EnemyAIStateController enemyai = new LittleMonsterStateController();
            CreatEnemyRole(enemyai, "Enemy/" + "NS", P, Quaternion.identity);
        }
        else if(type==1)
        {
            Vector3 P = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + GameObject.FindGameObjectWithTag("Player").transform.position;
            CombineEnemyAIStateController enemyai = new CombineMonsterStateController();
            CreatCombineEnemyRole(enemyai, "Enemy/" + "CB", P, Quaternion.identity);
        }
        else if (type == 2)
        {
            Vector3 P = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + GameObject.FindGameObjectWithTag("Player").transform.position;
            BossAction enemyai = new BossAction();
            CreatBossRole(enemyai, "Enemy/" + "Boss", P, Quaternion.identity);
        }
        //Vector3 P = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + GameObject.FindGameObjectWithTag("Player").transform.position;
        //EnemyAIStateController enemyai = new LittleMonsterStateController();
        //CreatEnemyRole(enemyai, "Enemy/" + "NS", P, Quaternion.identity);
        //Vector3 P = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + GameObject.FindGameObjectWithTag("Player").transform.position;
        //CombineEnemyAIStateController enemyai = new CombineMonsterStateController();
        //CreatCombineEnemyRole(enemyai, "Enemy/" + "CB", P, Quaternion.identity);
    }

    public void RemoveAIObj(AIController ai)
    {
        Destroy(ai.role, 3f);
        
    }
	
	public void RemoveAIObj(AIStateController ai)
    {
        Destroy(ai.Role, 3f);
        
    }
    #region CreatSolider
    public void CreatSoilderRole(SoilderAIStateController ai, string msg, Vector3 P, Quaternion Q)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        //ai.Role.transform.position = P;
        //ai.Role.transform.rotation = Q;
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        aiStateControllers.Add(ai);
        Debug.Log("Finish");
    }
    #endregion

    #region CreatEnemyRole

    public void CreatEnemyRole(EnemyAIStateController ai, string msg, Vector3 P, Quaternion Q)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        //ai.Role.transform.position = P;
        //ai.Role.transform.rotation = Q;
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        enemyaiStateControllers.Add(ai);
        //Debug.Log("Finish");
    }

    public void CreatGroupRoleRevenge(BeeMonsterStateController ai, string msg, Vector3 P, Quaternion Q, GroupEnemyAIStateController _House)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        //ai.Role.transform.position = P;
        //ai.Role.transform.rotation = Q;
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.DefendIntialize(this);
        ai.House = _House;

        //_House.beeMonsterStateControllers.Add(ai);
        enemyaiStateControllers.Add(ai);
    }

    public void CreatGroupRole(BeeMonsterStateController ai, string msg, Vector3 P, Quaternion Q, GroupEnemyAIStateController _House, GameObject target)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        //ai.Role.transform.position = P;
        //ai.Role.transform.rotation = Q;
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        ai.House = _House;
        if (_House.theTarget != null)
        {
            ai.theTarget = target;
            ai.DoChaseTarget(target);
        }

        //_House.beeMonsterStateControllers.Add(ai);
        enemyaiStateControllers.Add(ai);
        //Debug.Log("Finish");
    }
    public void CreatGroupRole(BeeMonsterStateController ai, string msg, Vector3 P, Quaternion Q, GroupEnemyAIStateController _House)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        //ai.Role.transform.position = P;
        //ai.Role.transform.rotation = Q;
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        ai.House = _House;
        _House.beeMonsterStateControllers.Add(ai);
        enemyaiStateControllers.Add(ai);
        //Debug.Log("Finish");
    }

    public void CreatCombineEnemyRole(CombineEnemyAIStateController ai, string msg, Vector3 P, Quaternion Q)
    {
        Debug.Log("StartCreatModel");
        ai.msg = msg;
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        combineaiStateControllers.Add(ai);

    }

    public void CreatBossRole(BossAction ai, string msg, Vector3 P, Quaternion Q)
    {
        Debug.Log("StartCreatModel");
        ai.msg = msg;
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);
        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.Intialize(this);
        Boss.Add(ai);

    }

    public void AutoCreatEnemyRole(EnemyAIStateController ai, string msg, Vector3 P, Quaternion Q)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);

        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.DefendIntialize(this);
        enemyaiStateControllers.Add(ai);
        //Debug.Log("Finish");
    }

    public void AutoCreatCombineRole(CombineEnemyAIStateController ai, string msg, Vector3 P, Quaternion Q)
    {
        //Debug.Log("StartCreatModel");
        ai.msg = msg;
        //Debug.Log(msg);
        GameObject go = Resources.Load<GameObject>(msg);
        ai.Role = Instantiate(go, P, Q);

        ai.Role.GetComponent<PlayerHealth>().stateController = ai;//
        ai.DefendIntialize(this);
        combineaiStateControllers.Add(ai);
    }

    public void AutoCreatGroupMonsterRole()
    {

    }

    #endregion

    public GameObject CreatItem(string name,Transform t, Quaternion q)
	{
        //Debug.Log("StartCreat" + name);
        GameObject go = Resources.Load<GameObject>(name);
        
        return Instantiate(go,t.transform.localPosition,q,t.transform);
	}
    public GameObject CreatItem(string name, Vector3 t, Quaternion q)
    {
        //Debug.Log("StartCreat" + name);
        GameObject go = Resources.Load<GameObject>(name);

        return Instantiate(go, t, q);
    }

    public void ReleaseSoilderAI(SoilderAIStateController ai,float t)
    {
        Destroy(ai.targetPos.gameObject);
        Destroy(ai.Role.gameObject, t);
        ai.IsEnable = false;

        //aiStateControllers.Remove(ai);
        
    }

    public void DeadAllEnemy()
    {
        for(int i=0;i<enemyaiStateControllers.Count;i++)
        {
            enemyaiStateControllers[i].p_health.currenthp -= 999;
        }
        for(int i=0;i<combineaiStateControllers.Count;i++)
        {
            combineaiStateControllers[i].p_health.currenthp -= 999;
        }

        for(int i=0;i<groupEnemyAIStateControllers.Count;i++)
        {
            groupEnemyAIStateControllers[i].p_health.currenthp -= 999;
        }

        //Debug.LogWarning("AllEnemy is cleared");
    }
    public void ReleaseEnemyAI(EnemyAIStateController ai ,float t)
    {
        ai.StopListen();
        Destroy(ai.targetPos.gameObject);
        Destroy(ai.Role.gameObject, t);
        ai.IsEnable = false;
        //enemyaiStateControllers.Remove(ai);
    }

    public void ReleaseCombineEnemyAI(CombineEnemyAIStateController ai, float t)
    {
        ai.StopListen();
        Destroy(ai.targetPos.gameObject);
        Destroy(ai.Role.gameObject, t);
        ai.IsEnable = false;

        //combineaiStateControllers.Remove(ai);
    }

    public void SummonSoilderAIHelp(SoilderAIStateController ai, GameObject target, float radious)
    {
        foreach (SoilderAIStateController AI in aiStateControllers)
        {
            if(AI.IsEnable)
            {
                if (Vector3.Distance(ai.RolePos, AI.RolePos) < radious && AI != ai && AI.m_aiState != SoilderAIStateController.AIState.ATTACK && AI.m_aiState != SoilderAIStateController.AIState.Chase)
                {
                    //Debug.Log("SummonPos" + ai.RolePos);
                    //Debug.Log("WalkerPos" + AI.RolePos);
                    //AI.SummonTarget = target;
                    AI.targetPos.transform.position = target.transform.position;
                    AI.m_aiState = SoilderAIStateController.AIState.WALK;
                }
            }
        }
    }

    public void SummonEnemyAIHelp(EnemyAIStateController ai, GameObject target, float radious)
    {
        foreach (EnemyAIStateController AI in enemyaiStateControllers)
        {
            if(AI.IsEnable)
            {
                if (Vector3.Distance(ai.RolePos, AI.RolePos) < radious && AI != ai && AI.m_aiState != EnemyAIStateController.EnemyAIState.ATTACK && AI.m_aiState != EnemyAIStateController.EnemyAIState.Chase)
                {
                    //Debug.Log("SummonPos" + ai.RolePos);
                    //Debug.Log("WalkerPos" + AI.RolePos);
                    //AI.SummonTarget = target;
                    AI.targetPos.transform.position = target.transform.position;
                    AI.m_aiState = EnemyAIStateController.EnemyAIState.WALK;
                }
            }
        }
    }

    #region SummonMao

    public void SummonSoilderAI(SoilderAIStateController ai, GameObject target,float radious)
    {
        foreach(SoilderAIStateController AI in aiStateControllers)
        {
            if(AI.IsEnable)
            {
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < radious && AI != ai)
                {
                    Debug.Log("SummonPos" + ai.RolePos);
                    Debug.Log("WalkerPos" + AI.RolePos);
                    //AI.SummonTarget = target;
                    AI.targetPos.transform.position = target.transform.position;
                    AI.m_aiState = SoilderAIStateController.AIState.WALK;
                }
            }
        }
    }

    public void SummonEnd(SoilderAIStateController ai, GameObject target)
    {
        Debug.Log("Summon_End");
        foreach (SoilderAIStateController AI in aiStateControllers)
        {
            if(AI.IsEnable)
            {
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < 2 && AI != ai)
                {
                    //AI.SummonTarget = null;
                    Vector3 backPos = (target.transform.position - AI.Role.transform.forward) * 2;
                    AI.Role.GetComponent<NavMeshAgent>().SetDestination(backPos);

                    AI.m_aiState = SoilderAIStateController.AIState.IDLE;
                }
                else if (AI.m_aiState == SoilderAIStateController.AIState.WALK)
                {
                    //AI.SummonTarget = null;
                    AI.m_aiState = SoilderAIStateController.AIState.IDLE;
                }
            }
        }
    }

    #endregion

    #region BombMao
    public void StartCoroutineEvent(IEnumerator m_event)
    {
        StartCoroutine(m_event);
    }

    private IEnumerator Bombdamage(AIStateController AI,int damage,float waittime)
    {
        yield return new WaitForSeconds(waittime);
        AI.Role.GetComponent<PlayerHealth>().currenthp -= damage;
    }

    public void DamageAround(AIStateController ai, float radious,int damage)
    {
        //yield return new WaitForSeconds(3);
        foreach (AIStateController AI in aiStateControllers)
        {
            if(AI.IsEnable)
            {
                Debug.Log(ai.Role.name + "and target - " + AI.Role.name + "is" + Vector3.Distance(ai.RolePos, AI.RolePos));
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < radious && AI != ai)
                {
                    Debug.Log("BombUnderTakeDamage");
                    //AI.Role.GetComponent<PlayerHealth>().currenthp -= damage;
                    StartCoroutineEvent(Bombdamage(AI, (damage - (damage / 2)), 3));
                }
            }
        }

        foreach (AIStateController AI in enemyaiStateControllers)
        {
            if(AI.IsEnable)
            {
                Debug.Log(ai.Role.name + "and target - " + AI.Role.name + "is" + Vector3.Distance(ai.RolePos, AI.RolePos));
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < radious && AI != ai)
                {
                    Debug.Log("BombUnderTakeDamage");
                    //AI.Role.GetComponent<PlayerHealth>().currenthp -= damage;
                    StartCoroutineEvent(Bombdamage(AI, damage, 3));
                }
            }
            
        }

        foreach(AIStateController AI in combineaiStateControllers)
        {
            if(AI.IsEnable)
            {
                Debug.Log(ai.Role.name + "and target - " + AI.Role.name + "is" + Vector3.Distance(ai.RolePos, AI.RolePos));
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < radious && AI != ai)
                {
                    Debug.Log("BombUnderTakeDamage");
                    //AI.Role.GetComponent<PlayerHealth>().currenthp -= damage;
                    StartCoroutineEvent(Bombdamage(AI, damage, 3));
                }
            }
            
        }

        foreach(AIStateController AI in groupEnemyAIStateControllers)
        {
            if(AI.IsEnable)
            {
                Debug.Log(ai.Role.name + "and target - " + AI.Role.name + "is" + Vector3.Distance(ai.RolePos, AI.RolePos));
                if (Vector3.Distance(ai.Role.transform.position, AI.Role.transform.position) < radious && AI != ai)
                {
                    Debug.Log("BombUnderTakeDamage");
                    //AI.Role.GetComponent<PlayerHealth>().currenthp -= damage;
                    StartCoroutineEvent(Bombdamage(AI, 3, 3));
                }
            }
        }
    }

    #endregion

    public void TurnStageSave()
    {
        datamanger.m_datainfolist.dataList.Clear();
        for (int i=0;i<aiStateControllers.Count;i++)
        {
            if(aiStateControllers[i].IsEnable)
            {
                datamanger.SaveAIData(aiStateControllers[i].RolePos, aiStateControllers[i].PfbTypeName, aiStateControllers[i].Role.GetComponent<PlayerHealth>().currenthp);

            }
        }

        datamanger.m_datainfolist.monsterdataList.Clear();
        if(enemyaiStateControllers.Count!=null)
        {
            for (int i = 0; i < enemyaiStateControllers.Count; i++)
            {
                if(enemyaiStateControllers[i].IsEnable)
                {
                    datamanger.SaveMonsterAIData(enemyaiStateControllers[i].RolePos, enemyaiStateControllers[i].PfbTypeName, enemyaiStateControllers[i].p_health.currenthp);

                }
            }
        }
        if(combineaiStateControllers.Count!=null)
        {
            for (int i = 0; i < combineaiStateControllers.Count; i++)
            {
                if(combineaiStateControllers[i].IsEnable)
                {
                    datamanger.SaveMonsterAIData(combineaiStateControllers[i].RolePos, combineaiStateControllers[i].PfbTypeName, combineaiStateControllers[i].p_health.currenthp);

                }
            }
        }
        //Debug.Log("totalai : enemyai-" + enemyaiStateControllers.Count + "combineai" + combineaiStateControllers.Count);
        //Debug.Log(datamanger.monsterDataList.monsterDataList.Count);
    }

    private void OnDrawGizmos()
    {
        if(ShowDrawnumi)
        {
            foreach (SoilderAIStateController aI in aiStateControllers)
            {
                if(aI.IsEnable)
                {
                    aI.OnDrawGizmos();
                }
                
            }
            foreach (EnemyAIStateController aI in enemyaiStateControllers)
            {
                if(aI.IsEnable)
                {
                    aI.OnDrawGizmos();
                }
                
            }
            foreach(CombineEnemyAIStateController aI in combineaiStateControllers)
            {
                if(aI.IsEnable)
                {
                    aI.OnDrawGizmos();
                }
                
            }
        }
    }

    #region Combine

    public void CheckCombine(CombineEnemyAIStateController m_ai)
    {
        m_ai.Checktimer += Time.deltaTime;
        if(m_ai.Checktimer>m_ai.checkInterval)
        {
            Debug.LogWarning("Check work success!!");
            if(m_ai.neighbors.Count!=null)
            {
                m_ai.neighbors.Clear();
            }
            
            Debug.LogWarning("Add Neighbor!!");
            foreach (CombineEnemyAIStateController AI in combineaiStateControllers)
            {
                if(AI.IsEnable)
                {
                    if (Vector3.Distance(AI.Role.transform.position, m_ai.Role.transform.position) <= 50 && AI != m_ai && !AI.IsGroup)
                    {
                        m_ai.neighbors.Add(AI);
                    }
                }
            }
            if(m_ai.neighbors.Count>4)
            {
                Debug.LogWarning("Neighbor is amount 4!!");
                m_ai.IsGroup = true;
                m_ai.m_aiState = CombineEnemyAIStateController.CombineEnemyState.Wait;
                for(int i=0;i<4;i++)
                {
                    m_ai.neighbors[i].IsGroup = true;
                    m_ai.neighbors[i].originator = m_ai.Role;
                    m_ai.neighbors[i].m_aiState = CombineEnemyAIStateController.CombineEnemyState.FindCombinePos;
                }
            }
            m_ai.Checktimer = 0;
        }
        
    }

    #endregion

}
