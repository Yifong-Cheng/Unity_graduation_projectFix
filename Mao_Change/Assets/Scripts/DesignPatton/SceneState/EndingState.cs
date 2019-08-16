using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingState : ISceneState
{
    private CursorLockMode wantedMode;
    public EndingState(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "EndingState";
    }

    public override void StateBegin()
    {
        gameLoop = GameLoop.FindObjectOfType<GameLoop>();
        gameLoop.CloseLoadScene();

        wantedMode = CursorLockMode.None;
        SetCursorState();
        Button tmpBtn1 = UITool.GetUIComponent<Button>("EndBtn", "Canvas");
        if (tmpBtn1 != null)
        {
            //tmpBtn1.onClick.AddListener(() => OnStartGameBtnClick(tmpBtn1,1));
            tmpBtn1.onClick.AddListener(() => OnEndBtn(tmpBtn1));


        }
    }

    private void OnEndBtn(Button btn)
    {
        btn.gameObject.SetActive(false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
}
