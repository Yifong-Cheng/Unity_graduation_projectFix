using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//我是外掛
public class SuperControl : MonoBehaviour {
    private List<ObjInfo> Objs = new List<ObjInfo>();
    public string BossString = "Boss";
    public string LittleBossString = "LittleBoss";
    public string SoilderIdleString = "IDLE";
    public OLDPlayerController m_player;
    public GameObject player;


    // Use this for initialization
    void Start()
    {
        m_player = player.GetComponent<OLDPlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

        //離開遊戲
        EscapeGame();

        //待機小兵清除
        DeleteIdleSoilder();

        //清小怪
        LittleKilled();

        //清大怪
        BossKilled();

        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            AddAllMats();
        }
        
    }

    private void EscapeGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

        }
    }

    private void DeleteIdleSoilder()
    {
        if(Input.GetKey(KeyCode.Delete))
        {
            ObjInfo[] Soilders = GameObject.FindObjectsOfType<ObjInfo>();
            //Objs.Add(Boss);
            for (int i = 0; i < Soilders.Length; i++)
            {
                if (Soilders[i].FollowerTypeID == 0 || Soilders[i].FollowerTypeID == 1 || Soilders[i].FollowerTypeID == 2)
                {
                    if(Soilders[i].State == SoilderIdleString)
                    {
                        Objs.Add(Soilders[i]);
                    }
                    
                }
            }
            for (int i = 0; i < Objs.Count; i++)
            {
                Objs[i].IsDead = true;
            }
            Release();
        }
    }

    private void BossKilled()
    {
        if(Input.GetKey(KeyCode.Backspace)&& Input.GetKey(KeyCode.LeftControl))
        {
            ObjInfo Boss = GameObject.Find(BossString).gameObject.GetComponent<ObjInfo>();
            Objs.Add(Boss);
        }
    }

    private void LittleKilled()
    {
        if (Input.GetKey(KeyCode.Backspace)&&Input.GetKey(KeyCode.LeftAlt))
        {
            ObjInfo[] LittleBoss = GameObject.FindObjectsOfType<ObjInfo>();
            //Objs.Add(Boss);
            for(int i=0;i< LittleBoss.Length;i++)
            {
                if(LittleBoss[i].Type==LittleBossString)
                {
                    Objs.Add(LittleBoss[i]);
                }
            }
            for(int i=0;i<Objs.Count;i++)
            {
                Objs[i].IsDead = true;
            }
            Release();
        }
    }

    private void Release()
    {
        Objs.Clear();
    }

    private void AddAllMats()
    {
        m_player.m_bgInfo.Instance.Enegy += 100;
        m_player.m_bgInfo.Instance.Mat0 += 10;
        m_player.m_bgInfo.Instance.Mat1 += 10;
        m_player.m_bgInfo.Instance.Mat2 += 10;

        Debug.Log("Super_Add_Success!!");
        Debug.Log("BagInfo : " + m_player.m_bgInfo.Instance.Enegy);
    }
}
