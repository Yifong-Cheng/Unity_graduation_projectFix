using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour {

    SceneStateController m_SceneStateController = new SceneStateController();
    private void Awake()
    {
        //轉換場景不被刪除
        GameObject.DontDestroyOnLoad(this.gameObject);

        //亂樹種子
        UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
    }

    private void Start()
    {
        //設定起始場景
        m_SceneStateController.SetState(new StartState(m_SceneStateController), "",this);
        //m_SceneStateController.SetState(new StartState(m_SceneStateController), 1, this);

    }

    private void Update()
    {
        m_SceneStateController.StateUpdate();
    }

    //0430
    public void StartLoadScene(string LoadSceneName)
    {
        StartCoroutine(LoadAsynchronously(LoadSceneName));

    }

    public GameObject loadingscreen;
    public Image imgBar;

    public void CloseLoadScene()
    {
        imgBar.fillAmount = 0;
        loadingscreen.SetActive(false);
    }

    private IEnumerator LoadAsynchronously(string LoadSceneName)
    {
        if (LoadSceneName != "")
        {
            loadingscreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(LoadSceneName);
            while (!operation.isDone)
            {
                float process = Mathf.Clamp01(operation.progress / .9f);
                imgBar.fillAmount = process;
                yield return null;
            }
        }
        else
            yield return null;
        
    }
}
