using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Count : MonoBehaviour {

    public Text[] text = new Text[5];
    public int[] number = new int[6];
    public static Count Instance;
    public float Crystal = 0.0f;

    public int[] StageDay = new int[3];
    public bool StoryNot = false;
    public bool TeachNot = false;
    // Use this for initialization
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*for(int i =0;i<5;i++)
        {
            text[i].text = number[i].ToString();
        }*/
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < 5; i++)
                number[i] += 5;
            Crystal += 5;
        }
        if(Application.loadedLevelName == "MainMenuScene")
        {
            for (int i = 0; i < 5; i++)
                number[i] = 0;
            Crystal = 0;
        }

    }
}
