using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState1 : ISceneState
{
    
    public BattleState1(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "BattleState1";
    }
    //開始
    public override void StateBegin()
    {
        gameLoop = GameObject.FindObjectOfType<GameLoop>();
        gameLoop.CloseLoadScene();
        MaoGame.Instance.Initinal(1, GameObject.Find("Stage1").GetComponent<LoadAndSave>(),GameObject.FindObjectOfType<StageChallange>(), GameObject.FindObjectOfType<AIGameManger>());
    }

    //結束
    public override void StateEnd()
    {
        
    }

    public override void StateUpdate()
    {
        //輸入
        InputProcess();

        //遊戲邏輯
        MaoGame.Instance.Update();

        //Render由Unity負責

        //遊戲是否結束
        
    }

    private void InputProcess()
    {
        //玩家輸入判斷程式碼...
        if(MaoGame.Instance.TurnStage==true)
        {
            switch(MaoGame.Instance.NextStage)
            {
                case 2:
                    m_Controller.SetState(new BattleState2(m_Controller), "mao_two", gameLoop);
                    //m_Controller.SetState(new EndingState(m_Controller), "EndScene",gameLoop);
                    //m_Controller.SetState(new EndingState(m_Controller), 3, gameLoop);
                    break;
                case 0:
                    //m_Controller.SetState(new MainMenuState(m_Controller), 1,gameLoop);
                    //m_Controller.SetState(new MainMenuState(m_Controller), "MainMenuScene", gameLoop);
                    break;
                default:

                    break;
            }
        }
    }

    //---------------------CH04--------------------------
    //遊戲系統

}
