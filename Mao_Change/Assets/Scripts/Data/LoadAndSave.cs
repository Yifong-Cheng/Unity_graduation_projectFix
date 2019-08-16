using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class DataInfoList
{
    public List<DataInfo> dataList = new List<DataInfo>();
    public List<StageMonsterDataInfo> monsterdataList = new List<StageMonsterDataInfo>();
    public PlayerData playerdata = new PlayerData();
    public StageInfo stageData = new StageInfo();
    public List<StageMaoHoleInfo> stageMaoHoleDataList = new List<StageMaoHoleInfo>();
    public List<StageMaoTreeInfo> stageTreeDataList = new List<StageMaoTreeInfo>();
}

public class MaoHoleDataInfoList
{
    public List<MaoHoleInfo> holeDataList = new List<MaoHoleInfo>();
}

public class MaoTreeDataInfoList
{
    public List<MaoTreeInfo> treeDataList = new List<MaoTreeInfo>();
}

public class MonsterDataInfoList
{
    public List<MonsterDataInfo> monsterDataList = new List<MonsterDataInfo>();
}

public class DataInfo
{
    public Vector3 LastPos;
    public string name = "";
    public int currentHp;
}

public class PlayerData
{
    public Vector3 LastPos = new Vector3(55,0.15f,-6);
}

public class MonsterDataInfo
{
    public double X;
    public double Y;
    public double Z;
    public string Type;
    public int currentHp;
}

public class StageMonsterDataInfo
{
    public Vector3 LastPos;
    public string Type = "";
    public int currentHp;
}

public class StageInfo
{
    public bool Pass;
    public int RunTime = 0;
}

public class MaoTreeInfo
{
    public double X;
    public double Y;
    public double Z;
    public string Type;
}
public class StageMaoTreeInfo
{
    public bool IsFirstTime;
    public float WaitTime;
    public float CurrentWaitTime;
    public int State;
    public string Type;
    public Vector3 Pos;
    
}

public class MaoHoleInfo
{
    //public Vector3 Pos;
    public double X;
    public double Y;
    public double Z;
    public int IsOn;
    
}
public class StageMaoHoleInfo
{
    public Vector3 Pos;
    public bool IsOn;
    public StageMaoTreeInfo treeInfo = new StageMaoTreeInfo();
}

public class LoadAndSave : MonoBehaviour
{
    [Header("(*必須/重要*)"+"關卡起始資訊檔案 - 孔")]
    public TextAsset StageFile;
    [Header("(*必須/重要*)" + "關卡起始資訊檔案 - 樹")]
    public TextAsset TreeStageFile;
    [Header("(*必須/重要*)" + "關卡起始資訊檔案 - 怪物")]
    public TextAsset MonsterStageFile;
    public DataInfoList m_datainfolist;
    public MaoHoleDataInfoList holeDataList = new MaoHoleDataInfoList();
    public MaoTreeDataInfoList treeDataList = new MaoTreeDataInfoList();
    public MonsterDataInfoList monsterDataList = new MonsterDataInfoList();
    public Vector3 PlayerCreatPos;

    private void Awake()
    {
        m_datainfolist = new DataInfoList();
        DontDestroyOnLoad(this);
        InitializedHolePos();
        InitializedTreeData();
        InitializedMonsterData();
        m_datainfolist.playerdata.LastPos = PlayerCreatPos;
    }

    private void Start()
    {
        //CreatHoleData();
    }

    private void Update()
    {
        foreach (StageMaoTreeInfo tree in m_datainfolist.stageTreeDataList)
        {
            if (tree.State == 0)
            {
                tree.CurrentWaitTime += Time.deltaTime;
                if (tree.CurrentWaitTime > tree.WaitTime)
                    tree.State = 1;
            }
        }
        foreach (StageMaoHoleInfo hole in m_datainfolist.stageMaoHoleDataList)
        {
            if (hole.IsOn)
            {
                if (hole.treeInfo.State == 0)
                {
                    hole.treeInfo.CurrentWaitTime += Time.deltaTime;
                    if (hole.treeInfo.CurrentWaitTime > hole.treeInfo.WaitTime)
                        hole.treeInfo.State = 1;
                }
            }
        }
    }

