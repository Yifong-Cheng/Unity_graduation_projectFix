using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStateController
{

    private ISceneState m_State;
    private bool m_bRunBegin = false;

    public SceneStateController() { }

    private float SceneLoadProcess;
    GameLoop gameLoop;

    public void SetState(ISceneState State, string LoadSceneName,GameLoop _gameLoop)
    {
        Debug.Log("SetState:" + State.ToString());
        m_bRunBegin = false;
        //0430
        gameLoop = _gameLoop;


        //載入場景
        LoadScene(LoadSceneName);
        //gameLoop.StartLoadScene(LoadSceneName);
        //gameLoop.StartCoroutine(LoadAsynchronously(LoadSceneName, State));

        //通知前一個State結束
        if (m_State != null)
        {
            m_State.StateEnd();
        }

        //設定
        m_State = State;
        //gameLoop.StartCoroutine(CheckLoadFinish(State));
    }

    //載入場景
    private void LoadScene(string LoadSceneName)
    {
        if (LoadSceneName == null || LoadSceneName.Length == 0)
        {
            return;
        }
        Application.LoadLevel(LoadSceneName);
    }

    private IEnumerator CheckLoadFinish(ISceneState State)
    {
        while(SceneLoadProcess<1)
        {
            yield return null;
        }
        if (m_State != null)
        {
            m_State.StateEnd();
        }

        //設定
        m_State = State;
    }

    //GameObject loadingscreen;
    //Slider slider;

    //private IEnumerator LoadAsynchronously(string LoadSceneName)
    //{
    //    loadingscreen.SetActive(true);
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(LoadSceneName);
    //    while(!operation.isDone)
    //    {
    //        float process = Mathf.Clamp01(operation.progress / .9f);
    //        slider.value = process;
    //        yield return null;
    //    }
    //}

    private IEnumerator LoadAsynchronously(string LoadSceneName, ISceneState State)
    {
        if (LoadSceneName != "")
        {
            gameLoop.loadingscreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(LoadSceneName);
            while (!operation.isDone)
            {
                float process = Mathf.Clamp01(operation.progress / .9f);
                gameLoop.imgBar.fillAmount = process;
                yield return null;
            }

            if (m_State != null)
            {
                m_State.StateEnd();
            }

            //設定
            m_State = State;
        }
        else
        {
            yield return null;
            if (m_State != null)
            {
                m_State.StateEnd();
            }

            //設定
            m_State = State;
        }
            


    }


    //
    public void StateUpdate()
    {
        //是否還在載入
        if (Application.isLoadingLevel)
            return;

        //通知新的State開始
        if (m_State != null && m_bRunBegin == false)
        {
            m_State.StateBegin();
            m_bRunBegin = true;
        }
        if (m_State != null)
            m_State.StateUpdate();
    }

}