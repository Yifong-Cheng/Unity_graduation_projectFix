using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState3 : ISceneState
{
    
    public BattleState3(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "BattleState3";
    }

    //開始
    public override void StateBegin()
    {
        gameLoop = GameObject.FindObjectOfType<GameLoop>();
        MaoGame.Instance.Initinal(2, GameObject.Find("Stage3").GetComponent<LoadAndSave>(), GameObject.FindObjectOfType<StageChallange>(), GameObject.FindObjectOfType<AIGameManger>());
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
                case 2:
                    m_Controller.SetState(new BattleState2(m_Controller), "mao_two",gameLoop);
                    break;
                

                default:

                    break;
            }
        }

    }

}
