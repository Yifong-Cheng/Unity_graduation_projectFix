using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManger : MonoBehaviour {
    [Header("基礎毛位置")]
    public Transform[] S0MaoTree;
    [Header("躁急毛位置")]
    public Transform[] S1MaoTree;
    [Header("炸彈毛位置")]
    public Transform[] S2MaoTree;
    [Header("頭角位置")]
    public Transform[] S3MaoTree;
    [Header("壯壯毛位置")]
    public Transform[] S4MaoTree;
    [Header("毛孔位置")]
    public List<HoleControl> MaoHole;
    public List<MaoTreeControl> MaoTree;
    private LoadAndSave Data;

    public Transform Player;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        CheckPlarerInRange();

    }

    public void Initialized(LoadAndSave _data,Transform _Player)
    {
        Data = _data;
        Data.CreatHoleModel(this);
        Data.CreatTreeModel(this);

        Player = _Player;
        foreach (MaoTreeControl tree in MaoTree)
        {
            tree.Player = Player.gameObject;
        }

        foreach (HoleControl hole in MaoHole)
        {
            hole.Player = Player.gameObject;
            if(hole.IsOn)
            {
                hole.tree.Player = Player.gameObject;
            }
        }
    }

    public void CheckPlarerInRange()
    {
        foreach(MaoTreeControl tree in MaoTree)
        {
            if(Vector3.Distance(tree.Art.transform.position, Player.transform.position) < 5)
            {
                tree.NearPlayer = true;
            }
            else
            {
                tree.NearPlayer = false;
            }
        }

        foreach(HoleControl hole in MaoHole)
        {
            if(hole.IsOn)
            {
                if (Player.GetComponent<NewPlayerController>().holeList.Contains(hole))
                {
                    hole.holeTutorial.SetActive(false);
                    Player.GetComponent<NewPlayerController>().holeList.Remove(hole);
                }
                MaoTreeControl tree = hole.tree;
                if (Vector3.Distance(tree.Art.transform.position, Player.transform.position) < 5&&tree.CreatFinish)
                {
                    tree.NearPlayer = true;
                }
                else
                {
                    tree.NearPlayer = false;
                }
            }
            else
            {
                if(Vector3.Distance(hole.gameObject.transform.position, Player.transform.position) < 3 )
                {
                    hole.NearPlayer = true;
                    //Player.GetComponent<NewPlayerController>().NearHole = true;
                    if(!Player.GetComponent<NewPlayerController>().holeList.Contains(hole))
                    {
                        Player.GetComponent<NewPlayerController>().holeList.Add(hole);
                        hole.holeTutorial.SetActive(true);
                    }
                    else
                    {

                    }
                    
                }
                else
                {
                    hole.NearPlayer = false;
                    hole.holeTutorial.SetActive(false);
                    //Player.GetComponent<NewPlayerController>().NearHole = false;
                    if (Player.GetComponent<NewPlayerController>().holeList.Contains(hole))
                    {
                        Player.GetComponent<NewPlayerController>().holeList.Remove(hole);
                    }
                }
            }
        }
    }
   

    public void CreatMaoTree(Vector3 t, string Sln,string _type,StageMaoTreeInfo treeinfo)
    {
        GameObject go  = Instantiate(Resources.Load<GameObject>(Sln + "MaoTree"), t, Quaternion.identity);
        go.GetComponent<MaoTreeControl>().Art = GameObject.Instantiate(Resources.Load<GameObject>(Sln + _type), go.transform.position, Quaternion.identity, go.transform);
        go.GetComponent<MaoTreeControl>().Art.transform.localScale = new Vector3(.5f, .5f, .5f);
        go.GetComponent<MaoTreeControl>().Initialized(_type);
        go.GetComponent<MaoTreeControl>().growMaoControl.waitTime = treeinfo.WaitTime;
        
    }
    public GameObject CreatMaoTree(Vector3 t, string _type, StageMaoTreeInfo treeinfo)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("MaoTree/" + "MaoTree"), t, Quaternion.identity);
        go.GetComponent<MaoTreeControl>().Art = GameObject.Instantiate(Resources.Load<GameObject>("MaoTree/" + _type), go.transform.position, Quaternion.identity, go.transform);
        
        go.GetComponent<MaoTreeControl>().Initialized(_type);
        go.GetComponent<MaoTreeControl>().growMaoControl.waitTime = treeinfo.WaitTime;
        go.GetComponent<MaoTreeControl>().growMaoControl.growState = treeinfo.State;
        if (treeinfo.State == 0)
        {
            go.GetComponent<MaoTreeControl>().Art.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
        return go;
    }
    public void TurnStageSave()
    {
        for(int i=0;i<MaoHole.Count;i++)
        {
            Data.m_datainfolist.stageMaoHoleDataList[i].IsOn = MaoHole[i].GetComponent<HoleControl>().IsOn;
            if (MaoHole[i].GetComponent<HoleControl>().IsOn)
            {
                Data.m_datainfolist.stageMaoHoleDataList[i].treeInfo.Type = MaoHole[i].GetComponent<HoleControl>().Type;
                Data.m_datainfolist.stageMaoHoleDataList[i].treeInfo.State = MaoHole[i].GetComponent<HoleControl>().tree.growMaoControl.growState;
                Data.m_datainfolist.stageMaoHoleDataList[i].treeInfo.CurrentWaitTime = MaoHole[i].GetComponent<HoleControl>().tree.growMaoControl.currentTime;
            }
        }

        for(int i=0;i<MaoTree.Count;i++)
        {
            Data.m_datainfolist.stageTreeDataList[i].CurrentWaitTime = MaoTree[i].GetComponent<MaoTreeControl>().growMaoControl.currentTime;
            Data.m_datainfolist.stageTreeDataList[i].WaitTime = MaoTree[i].GetComponent<MaoTreeControl>().growMaoControl.waitTime;
            Data.m_datainfolist.stageTreeDataList[i].State = MaoTree[i].GetComponent<MaoTreeControl>().growMaoControl.growState;
            Debug.Log("savestate : - " + Data.m_datainfolist.stageTreeDataList[i].State);
        }
    }
}
