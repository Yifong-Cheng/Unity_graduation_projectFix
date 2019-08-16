using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageChallange : MonoBehaviour
{
    public bool IsPass;
    public bool ChallangeStart;
    //public int stageNum;
    public int totalChallange=3;
    public int currentChallange=0;
    public int PassChallange = 0;
    //public UnityEvent m_MyEvent = new UnityEvent();
    private LoadAndSave stageData;
    private bool CreatDoor;
    [Header("(*必須*)傳送門位置")]
    public Transform StageDoorPos;
    [Header("傳送門 : ")]
    public GameObject Door;
    [Header("(*必須*)出怪點 : ")]
    public Transform[] CreatPos;
    [Header("(*必須*)怪物名稱 : ")]
    public string[] MonsterType;
    [Header("(*必須*) 針筒")]
    public GameObject Syringe;
    private GameObject medicine;
    public AIGameManger aIGameManger;

    private LoadAndSave stagedata;
    private ParticileControl particileControl;
    private bool MedicineIn;

    public GameObject PlaceObj;

    public List<int> monsterNum = new List<int>();
    public List<string> monsterType = new List<string>();
    public List<int> challangeWaitTime = new List<int>();
    public List<Vector3> creatCenter = new List<Vector3>();
    [Range(0,2)]
    public float InvokeTime;
    public float currentChallangeTime = 0;
    public bool Startevent;
    private bool FirstCreatEnd;

    //水晶
    private int Need = 0;

    GameObject sucCanvas;

    private void OnEnable()
    {
        EventManager.StartListening("CreatMonster", CreatMonster);
        EventManager.StartListening(" ShowChallangeInfo", ShowChallangeInfo);
    }

    private void OnDisable()
    {
        EventManager.StopListening("CreatMonster", CreatMonster);
        EventManager.StopListening(" ShowChallangeInfo", ShowChallangeInfo);
    }

    void Start()
    {
        totalChallange = monsterNum.Count;
        if(totalChallange>0)
        {
            currentChallangeTime = challangeWaitTime[currentChallange];
        }
    }

    void Update()
    {

        if(currentChallange<totalChallange&&!IsPass)
        {
            if (currentChallangeTime < 0 && Startevent&& FirstCreatEnd)
            {

                EventManager.TriggerEvent("CreatMonster");
            }
            else if (currentChallangeTime >0 && Startevent&& FirstCreatEnd)
            {
                currentChallangeTime -= Time.deltaTime;
            }
        }
        else if(currentChallange > totalChallange && !IsPass)
        {
            IsPass = true;
            StartCoroutine(PassStage(.5f));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PassStage(.5f));
            //StartCoroutine(PassSpawner());
        }

        //if(sucCanvas!=null)
        //{
        //    if (Input.anyKeyDown)
        //        CloseGame(sucCanvas);
        //}
    }

    public void Initialized(AIGameManger _aIGameManger, LoadAndSave data)
    {
        IsPass = false;
        //Door = GameObject.FindGameObjectWithTag("StageDoor");
        aIGameManger = _aIGameManger;
        Door = Instantiate(Resources.Load<GameObject>("Door"), StageDoorPos.position, Quaternion.identity);
        Door.GetComponent<DoorInfo>().NextStage = aIGameManger.StageNum + 1;
        Door.SetActive(false);
        
        stageData = data;
        particileControl = GetComponent<ParticileControl>();

    }
    public void PassInitialized(AIGameManger _aIGameManger, LoadAndSave data)
    {
        IsPass = true;
        //BoxBackageUIControl.Instance.SuccessState();
        aIGameManger = _aIGameManger;
        Door = Instantiate(Resources.Load<GameObject>("Door"), StageDoorPos.position, Quaternion.identity);
        Door.GetComponent<DoorInfo>().NextStage = aIGameManger.StageNum + 1;
        Vector3 ChallangePos = this.transform.position;
        Syringe = GameObject.Instantiate(Resources.Load<GameObject>("Syringe"), ChallangePos, Quaternion.identity);
        medicine = Syringe.transform.GetChild(3).gameObject;
        medicine.transform.localScale = new Vector3(1, .1f, 1);
        //Syringe.SetActive(true);
        stageData = data;
        particileControl = GetComponent<ParticileControl>();
        if (PlaceObj != null)
        {
            PlaceObj.SetActive(true);
        }
        //StartCoroutine(PassSpawner());
    }

    //public void EasyChallange()
    //{
    //    ShowTimeDiscount(currentChallangeTime);
    //}

    void ShowTimeDiscount(float time)
    {
        GameObject go = Resources.Load<GameObject>("TimeCanvas/" + "TimeDisCountCanvas");
        go.GetComponent<DestroyByTimeExtended>().lifeTime = time - 1;
        TimeDiscount timeDiscount = GameObject.Instantiate(go).GetComponent<TimeDiscount>();
        timeDiscount.timecount = go.GetComponent<DestroyByTimeExtended>().lifeTime;
        timeDiscount.enabled = true;
    }

    void ShowChallangeInfo()
    {
        if (GameObject.Find("ShowChallangeInfo") == null)
        {
            GameObject go = Resources.Load<GameObject>("TimeCanvas/"+"ShowChallangeInfo");
            go.GetComponent<ShowChallangeInfo>().currentChallange= currentChallange;
            go.GetComponent<ShowChallangeInfo>().totalChallange = totalChallange;
            
            GameObject showinfo =  GameObject.Instantiate(go);
            showinfo.name = "ShowChallangeInfo";
        }
        else
        {
            ShowChallangeInfo go = GameObject.Find("ShowChallangeInfo").GetComponent<ShowChallangeInfo>();
            go.currentChallange = currentChallange;
            go.totalChallange = totalChallange;
            go.ResetChallangeText();
        }
        if(currentChallange==totalChallange)
        {
            IsPass = true;
            StartCoroutine(PassStage(.5f));
        }
    }

    public void TimeLineFirstPlay()
    {
        FirstCreatEnd = true;
        EventManager.TriggerEvent("CreatMonster");
    }

    void CreatMonster()
    {
        //if(teach.Instance.FirstMonster == false)
        //{
        //    teach.Instance.TeachLevel = 153;
        //}
        Vector3 ChallangePos = this.transform.position;
        GameObject go = Resources.Load<GameObject>("Syringe");
        PlayerHealth health;
        if (go.GetComponent<PlayerHealth>()!=null)
        {
            health = go.GetComponent<PlayerHealth>();
        }
        else
        {
            health = go.AddComponent<PlayerHealth>();
        }
        health.IsStageChallange = true;
        health.hp = 100;
        Syringe = GameObject.Instantiate(go, ChallangePos + new Vector3(0,10,0), Quaternion.identity);
        medicine = Syringe.transform.GetChild(3).gameObject;

        switch (monsterType[currentChallange])
        {
            case "Easy":
                {
                    for (int i = 0; i < monsterNum[currentChallange]; i++)
                    {
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        EnemyAIStateController ai = new LittleMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + "NS", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            case "Normal":
                {
                    for (int i = 0; i < monsterNum[currentChallange]; i++)
                    {

                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        CombineEnemyAIStateController ai = new CombineMonsterStateController();
                        aIGameManger.AutoCreatCombineRole(ai, "Enemy/" + "CB", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = CombineEnemyAIStateController.CombineEnemyState.DEFEND;
                    }
                }

                break;

            case "Mix1":
                {
                    int halfnum = monsterNum[currentChallange] / 2;
                    for (int i = 0; i < halfnum; i++)
                    {
                        
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        EnemyAIStateController ai = new BeeMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + "BeeM", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                    for (int i = halfnum; i < monsterNum[currentChallange]; i++)
                    {
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        EnemyAIStateController ai = new LittleMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + "NS", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            case "Mix2":
                {
                    int thirdnum = monsterNum[currentChallange] / 3;
                    for(int i=0;i<thirdnum;i++)
                    {
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        EnemyAIStateController ai = new BeeMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + "BeeM", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                    
                    for(int i = thirdnum;i<2* thirdnum;i++)
                    {
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        EnemyAIStateController ai = new LittleMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + "NS", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                    for (int i = 2-thirdnum; i < monsterNum[currentChallange]; i++)
                    {
                        Vector3 creatpos = creatCenter[currentChallange] + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        Quaternion spawnRotation = new Quaternion();
                        spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
                        CombineEnemyAIStateController ai = new CombineMonsterStateController();
                        aIGameManger.AutoCreatCombineRole(ai, "Enemy/" + "CB", creatpos, spawnRotation);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = CombineEnemyAIStateController.CombineEnemyState.DEFEND;
                    }

                }
                break;

            case "Mix3":
                {

                }
                break;

            case "Hard":
                {

                }
                break;

            default:
                break;
        }
        
        currentChallange += 1;
        if(currentChallange<totalChallange)
        {
            currentChallangeTime = challangeWaitTime[currentChallange];

            ShowTimeDiscount(currentChallangeTime);
        }
        
    }
    /*
    private void StartChallange(string type, Vector3 center)
    {
        currentCreatTime = 0;
        NextCreatTime = Random.Range(1.5f, 3);
        CreatTime = NextCreatTime;
        int nums = Random.Range(2, 5);

        switch (type)
        {
            case "NS":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        EnemyAIStateController ai = new LittleMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            case "CB":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        CombineEnemyAIStateController ai = new CombineMonsterStateController();
                        aIGameManger.AutoCreatCombineRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = CombineEnemyAIStateController.CombineEnemyState.DEFEND;
                    }
                }

                break;

            case "BeeM":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        EnemyAIStateController ai = new BeeMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            default:
                break;
        }
    }
    */
    private IEnumerator DownSyringe()
    {
        //if (Syringe.transform.position.y > 0)
        //{
        //    Syringe.transform.position -= new Vector3(0, .5f, 0);
        //    yield return new WaitForSeconds(.1f);
        //    StartCoroutine(DownSyringe());

        //}
        //else
        //{
        //    ChallangeStart = true;
        //    yield return null;
        //}
        while (Syringe.transform.position.y > 0)
        {
            Syringe.transform.position -= new Vector3(0, .5f, 0);
            yield return new WaitForSeconds(.1f);
        }
        ChallangeStart = true;
    }

    private IEnumerator PassSpawner()
    {
        int currentspawn = 0;
        while(currentspawn<5)
        {
            currentspawn++;
            EventManager.TriggerEvent("Spawn");
            yield return new WaitForSeconds(.5f);
        }
    }

    private void MedicineInput()
    {
        if (medicine.transform.localScale.y <= 0.1f)
        {

            Debug.Log("PassStage");
            aIGameManger.DeadAllEnemy();
            //timeline
            IsPass = true;
            stageData.m_datainfolist.stageData.Pass = IsPass;

            particileControl.Play(0, 10, this.transform.position + new Vector3(0, -4.5f, 0));
            if (PlaceObj != null)
            {
                PlaceObj.SetActive(true);
            }

        }
        else if (medicine.transform.localScale.y > 0.1f && !IsPass)
        {
            MedicineIn = false;
            medicine.transform.localScale -= new Vector3(0, .05f, 0);
        }
    }

    public IEnumerator PassStage(float delayTime)
    {
        IsPass = true;
        stageData.m_datainfolist.stageData.Pass = IsPass;
        currentChallange = totalChallange + 1;
        PassChallange = totalChallange;
        yield return new WaitForSeconds(delayTime);
        particileControl.Play(0, 10, this.transform.position + new Vector3(0,8,0) );
        yield return new WaitForSeconds(delayTime);
        aIGameManger.DeadAllEnemy();
        yield return new WaitForSeconds(delayTime);
        Door.SetActive(true);
        yield return new WaitForSeconds(5);
        //if (GameObject.Find("SuccessCanvas")!=null)
        //{
            
        //}
        //else
        //{

        //    sucCanvas = GameObject.Instantiate(Resources.Load<GameObject>("Tutorial/" + "SuccessCanvas"));
        //    sucCanvas.name = "SuccessCanvas";
        //    //Button btn = sucCanvas.GetComponentInChildren<Button>();
        //    //btn.onClick.AddListener(() => DestoryObj(sucCanvas));
        //}
        //yield return new WaitForSeconds(3);
        //CloseGame(sucCanvas);
    }

    //private void DestoryObj(GameObject gameObject)
    //{
    //    Destroy(gameObject);
    //}

    private void CloseGame(GameObject gameObject)
    {
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //        Application.Quit();
        //#endif
        Destroy(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}

#region old
/*
public class StageChallange : MonoBehaviour {

    public bool IsPass;
    public bool ChallangeStart;
    public int stageNum;
    private LoadAndSave stageData;
    private bool CreatDoor;
    [Header("(*必須*)傳送門位置")]
    public Transform StageDoorPos;
    [Header ("傳送門 : ")]
    public GameObject Door;
    [Header("(*必須*)出怪點 : ")]
    public Transform[] CreatPos;
    [Header("(*必須*)怪物名稱 : ")]
    public string[] MonsterType;
    [Header("(*必須*) 針筒")]
    public GameObject Syringe;
    private GameObject medicine;
    public AIGameManger aIGameManger;
    private float currentCreatTime = 0;
    private float CreatTime,NextCreatTime;
    private LoadAndSave stagedata;
    private ParticileControl particileControl;
    private bool MedicineIn;

    public GameObject PlaceObj;

    //水晶
    private int Need = 0;
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        if(!ChallangeStart && !IsPass)
        {
            //if (IsPass)
            //{

            //}
            //else
            //{
            //    CheckInChallangeRange();
            //}
            //CheckInChallangeRange();
        }
        else if(ChallangeStart && !IsPass)
        {
            currentCreatTime += Time.deltaTime;
            if(currentCreatTime>CreatTime)
            {
                int p_num = CreatPos.Length-1;
                Vector3 centerpos = CreatPos[Random.Range(0, p_num)].position;
                int m_num = MonsterType.Length - 1;
                int m_typenum = Random.Range(0, m_num);
                StartChallange(MonsterType[m_typenum],centerpos);
            }
            if(!MedicineIn)
            {
                MedicineIn = true;
                Invoke("MedicineInput", 5f);
            }
            
            
        }
        if(ChallangeStart && IsPass && !CreatDoor )
        {
            CreatDoor = true;
            Door.SetActive(true);
        }
		
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("PassStage");
            aIGameManger.DeadAllEnemy();
            //timeline
            IsPass = true;
            BoxBackageUIControl.Instance.SuccessState();
            stageData.m_datainfolist.stageData.Pass = IsPass;
            particileControl.Play(0, 10, this.transform.position + new Vector3(0,-4.5f,0));
            if(PlaceObj!=null)
            {
                PlaceObj.SetActive(true);
            }
            
        }
	}

    public void Initialized(AIGameManger _aIGameManger,LoadAndSave data)
    {
        IsPass = false;
        //Door = GameObject.FindGameObjectWithTag("StageDoor");
        aIGameManger = _aIGameManger;
        Door = Instantiate(Resources.Load<GameObject>("Door"), StageDoorPos.position, Quaternion.identity);
        Door.GetComponent<DoorInfo>().NextStage = aIGameManger.StageNum + 1;
        Door.SetActive(false);
        Syringe = GameObject.Find("syringe");
        Syringe.transform.position = new Vector3(0, 10, 0) + Syringe.transform.position;
        medicine = Syringe.transform.GetChild(3).gameObject;
        Syringe.SetActive(false);
        stageData = data;
        particileControl = GetComponent<ParticileControl>();

    }
    public void PassInitialized(AIGameManger _aIGameManger,LoadAndSave data)
    {
        IsPass = true;
        //BoxBackageUIControl.Instance.SuccessState();
        aIGameManger = _aIGameManger;
        Door = Instantiate(Resources.Load<GameObject>("Door"), StageDoorPos.position, Quaternion.identity);
        Door.GetComponent<DoorInfo>().NextStage = aIGameManger.StageNum + 1;
        //Door = GameObject.FindGameObjectWithTag("StageDoor");
        Syringe = GameObject.Find("syringe");
        medicine = Syringe.transform.GetChild(3).gameObject;
        medicine.transform.localScale = new Vector3(1, .1f, 1);
        Syringe.SetActive(true);
        stageData = data;
        particileControl = GetComponent<ParticileControl>();
        if (PlaceObj != null)
        {
            PlaceObj.SetActive(true);
        }
    }

    //private void CheckInChallangeRange()
    //{

    //}
    private void StartChallange(string type,Vector3 center)
    {
        currentCreatTime = 0;
        NextCreatTime = Random.Range(1.5f, 3);
        CreatTime = NextCreatTime;
        int nums = Random.Range(2, 5);

        switch(type)
        {
            case "NS":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        EnemyAIStateController ai = new LittleMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            case "CB":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        CombineEnemyAIStateController ai = new CombineMonsterStateController();
                        aIGameManger.AutoCreatCombineRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = CombineEnemyAIStateController.CombineEnemyState.DEFEND;
                    }
                }

                break;

            case "BeeM":
                {
                    for (int i = 0; i < nums; i++)
                    {

                        Vector3 creatpos = center + new Vector3(Random.Range(-20, 20), .2f, Random.Range(-20, 20));
                        EnemyAIStateController ai = new BeeMonsterStateController();
                        aIGameManger.AutoCreatEnemyRole(ai, "Enemy/" + type, creatpos, Quaternion.identity);
                        Vector3 challangepos = GameObject.FindObjectOfType<StageChallange>().transform.position;
                        challangepos.y = 0;
                        ai.targetPos.transform.position = challangepos;
                        ai.m_aiState = EnemyAIStateController.EnemyAIState.DEFEND;
                    }
                }

                break;

            default:
                break;
        }

        
        

    }
    private IEnumerator DownSyringe()
    {
        if(Syringe.transform.position.y>0)
        {
            Syringe.transform.position -= new Vector3(0, .5f, 0);
            yield return new WaitForSeconds(.1f);
            StartCoroutine(DownSyringe());
            
        }
        else
        {
            ChallangeStart = true;
            yield return null;
        }
        
    }

    private void MedicineInput()
    {
        if(medicine.transform.localScale.y<=0.1f)
        {
            
            Debug.Log("PassStage");
            aIGameManger.DeadAllEnemy();
            //timeline
            IsPass = true;
            stageData.m_datainfolist.stageData.Pass = IsPass;
            
            particileControl.Play(0, 10, this.transform.position + new Vector3(0, -4.5f, 0));
            if (PlaceObj != null)
            {
                PlaceObj.SetActive(true);
            }

        }
        else if(medicine.transform.localScale.y > 0.1f&&!IsPass)
        {
            MedicineIn = false;
            medicine.transform.localScale -= new Vector3(0, .05f, 0);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player"&& !ChallangeStart && !IsPass)
        {
            if (Need < BoxBackageUIControl.Instance.TargetNeed)
            {
                for (int i = 0; i < (int)Count.Instance.Crystal; i++)
                {
                    Count.Instance.Crystal -= 1.0f;
                    Need++;
                }
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !ChallangeStart && !IsPass)
        {
            //if (Need < BoxBackageUIControl.Instance.TargetNeed)
            //{
            //    for (int i = 0; i < (int)Count.Instance.Crystal; i++)
            //    {
            //        Count.Instance.Crystal -= 1.0f;
            //        Need++;
            //    }
            //}
            if (Need >= (int)BoxBackageUIControl.Instance.TargetNeed)
            {
                Syringe.SetActive(true);
                StartCoroutine(DownSyringe());
            }
        }
    }
}
*/
#endregion
