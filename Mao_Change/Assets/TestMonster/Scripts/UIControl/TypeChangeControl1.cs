using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeChangeControl1 : MonoBehaviour {

    public GameObject TypeCircle;
    float angel = 0;

    public Material mat0;
    public Material mat1;

    List<GameObject> circles= new List<GameObject>();

    int Choose = -1;
    // Use this for initialization
    void Start () {
        //CreatAllCircles();
        CreatCircles();
    }

	// Update is called once per frame
	void Update () {
        //transform.Rotate(0, 0, 5);

        ChangeColor();
        

    }

    #region changecolor
    void ChangeColor()
    {
        //Debug.Log(circles.Count);
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
                //else
                //{
                //    Choose = 0;
                //}

            }
            
            for (int i = 0; i < circles.Count; i++)
            {
                if (i == Choose)
                {
                    circles[Choose].GetComponent<MeshRenderer>().sharedMaterial = mat1;
                }
                else
                {
                    circles[i].GetComponent<MeshRenderer>().sharedMaterial = mat0;
                }
            }
            LeftRotate();
        }
    }

    #endregion

    #region 六邊形
    GameObject Zero_OBJ;//六边形物体
    void CreatAllCircles()
    {
        int RoundMax = 10;//最大环数变量 //10

        float Radius = 1f;//六边形最短宽度 //1

        for (int round = 1; round <= RoundMax; round++)//每一层环的循环体

        {
            for (int id = 0; id <= round * 6; id++)//当前环的总个数 = round*6

            {
                Vector3 Pos =
                    new Vector3(
                                Mathf.Sin(360 / (round * 6) * id * Mathf.PI / 180) * Radius * round + 0,
                                Mathf.Cos(360 / (round * 6) * id * Mathf.PI / 180) * Radius * round);

                Instantiate(TypeCircle, TypeCircle.transform.position + Pos,//依据物体坐标偏移
                Quaternion.identity);
            }
        }
    }
    #endregion


    #region 旋轉

    void LeftRotate()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
            
        //    Quaternion ChangeAngle = new Quaternion(0, 0, Choose * 20, -1);
        //    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360/20));
        //}
        Quaternion ChangeAngle = new Quaternion(0, 0, Choose * 20, -1);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Choose * 360 / 20));
    }

    void RightRotate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.eulerAngles = new Vector3(0, 0 , Choose * 20);
        }
    }


    #endregion

    void CreatCircles()
    {
        float Radius = 0.15f;
        int nums = 20;
        for (int id = 0; id < nums; id++)

        {
            Vector3 Pos = new Vector3(
                                        Mathf.Sin(360 / nums * id * Mathf.PI / 180) * Radius * nums,
                                        Mathf.Cos(360 / nums * id * Mathf.PI / 180) * Radius * nums);
            GameObject go = Instantiate(TypeCircle,this.transform.position+Pos,Quaternion.identity,this.transform);
            
            circles.Add(go);
        }
    }

    private float timeScale = 1.0f;

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 100), "Player Machine");

        GUI.TextField(new Rect(20, 40, 180, 20), string.Format("State: {0}", this.Choose));
        timeScale = GUI.HorizontalSlider(new Rect(20, 70, 180, 20), timeScale, 0.0f, 1.0f);

        Time.timeScale = timeScale;
    }


    //GameObject Zero_OBJ;//六边形物体
    //    void CreatAllCircles1()
    //    {


    //        int RoundMax = 10;//最大环数变量

    //        float Radius = 1f;//六边形最短宽度

    //        for(int round = 1; round <= RoundMax; round++)//每一层环的循环体

    //{

    //            int[] Pos_6 = new int[6];//记录正确6个位置的数组
    //            Vector3[] Pos_6id = new Vector3[Pos_6[round]];
    //            for (int id = 0; id <= 6; id++)
    //            {

    //                Pos_6id[id] = 
    //                    new Vector3(Mathf.Sin(360 / (round * 6) * id * Mathf.PI / 180) * Radius*round ,
    //                                0,
    //                                Mathf.Cos(360 / (round * 6) * id * Mathf.PI / 180) * Radius * round);
    //                Instantiate(Zero_OBJ,Zero_OBJ.transform.position + Pos_6id[id],//依据物体坐标偏移
    //                Quaternion.identity);

    //            }
    //            if (round > 1)//第2圈开始执行插入

    //            {
    //                for(int id = 0; id <= 6; id++)//逐个区间插入
    //                {
    //                    int NextID = (id + 1) % 6;//获取下一个位置ID，在0~5中循环取值
    //                    Vector3 Orientation = Vector3.Normalize(Pos_6id[id], Pos_6[NextID]);//单位朝向（当前点，上一个点）

    //                    for (int add = 0; add < round - 1; add++)//循环插入

    //                    {
    //                        //----------插入点 = 单位方向*当前偏移距离+起点偏移
    //                        Vector3 Now_Pos =Orientation* (Radius * add)+ (Pos_6id[id] + Zero_OBJ.transform.position);
    //                        //-------------------------------------------------------------
    //                        Instantiate(Zero_OBJ, Now_Pos, Quaternion.identity);
    //                    }

    //                }

    //            }
    //        }
    //    }
}
