using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITypeChange : MonoBehaviour {

    public GameObject TypeCircle;
    float angel = 0;

    List<GameObject> circles = new List<GameObject>();
    int Choose = -1;

    List<GameObject> types = new List<GameObject>();
    int ChooseType = -1;

    public Text L_InfoText;

    public Transform R_bgInfo;
    public Transform L_bgInfo;

    // Use this for initialization
    void Start () {
        CreatLeftCircles();
        CreatRightCircles();
    }
	
	// Update is called once per frame
	void Update () {
        ChangeInfo();
        //transform.Rotate(0, 0, 5);
    }

    #region ChangeInfo
    void ChangeInfo()
    {
        //Debug.Log(circles.Count);
        EmptyJarUIControl();

        FullJarUIControl();

    }

    //空罐的位置
    void EmptyJarUIControl()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Choose < (circles.Count - 1))
            {
                Choose++;
            }
            else
            {
                if (Choose == circles.Count - 1)
                {
                    Choose = 0;
                }
            }
            int realNum = ((Choose + 3) < 20 ? (Choose + 3) : (Choose + 3 - 20));
            for (int i = 0; i < circles.Count; i++)
            {
                if (i == realNum)
                {

                }
                else
                {

                }
            }
            LeftRotate();
            L_InfoText.text = realNum.ToString();
        }
    }

    //裝完的罐子或道具的位置
    void FullJarUIControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ChooseType < (types.Count - 1))
            {
                ChooseType++;
            }
            else
            {
                if (ChooseType == types.Count - 1)
                {
                    ChooseType = 0;
                }
            }
            int realNum = ((ChooseType + 3) < 20 ? (ChooseType + 3) : (ChooseType + 3 - 20));
            for (int i = 0; i < types.Count; i++)
            {
                if (i == realNum)
                {

                }
                else
                {

                }
            }

            RightRotate();
        }
    }

    #endregion

    #region CreatEmptyTransform
    void CreatLeftCircles()
    {
        float Radius = 50;
        int nums = 20;
        for (int id = 0; id < nums; id++)

        {
            GameObject go = Resources.Load("TypeCircle") as GameObject;

            Vector2 Pos = new Vector2(
                                        Mathf.Sin(360 / nums * id * Mathf.PI / 180) * Radius * nums,
                                        Mathf.Cos(360 / nums * id * Mathf.PI / 180) * Radius * nums);
            Vector2 thisPos = new Vector2(L_bgInfo.GetComponent<RectTransform>().anchoredPosition.x + 960, L_bgInfo.GetComponent<RectTransform>().anchoredPosition.y + 540);
            //Quaternion Circle_rotate = new Quaternion(0, 0, id*360/20, 1);
            TypeCircle = Instantiate(go, thisPos + Pos, Quaternion.identity, L_bgInfo);
            //GameObject TypeCircle = Instantiate(go, thisPos + Pos, Circle_rotate, this.transform);
            //TypeCircle.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition + Pos;
            //TypeCircle.transform.parent = this.transform;
            TypeCircle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (id - 3) * 360 / 20));
            //TypeCircle.GetComponent<ShowUIInfo>().showInfo.text = id.ToString();

            circles.Add(go);
        }
    }

    void CreatRightCircles()
    {
        float Radius = 50;
        int nums = 20;
        for (int id = 0; id < nums; id++)

        {
            GameObject go = Resources.Load("TypeCircle") as GameObject;

            Vector2 Pos = new Vector2(
                                        Mathf.Sin(360 / nums * id * Mathf.PI / 180) * Radius * nums,
                                        Mathf.Cos(360 / nums * id * Mathf.PI / 180) * Radius * nums);
            Vector2 thisPos = new Vector2(R_bgInfo.GetComponent<RectTransform>().anchoredPosition.x + 960, R_bgInfo.GetComponent<RectTransform>().anchoredPosition.y + 540);
            //Quaternion Circle_rotate = new Quaternion(0, 0, id*360/20, 1);
            TypeCircle = Instantiate(go, thisPos + Pos, Quaternion.identity, R_bgInfo);
            //GameObject TypeCircle = Instantiate(go, thisPos + Pos, Circle_rotate, this.transform);
            //TypeCircle.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition + Pos;
            //TypeCircle.transform.parent = this.transform;
            TypeCircle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (id - 3) * 360 / 20));
            //TypeCircle.GetComponent<ShowUIInfo>().showInfo.text = id.ToString();

            types.Add(go);
        }
    }

    #endregion



    #region 旋轉

    void LeftRotate()
    {
        Quaternion ChangeAngle = new Quaternion(0, 0, Choose * 20, -1);
        L_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
        //for (int i = 0; i < circles.Count; i++)
        //{
        //    circles[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
        //}
    }

    void RightRotate()
    {
        Quaternion ChangeAngle = new Quaternion(0, 0, ChooseType * 20, -1);
        R_bgInfo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -ChooseType * 360 / 20));
    }


    #endregion

}
