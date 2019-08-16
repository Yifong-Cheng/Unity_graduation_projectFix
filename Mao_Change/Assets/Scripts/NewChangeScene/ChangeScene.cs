using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string SceneName;
	// Use this for initialization
	void Start () {
        StartLoadScene(SceneName);
	}
	
	// Update is called once per frame
	void Update () {
		
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
