using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITypeChange2 : MonoBehaviour {
    private int BackageNums = 6;
    private GameObject Prop;
    public List<GameObject> Props = new List<GameObject>();
    private int Choose = 0;
    public GameObject Sign;
    public GameObject LightSign;

    public GameObject Player;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    // Use this for initialization
    void Start () {
        //CreatBackageObj(BackageNums);
        //CreatTestProp(BackageNums);

    }
	
	// Update is called once per frame
	void Update () {
        ChangeChoose();
        UseProp();
    }

    #region CreatUI

    void CreatBackageObj(int nums)
    {
        for(int i =0;i<nums;i++)
        {
            GameObject go = Resources.Load("TestProp") as GameObject;
            go.name = "P" + i;
            Vector2 Pos = new Vector2(-525+(i*175),0);
            Vector2 thisPos = new Vector2(this.transform.GetComponent<RectTransform>().position.x, this.transform.GetComponent<RectTransform>().position.y);
            Prop = Instantiate(go, thisPos + Pos, Quaternion.identity, this.transform);
            Props.Add(Prop);
        }

    }
    void CreatTestProp(int nums)
    {
        for (int i = 0; i < nums; i++)
        {
            GameObject go = Resources.Load("TestProp") as GameObject;
            go.name = "P" + i;
            //Vector2 Pos = new Vector2(-525 + (i * 175), 0);
            Vector2 thisPos = new Vector2(Props[i].transform.GetComponent<RectTransform>().position.x, Props[i].transform.GetComponent<RectTransform>().position.y);
            Prop = Instantiate(go, thisPos, Quaternion.identity, this.transform.GetChild(i+1));
            //Props.Add(Prop);
        }
        Prop = null;

    }

    #endregion

    #region UIControl

    void ChangeChoose()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (Choose < (Props.Count - 1))
            {
                Choose++;
            }
            else
            {
                if (Choose == Props.Count - 1)
                {
                    Choose = 0;
                }
            }
            ChangeBackageUI();
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            if (Choose > 0)
            {
                Choose--;
            }
            else if(Choose<=0)
            {
                if (Choose == 0)
                {
                    Choose = (Props.Count-1);
                }
            }
            //Debug.Log(Choose);
            ChangeBackageUI();
        }
    }

    void ChangeBackageUI()
    {
        if(Sign.active==false || LightSign.active == false)
        {
            Sign.SetActive(true);
            LightSign.SetActive(true);
        }
        Sign.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.x, 
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.y-350
                                                                         );
        LightSign.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.x,
                                                                            Props[Choose].GetComponent<RectTransform>().anchoredPosition.y
                                                                         );
    }

    #endregion

    #region UseProp

    void UseProp()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            if(PropEnough())
            {
                //DoAnim

                //Creat
                Instantiate(RealProp(), Player.GetComponent<NewPlayerController>().PutPos.position + new Vector3(0,3,0), Quaternion.identity);
            }
            else
            {
                Debug.Log("This IS Empty");
            }
        }
    }

    bool PropEnough()
    {
        return Props[Choose].GetComponent<PropInfoControl>().num > 0 ? true : false;
    }

    GameObject RealProp()
    {
        GameObject go  = Resources.Load<GameObject>("TestProps/" + Props[Choose].GetComponent<PropInfoControl>().ImageInfoName);
        return go;
    }

    #endregion
}
