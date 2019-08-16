using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgTurn : MonoBehaviour {
    private Image Img;
    private Button FontBtn;
    private Button BackBtn;
    private Button CloseBtn;
    private int currentImgNum = 1;
    private int totalImgNum = 13;

    private void OnEnable()
    {

    }

    private void Awake()
    {
        Img = transform.GetChild(0).GetComponent<Image>();
        FontBtn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        BackBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        CloseBtn = transform.GetChild(0).GetChild(0).GetComponent<Button>();
    }

    // Use this for initialization
    void Start()
    {
        FontBtn.onClick.AddListener(() => ClickFrontBtn(FontBtn));
        BackBtn.onClick.AddListener(() => ClickBackBtn(BackBtn));
        CloseBtn.onClick.AddListener(() => ClickCloseBtn(CloseBtn));
        ShowImg();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowImg()
    {
        Img.sprite = Resources.Load<Sprite>("Tutorial/" + "teach_" + currentImgNum);
    }

    private void ClickCloseBtn(Button btn)
    {
        this.gameObject.SetActive(false);
    }

    private void ClickFrontBtn(Button btn)
    {
        currentImgNum++;
        if (currentImgNum > totalImgNum)
        {
            currentImgNum = 1;
        }
        Img.sprite = Resources.Load<Sprite>("Tutorial/" + "teach_" + currentImgNum);
    }
    private void ClickBackBtn(Button btn)
    {
        currentImgNum--;
        if (currentImgNum < 1)
        {
            currentImgNum = totalImgNum;
        }
        Img.sprite = Resources.Load<Sprite>("Tutorial/" + "teach_" + currentImgNum);
    }
}
