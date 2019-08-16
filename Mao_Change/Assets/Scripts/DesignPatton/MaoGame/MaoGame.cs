using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaoGame {

    private static MaoGame instance;
    public static MaoGame Instance
    {
        get
        {
            if (instance == null)
                instance = new MaoGame();
            return instance;
        }
    }

    private MaoGame()
    { }

    //AIStateController aIStateController = null;
    AvaterController m_Controller = null;
    //CharacterControlMove m_Controller;
    public bool TurnStage;
    public int NextStage;

    GameObject Player;
    // Use this for initialization
    public void Initinal(int stageNum, LoadAndSave _PlayerData,StageChallange stageChallange,AIGameManger _aIGameManger)
    {
        ////aIStateController = new AIStateController();
        //m_Controller = new AvaterController();
        ////m_Controller = new CharacterControlMove();
        ////m_Controller.Player = GameObject.Find("Player");
        //m_Controller.Player =GameObject.Instantiate( Resources.Load<GameObject>("Player"), _PlayerData.m_datainfolist.playerdata.LastPos,Quaternion.identity);
        //m_Controller.Player.name = "Player";
        //m_Controller.Initialized();
        //m_Controller.Player.GetComponent<NewPlayerController>().maogame = this;
        //m_Controller.Player.GetComponent<NewPlayerController>()._data = _PlayerData;
        //TurnStage = false;
        //GameObject.FindObjectOfType<CameraFollow>().Player = m_Controller.Player;
        //if(!_PlayerData.m_datainfolist.stageData.Pass)
        //{
        //    //creat door or sendmessange to scene obj
        //    GameObject.FindObjectOfType<StageChallange>().Initialized(_aIGameManger,_PlayerData);
        //}
        //else
        //{
        //    //
        //    GameObject.FindObjectOfType<StageChallange>().PassInitialized(_aIGameManger,_PlayerData);

        //}
        //if(GameObject.FindObjectOfType<teach>() != null)
        //{
        //    GameObject.FindObjectOfType<teach>().Player = m_Controller.Player;
        //}
        ////------
        //TreeManger treeManger = GameObject.FindObjectOfType<TreeManger>();
        //treeManger.Initialized(_PlayerData,m_Controller.Player.transform);

        //if(GameObject.FindObjectOfType<NewMiniMap>()!=null)
        //{
        //    NewMiniMap minimap = GameObject.FindObjectOfType<NewMiniMap>();
        //    minimap.InitializedMapRange(m_Controller.Player);
        //}
        //aIStateController = new AIStateController();



        //Player = GameObject.Instantiate(Resources.Load<GameObject>("Player"), _PlayerData.m_datainfolist.playerdata.LastPos, Quaternion.identity);
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.name = "Player";
        
        Player.GetComponent<NewPlayerController>().maogame = this;
        Player.GetComponent<NewPlayerController>()._data = _PlayerData;
        TurnStage = false;
        GameObject.FindObjectOfType<CameraFollow>().Player = Player;
        if (!_PlayerData.m_datainfolist.stageData.Pass)
        {
            //creat door or sendmessange to scene obj
            GameObject.FindObjectOfType<StageChallange>().Initialized(_aIGameManger, _PlayerData);
        }
        else
        {
            //
            GameObject.FindObjectOfType<StageChallange>().PassInitialized(_aIGameManger, _PlayerData);
            Player.transform.position = _PlayerData.m_datainfolist.playerdata.LastPos;

        }
        if (GameObject.FindObjectOfType<teach>() != null)
        {
            GameObject.FindObjectOfType<teach>().Player = Player;
        }
        //------
        TreeManger treeManger = GameObject.FindObjectOfType<TreeManger>();
        treeManger.Initialized(_PlayerData, Player.transform);

        if (GameObject.FindObjectOfType<NewMiniMap>() != null)
        {
            NewMiniMap minimap = GameObject.FindObjectOfType<NewMiniMap>();
            minimap.InitializedMapRange(Player);
        }
    }
	
	// Update is called once per frame
	public void Update () {
        //m_Controller.Update();
	}

    public void InputPocess()
    {

    }
}
