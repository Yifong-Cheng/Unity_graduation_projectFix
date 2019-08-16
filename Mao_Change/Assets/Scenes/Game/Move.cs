using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour {
    [Header("這是要位移的方向")]
    public bool x, y, z;

    [Header("位移的量")]
    [Range(20,100)]
    public float boostrange = 1;
    [Header("指定位移量")]
    public Vector3 direction = new Vector3(0, 0, 0);
    private Vector3 movedirection;
    // Use this for initialization
    private MainMenuState scenestate;
    private Button btn;
    [SerializeField]
    private float currenttime = 0;

    public void Start()
    {
        Time.timeScale = 1;
    }

    public void Init(MainMenuState _scenestate,Button _btn)
    {
        movedirection = (new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0) + direction) * boostrange * boostrange;
        scenestate = _scenestate;
        btn = _btn;
        currenttime = 0;
        //StopAllCoroutines();
    }

    //private void Update()
    //{
    //    if(Input.anyKeyDown&&!StartChange)
    //    {
    //        StartChange = true;
            
    //    }

    //    if(StartChange&&!ChangeFinish)
    //    {
    //        currenttime += Time.deltaTime;
    //        this.transform.position += movedirection * Time.deltaTime;
    //    }
        
    //    if(currenttime>3&&!ChangeFinish)
    //    {
    //        ChangeFinish = true;
    //        scenestate.StartGame();
    //    }
    //}

    public void ClickBtn()
    {
        Debug.Log("Say Yo!");
        StartCoroutine(ImgRunStartGame());
        //StartGame();
        Debug.LogWarning("Say Hellow");
    }

    private IEnumerator ImgRunStartGame()
    {
        btn.gameObject.SetActive(false);
        
        while(currenttime<3)
        {
            currenttime += Time.deltaTime;
            this.transform.position += movedirection * Time.deltaTime;
            yield return null;
        }

        scenestate.StartGame();
    }

    private void StartGame()
    {
        
        btn.gameObject.SetActive(false);
        scenestate.StartGame();
    }
}
