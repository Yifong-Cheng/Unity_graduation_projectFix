using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : ISceneState
{
    Move m = null;
    public MainMenuState(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "MainMenuState";
    }

    public override void StateBegin()
    {
        gameLoop = GameObject.FindObjectOfType<GameLoop>();
        gameLoop.CloseLoadScene();
        Button tmpBtn1 = UITool.GetUIComponent<Button>("GameStartBtn1", "Canvas");
        m = GameObject.FindObjectOfType<Move>();
        m.Init(this,tmpBtn1);
        //取得開始按鈕
        //Button tmpBtn1 = UITool.GetUIComponent<Button>("GameStartBtn1", "Canvas");
        //if (tmpBtn1 != null)
        //{
        //    //tmpBtn1.onClick.AddListener(() => OnStartGameBtnClick(tmpBtn1,1));
        //    tmpBtn1.onClick.AddListener(() => OnStartBtn(tmpBtn1));


        //}
        //Button tmpBtn2 = UITool.GetUIComponent<Button>("GameStartBtn2", "Canvas");
        //if (tmpBtn2 != null)
        //{
        //    tmpBtn2.onClick.AddListener(() => OnStartGameBtnClick(tmpBtn2, 2));
        //}
    }

    public override void StateUpdate()
    {
        //base.StateUpdate();
        //輸入
        InputProcess();
    }
    float currenttime;
    float Endtime = 3;
    bool startchange;
    bool stagechange;

    private void InputProcess()
    {
        //if(startchange)
        //{
        //    currenttime += Time.deltaTime;
        //    if (currenttime > Endtime && !stagechange)
        //    {
                
        //        stagechange = true;
        //        m_Controller.SetState(new BattleState1(m_Controller), "mao_one");
        //        //m_Controller.SetState(new BattleState1(m_Controller), "TestScene");
        //        //stagechange = true;
        //    }
        //}
        

    }

    public void StartGame()
    {
        m_Controller.SetState(new BattleState1(m_Controller), "mao_one", gameLoop);
        //m_Controller.SetState(new BattleState1(m_Controller), 2, gameLoop);
    }

    //private void OnStartBtn(Button theButton)
    //{
    //    GameObject.FindObjectOfType<Move>().enabled = true;
    //    theButton.gameObject.SetActive(false);
    //    startchange = true;
    //}

    //// 開始戰鬥
    //private void OnStartGameBtnClick(Button theButton,int StageID)
    //{
    //    //Debug.Log ("OnStartBtnClick:"+theButton.gameObject.name);
    //    //m_Controller.SetState(new BattleState(m_Controller), "BattleScene");
    //    switch (StageID)
    //    {
    //        case 1:
    //            //m_Controller.SetState(new BattleState1(m_Controller), "MapScene");
    //            //m_Controller.SetState(new BattleState1(m_Controller), "TestScene", gameLoop);
    //            //m_Controller.SetState(new BattleState1(m_Controller), "mao_one");
    //            //m_Controller.SetState(new BattleState1(m_Controller), "TestScene1");
    //            //m_Controller.SetState(new BattleState1(m_Controller), "mao_three");

    //            break;

    //        case 2:
    //            //m_Controller.SetState(new BattleState2(m_Controller), "GameStage2", gameLoop);
    //            break;

    //        default:
    //            break;
    //    }

    //}

}