    public void LoadSoilderData()
    {
        AIGameManger aIGameManger = GameObject.Find("aiGameManger").GetComponent<AIGameManger>();
        CreatSoilderModel(aIGameManger);
    }

    public void LoadEnemyData()
    {
        Debug.Log("Data - LoadEnemy");
        AIGameManger aIGameManger = GameObject.Find("aiGameManger").GetComponent<AIGameManger>();
        CreatEnemyModel(aIGameManger);
    }

    


    public void InitializedHolePos()
    {
        holeDataList = JsonMapper.ToObject<MaoHoleDataInfoList>(StageFile.text);
        for(int i=0;i<holeDataList.holeDataList.Count;i++)
        {
            StageMaoHoleInfo stageMaoHoleInfo = new StageMaoHoleInfo();
            stageMaoHoleInfo.IsOn = false;
            stageMaoHoleInfo.treeInfo = new StageMaoTreeInfo();
            stageMaoHoleInfo.Pos = new Vector3((float)holeDataList.holeDataList[i].X, (float)holeDataList.holeDataList[i].Y, (float)holeDataList.holeDataList[i].Z);
            stageMaoHoleInfo.treeInfo.WaitTime = 20f;
            m_datainfolist.stageMaoHoleDataList.Add(stageMaoHoleInfo);
        }
    }

    public void InitializedTreeData()
    {
        treeDataList = JsonMapper.ToObject<MaoTreeDataInfoList>(TreeStageFile.text);
        for (int i = 0; i < treeDataList.treeDataList.Count; i++)
        {
            StageMaoTreeInfo stageMaoTreeInfo = new StageMaoTreeInfo
            {
                Pos = new Vector3(
                    (float)treeDataList.treeDataList[i].X,
                    (float)treeDataList.treeDataList[i].Y,
                    (float)treeDataList.treeDataList[i].Z),
                Type = treeDataList.treeDataList[i].Type,
                WaitTime = 20f
            };
            m_datainfolist.stageTreeDataList.Add(stageMaoTreeInfo);
        }
    }

    public void InitializedMonsterData()
    {
        monsterDataList = JsonMapper.ToObject<MonsterDataInfoList>(MonsterStageFile.text);
        for(int i=0;i<monsterDataList.monsterDataList.Count;i++)
        {
            StageMonsterDataInfo stageMonsterInfo = new StageMonsterDataInfo()
            {
                LastPos = new Vector3((float)monsterDataList.monsterDataList[i].X, (float)monsterDataList.monsterDataList[i].Y, (float)monsterDataList.monsterDataList[i].Z),
                Type = monsterDataList.monsterDataList[i].Type,
                currentHp = monsterDataList.monsterDataList[i].currentHp
            };
            m_datainfolist.monsterdataList.Add(stageMonsterInfo);
        }
        //Debug.Log(monsterDataList.monsterDataList.Count);
        //Debug.Log(m_datainfolist.monsterdataList.Count);
    }

    #region creatjson
    public void CreatHoleData()
    {
        CreatFile(Application.persistentDataPath, "Stage1Data.txt");
    }
    void CreatFile(string path, string name)
    {
        MaoHoleInfo maoHoleInfo = new MaoHoleInfo();
        //maoHoleInfo.X = 15;
        //maoHoleInfo.Y = .2;
        //maoHoleInfo.Z = 30;
        //maoHoleInfo.IsOn = false;
        //maoHoleInfo.treeInfo = new MaoTreeInfo();
        holeDataList.holeDataList.Add(maoHoleInfo);
        holeDataList.holeDataList.Add(maoHoleInfo);
        holeDataList.holeDataList.Add(maoHoleInfo);
        string info;
        StreamWriter file;
        FileInfo fileInfo = new FileInfo(path + "//" + name);
        info = JsonMapper.ToJson(holeDataList);
        if (!fileInfo.Exists)
        {
            file = fileInfo.CreateText();
            file.WriteLine(info);
        }
        else
        {
            file = fileInfo.AppendText();
        }
        file.Close();
        file.Dispose();
    }
    #endregion
    public void LoadHoleData()
    {
        TreeManger treeManger = GameObject.FindObjectOfType<TreeManger>();
    }

