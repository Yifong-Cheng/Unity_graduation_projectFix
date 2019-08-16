using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Story : MonoBehaviour {

    public GameObject ItemPrefab;
    //創建數量
    public int ItemCount;
    //物件的父物件
    public Transform ContentTransform;
    private float time = 0.0f;
    private int i = -1;
    public GameObject canvas;
    public GameObject teachcanvas;
    public CameraFollow camera;
    private bool DoTeach;
    private int runtime;
    private bool FistImage;
    private bool fadeout;

    public GameObject StartTimeLine;
    public PlayableDirector playableDirector;

    //public Text Teach;
    //public int TeachLevel;
    // Use this for initialization

    private void Awake()
    {
        camera = GameObject.FindObjectOfType<CameraFollow>();
        FistImage = false;
        fadeout = false;
        //teachcanvas = GameObject.Find("Teach");
        DoTeach = GameObject.Find("Stage1" ).GetComponent<LoadAndSave>().m_datainfolist.stageData.Pass;
        runtime = GameObject.Find("Stage1").GetComponent<LoadAndSave>().m_datainfolist.stageData.RunTime;
        //Debug.Log(DoTeach);
    }

    void Start () {
        //StoryData();
        //teachcanvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(!DoTeach && runtime<1)
        {
            //camera.LockCamera = true;
            time += Time.deltaTime;
            if (time > 5f && i <= ItemCount)
            {
                StoryData();
            }
            if(time > 3f && fadeout == false)
            {
                GameObject itemTemp = Instantiate(Resources.Load<GameObject>("Image/storypr/fadeout"));
                itemTemp.name = "fadeout" + i;
                itemTemp.transform.parent = ContentTransform;
                itemTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                fadeout = true;
            }
            if(FistImage == false)
            {
                StoryData();
                FistImage = true;
            }
            if (canvas.activeInHierarchy == true)
            {

                Debug.Log("123");
                //camera.LockCamera = true;
                camera.UnlockMouse();

                BoxBackageUIControl.Instance.Day = 7;

            }
            if (Input.GetKeyDown(KeyCode.Space) && FistImage == true)
            {
                StoryData();
            }
            if (i > ItemCount)
            {
                //canvas.SetActive(false);
                //teachcanvas.SetActive(true);
                CloseStory();

            }
        }
        else
        {
            canvas.SetActive(false);
        }
        
        
    }

    void StoryData()
    {
        if (i == -1)
        {
            GameObject itemTemp = Instantiate(Resources.Load<GameObject>("Image/storypr/" + (i + 2)));
            itemTemp.name = "Item" + (i + 1);
            itemTemp.transform.parent = ContentTransform;
            itemTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            //itemTemp.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/Story/" + (i + 1), typeof(Sprite));
            time = 0.0f;
            FadeOut();
            i++;
        }
        else
        {
            GameObject itemTemp = Instantiate(Resources.Load<GameObject>("Image/storypr/" + (i + 1)));
            itemTemp.name = "Item" + (i + 1);
            itemTemp.transform.parent = ContentTransform;
            itemTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            //itemTemp.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/Story/" + (i + 1), typeof(Sprite));
            time = 0.0f;
            i++;
            fadeout = false;
        }
    }
    public void Back()
    {
        GameObject itemTemp = Instantiate(Resources.Load<GameObject>("Image/storypr/" + (i - 1)));
        itemTemp.name = "Item" + (i + 1);
        itemTemp.transform.parent = ContentTransform;
        itemTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //itemTemp.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/Story/" + (i + 1), typeof(Sprite));
        time = 0.0f;
        i--;
    }
    public void CloseStory()
    {
        //camera.LockMouse();
        //camera.LockCamera = false;
        canvas.SetActive(false);

        //teachcanvas.SetActive(true);
        //i = 100;

        //GameObject.FindObjectOfType<MyTutorial>().enabled = true;
        StartTimeLine.SetActive(true);
        playableDirector.Play();
        camera.LockMouse();
    }

    public void StartTeachCanvas()
    {
        //teachcanvas.SetActive(true);
        i = 100;

        GameObject.FindObjectOfType<MyTutorial>().enabled = true;
    }
    public void FadeOut()
    {
        GameObject itemTemp = Instantiate(Resources.Load<GameObject>("Image/storypr/fadeIn"));
        itemTemp.name = "fadeout" + i;
        itemTemp.transform.parent = ContentTransform;
        itemTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        fadeout = true;
    }
    
}
