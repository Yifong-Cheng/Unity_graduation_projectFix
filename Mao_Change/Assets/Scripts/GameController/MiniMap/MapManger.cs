using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManger : MonoBehaviour {

    private GameObject CloseBtn;
    private GameObject OpenBtn;
    private GameObject MiniMap;
    private float MiniCameraRange = 5f;
    private GameObject playerCamera;
    private Slider MiniSlider;

    private void Start()
    {
        playerCamera = GameObject.Find("Player").transform.GetChild(1).gameObject;
        CloseBtn = GameObject.Find("CloseBtn");
        OpenBtn = GameObject.Find("OpenBtn");
        MiniMap = GameObject.Find("MiniBackImg");
        OpenBtn.SetActive(false);
        MiniSlider = transform.GetChild(1).GetComponent<Slider>();
    }

    public void MapClose()
    {
        OpenBtn.gameObject.SetActive(true);
        MiniMap.SetActive(false);
        CloseBtn.gameObject.SetActive(false);
    }
    public void MapOpen()
    {
        CloseBtn.gameObject.SetActive(true);
        MiniMap.SetActive(true);
        OpenBtn.gameObject.SetActive(false);
    }
    public void ZoomMiniSight()
    {
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, MiniSlider.value, playerCamera.transform.position.z);
    }
}