    public void CreatHoleModel(TreeManger treeManger)
    {
        for(int i=0;i< m_datainfolist.stageMaoHoleDataList.Count;i++)
        {
            HoleControl go = GameObject.Instantiate(Resources.Load<GameObject>("MaoHole/"+"MaoHole"),m_datainfolist.stageMaoHoleDataList[i].Pos , Quaternion.identity).GetComponent<HoleControl>();
            go.GetComponent<HoleControl>().data = this;
            GameObject tree;
            if (m_datainfolist.stageMaoHoleDataList[i].IsOn)
            {
                go.GetComponent<HoleControl>().IsOn = true;
                go.GetComponent<HoleControl>().Type = m_datainfolist.stageMaoHoleDataList[i].treeInfo.Type;
                go.GetComponent<BoxCollider>().enabled = false;
                //treeManger.CreatMaoTree(m_datainfolist.stageMaoHoleDataList[i].Pos,"MaoTree/", m_datainfolist.stageMaoHoleDataList[i].treeInfo.Type, m_datainfolist.stageMaoHoleDataList[i].treeInfo);
                tree =treeManger.CreatMaoTree(m_datainfolist.stageMaoHoleDataList[i].Pos, m_datainfolist.stageMaoHoleDataList[i].treeInfo.Type, m_datainfolist.stageMaoHoleDataList[i].treeInfo);
                tree.GetComponent<MaoTreeControl>().growMaoControl.currentTime = m_datainfolist.stageMaoHoleDataList[i].treeInfo.CurrentWaitTime;
                tree.GetComponent<MaoTreeControl>().data = this;
                tree.GetComponent<MaoTreeControl>().growMaoControl.waitTime = m_datainfolist.stageMaoHoleDataList[i].treeInfo.WaitTime;
                tree.GetComponent<MaoTreeControl>().growMaoControl.growState = m_datainfolist.stageMaoHoleDataList[i].treeInfo.State;
                if (m_datainfolist.stageMaoHoleDataList[i].treeInfo.State == 0)
                {
                    tree.GetComponent<MaoTreeControl>().Art.transform.localScale = new Vector3(.5f, .5f, .5f);
                }
                go.GetComponent<HoleControl>().tree = tree.GetComponent<MaoTreeControl>();
            }
            treeManger.MaoHole.Add(go);
        }
    }

    public void CreatTreeModel(TreeManger treeManger)
    {
        for(int i=0;i<m_datainfolist.stageTreeDataList.Count;i++)
        {
            MaoTreeControl go = GameObject.Instantiate(Resources.Load<GameObject>("MaoTree/" + "MaoTree"), m_datainfolist.stageTreeDataList[i].Pos, Quaternion.identity).GetComponent<MaoTreeControl>();
            go.GetComponent<MaoTreeControl>().Art = GameObject.Instantiate(Resources.Load<GameObject>("MaoTree/" + m_datainfolist.stageTreeDataList[i].Type), go.transform.position, Quaternion.identity, go.transform);
            go.GetComponent<MaoTreeControl>().Initialized(m_datainfolist.stageTreeDataList[i].Type);
            go.GetComponent<MaoTreeControl>().data = this;
            go.GetComponent<MaoTreeControl>().growMaoControl.waitTime = m_datainfolist.stageTreeDataList[i].WaitTime;
            //go.GetComponent<MaoTreeControl>().growMaoControl.growState = 1;
            go.GetComponent<MaoTreeControl>().growMaoControl.growState = m_datainfolist.stageTreeDataList[i].State;
            if (!m_datainfolist.stageTreeDataList[i].IsFirstTime)
            {
                m_datainfolist.stageTreeDataList[i].IsFirstTime = true;
                go.GetComponent<MaoTreeControl>().growMaoControl.growState = 1;
                go.GetComponent<MaoTreeControl>().growMaoControl.currentTime = 18;
            }
            else
            {
                go.GetComponent<MaoTreeControl>().growMaoControl.currentTime = m_datainfolist.stageTreeDataList[i].CurrentWaitTime;
                if(m_datainfolist.stageTreeDataList[i].State==0)
                {
                    go.GetComponent<MaoTreeControl>().Art.transform.localScale = new Vector3(.5f, .5f, .5f);
                }
            }
            //Debug.Log("Creatstate : - " +  m_datainfolist.stageTreeDataList[i].State);
            treeManger.MaoTree.Add(go);
        }
    }

