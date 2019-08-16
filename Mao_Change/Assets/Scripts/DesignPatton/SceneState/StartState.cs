using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : ISceneState
{
    public StartState(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "StartState";
    }

    //開始
    public override void StateBegin()
    {

        //在此進行遊戲資料載入及初始化設定等等
        gameLoop = GameObject.FindObjectOfType<GameLoop>();
        gameLoop.CloseLoadScene();
    }
    //更新
    public override void StateUpdate()
    {
        //更換為 -> MainMenuState狀態，與MainMenuScene場景
        m_Controller.SetState(new MainMenuState(m_Controller), "MainMenuScene_1",gameLoop);
        //m_Controller.SetState(new MainMenuState(m_Controller), 1, gameLoop);
    }

}
