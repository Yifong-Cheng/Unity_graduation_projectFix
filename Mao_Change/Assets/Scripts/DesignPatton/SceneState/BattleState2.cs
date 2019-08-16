using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState2 : ISceneState
{
    
    public BattleState2(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "BattleState2";
    }
    //開始
    public override void StateBegin()
    {
        gameLoop = GameObject.FindObjectOfType<GameLoop>();
        MaoGame.Instance.Initinal(2, GameObject.Find("Stage2").GetComponent<LoadAndSave>(), GameObject.FindObjectOfType<StageChallange>(), GameObject.FindObjectOfType<AIGameManger>());
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
        if (MaoGame.Instance.TurnStage == true)
        {
            switch (MaoGame.Instance.NextStage)
            {
                case 1:
                    m_Controller.SetState(new BattleState1(m_Controller), "mao_one",gameLoop);
                    break;
                case 3:
                    m_Controller.SetState(new BattleState3(m_Controller), "mao_three",gameLoop);
                    break;

                default:

                    break;
            }
        }
            
    }

    //---------------------CH04--------------------------
    //遊戲系統

}