    private void CreatSoilderModel(AIGameManger _aIGameManger)
    {
        for (int i = 0; i < m_datainfolist.dataList.Count; i++)
        {
            SoilderAIStateController ai;
            Quaternion spawnRotation = new Quaternion();
            spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
            switch (m_datainfolist.dataList[i].name)
            {
                case ("S0"):
                    ai = new LittleMaoStateController();
                    
                    _aIGameManger.CreatSoilderRole(ai, "Props/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;

                case ("S1"):
                    ai = new SummonMaoStateController();
                    
                    _aIGameManger.CreatSoilderRole(ai, "Props/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;


                case ("S2"):
                    ai = new BombMaoStateController();
                    _aIGameManger.CreatSoilderRole(ai, "Props/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;


                case ("S3"):
                    ai = new HeadMaoStateController();
                    _aIGameManger.CreatSoilderRole(ai, "Props/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;


                case ("S4"):
                    ai = new BattleMaoStateController();
                    _aIGameManger.CreatSoilderRole(ai, "Props/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;

                default:
                    break;
            }
        }
    }

    private void CreatEnemyModel(AIGameManger _aIGameManger)
    {
        Debug.Log("data - createnemy");
        for (int i = 0; i < m_datainfolist.monsterdataList.Count; i++)
        {
            EnemyAIStateController ai;
            CombineEnemyAIStateController combine_ai;
            Quaternion spawnRotation = new Quaternion();
            spawnRotation.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f));
            switch (m_datainfolist.monsterdataList[i].Type)
            {
                case ("NS"):
                    ai = new LittleMonsterStateController();
                    _aIGameManger.CreatEnemyRole(ai, "Enemy/" + m_datainfolist.monsterdataList[i].Type, m_datainfolist.monsterdataList[i].LastPos, spawnRotation);
                    ai.p_health.currenthp = m_datainfolist.monsterdataList[i].currentHp;
                    break;

                case ("CB"):
                    combine_ai = new CombineMonsterStateController();
                    _aIGameManger.CreatCombineEnemyRole(combine_ai, "Enemy/" + m_datainfolist.monsterdataList[i].Type, m_datainfolist.monsterdataList[i].LastPos, spawnRotation);
                    combine_ai.p_health.currenthp = m_datainfolist.monsterdataList[i].currentHp;
                    if(m_datainfolist.monsterdataList[i].currentHp>30)
                    {
                        combine_ai.ChangeModel();
                    }
                    break;


                case ("CA"):
                    combine_ai = new CombineMonsterStateController();
                    _aIGameManger.CreatCombineEnemyRole(combine_ai, "Enemy/" + m_datainfolist.dataList[i].name, m_datainfolist.dataList[i].LastPos, spawnRotation);
                    combine_ai.Role.GetComponent<PlayerHealth>().currenthp = m_datainfolist.dataList[i].currentHp;
                    break;

                default:
                    break;
            }
        }
    }

    public void SaveAIData(Vector3 lastpos,string name,int currenthp)
    {
        DataInfo data = new DataInfo();
        data.LastPos = lastpos;
        data.name = name;
        data.currentHp = currenthp;

        m_datainfolist.dataList.Add(data);
    }
    public void SaveMonsterAIData(Vector3 lastpos, string _type,int currenthp)
    {
        StageMonsterDataInfo data = new StageMonsterDataInfo();
        data.LastPos = lastpos;
        data.Type = _type;
        data.currentHp = currenthp;

        m_datainfolist.monsterdataList.Add(data);

    }

}
