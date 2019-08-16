using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropInfoControl : MonoBehaviour
{
    public string ImageInfoName = null;
    public string Path = null;
    public int num = 0;
    public int image;
    public Text Name;
    public GameObject Show;
    private bool ShowNameOrNot = false;
    // Use this for initialization
    void Start()
    {
        //Thos Is Out Of Date
        //this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("TestImage/"+ImageInfoName);
        /*if(ImageInfoName!=null || ImageInfoName!="")
        {
            this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(Path + "/" + image);
        }*/
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(Path + "/" + image);
        if (ShowNameOrNot == false)
        {
            switch (image)
            {
                case 1:
                    Name.text = "";
                    break;
                case 2:
                    Name.text = "" + Count.Instance.number[0];
                    num = Count.Instance.number[0];
                    break;
                case 3:
                    Name.text = "" + Count.Instance.number[1];
                    num = Count.Instance.number[1];
                    break;
                case 4:
                    Name.text = "" + Count.Instance.number[2];
                    num = Count.Instance.number[2];
                    break;
                case 5:
                    Name.text = "" + Count.Instance.number[3];
                    num = Count.Instance.number[3];
                    break;
                case 6:
                    Name.text = "" + Count.Instance.number[4];
                    num = Count.Instance.number[4];
                    break;
            }
        }
        //if(teach.Instance.teachcanvas.activeInHierarchy == false)
        //{
        
        //}

        if (Input.GetKeyDown(KeyCode.E))
        {

            image++;
            Check();
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            image--;
            Check();
            UpdateUI();
        }
    }


    void UpdateUI()
    {
        /*if (image > 6)
        {
            image = 1;
            this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(Path + "/" + image);
        }
        else if (image < 1)
        {
            image = 6;
            this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(Path + "/" + image);
        }*/
        this.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Image/ui/" + image);
        switch (image)
        {
            case 1:
                Name.text = "";
                break;
            case 2:
                Name.text = "" + Count.Instance.number[0];
                break;
            case 3:
                Name.text = "" + Count.Instance.number[1];
                break;
            case 4:
                Name.text = "" + Count.Instance.number[2];
                break;
            case 5:
                Name.text = "" + Count.Instance.number[3];
                break;
            case 6:
                Name.text = "" + Count.Instance.number[4];
                break;
        }
        
    }
    void Check()
    {
        if (image > 6)
        {
            image = 1;

        }
        else if (image < 1)
        {
            image = 6;

        }
    }
    public void ShowName()
    {
        ShowNameOrNot = true;
        switch (image)
        {
            case 1:
                Name.text = "打氣筒";
                break;
            case 2:
                Name.text = "小茸毛果";
                break;
            case 3:
                Name.text = "噪集毛果";
                break;
            case 4:
                Name.text = "炸炸毛果";
                break;
            case 5:
                Name.text = "刺刺毛果";
                break;
            case 6:
                Name.text = "壯壯毛果";
                break;

        }

        Show.SetActive(true);


    }
    public void NoShow()
    {
        ShowNameOrNot = false;
        UpdateUI();
    }
}
