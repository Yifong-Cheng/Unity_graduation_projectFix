using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    //public Transform PlayerTransform;
    public GameObject Player;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = .5f;

    public bool LookAtPlayer = false;

    public bool RotateAroundPlayer = false;

    public float RotationSpeed = 5.0f;

    private float pitch, yaw;

    public bool HideMouse = false;

    private Quaternion _originalAngle;
    private Vector3 _originalOffset;
    public Vector3 CameraOffset = new Vector3(3.4f,10.1f,-23.9f);

    private CursorLockMode wantedMode;
    public bool LockCamera;
    public CinemachineFreeLook freeLook;
    public CinemachineStateDrivenCamera stateCamera;
    //private Camera realCamera;
    public Camera AreaCamera;
    private SelectTool tool;

    //private Button btn;
    private void Awake()
    {
        //PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(PlayerTransform.name);
        //realCamera = transform.GetChild(0).GetComponent<Camera>();
        tool = GameObject.FindObjectOfType<SelectTool>();
        //freeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        stateCamera = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>();
        
    }

    // Use this for initialization
    void Start () {

        _cameraOffset = CameraOffset;

        _originalAngle = transform.rotation;
        _originalOffset = CameraOffset;

        wantedMode = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CurseStateControl();
    }
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
    public void ShowCursor()
    {
        LockCamera = true;
        tool.enabled = true;
        Time.timeScale = 0.3f;
        Cursor.lockState = wantedMode = CursorLockMode.None;
    }

    public void CurseStateControl()
    {
        switch (Cursor.lockState)
        {
            case CursorLockMode.None:
                //if (Input.GetAxis("Mouse ScrollWheel") < (-.1f) && teach.Instance.TeachLevel == 14)
                //{
                //    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Farsee", 0, 0);
                //    Player.GetComponent<Animator>().Play("Farsee", 0, 0);
                //    GameObject.FindObjectOfType<CureInfo>().SetZhuyinRotation();
                //    teach.Instance.TeachLevel = 15;

                //}
                //else if (Input.GetAxis("Mouse ScrollWheel") < (-.1f) && teach.Instance.TeachLevel == 13)
                //{
                //    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Farsee", 0, 0);
                //    Player.GetComponent<Animator>().Play("Farsee", 0, 0);
                //    GameObject.FindObjectOfType<CureInfo>().SetZhuyinRotation();
                //    teach.Instance.TeachLevel = 15;

                //}
                //else 
                if (Input.GetAxis("Mouse ScrollWheel") < (-.1f))
                {
                    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Farsee", 0, 0);
                    Player.GetComponent<Animator>().Play("Farsee", 0, 0);
                    GameObject.FindObjectOfType<CureInfo>().SetZhuyinRotation();
                }
                
                else if (Input.GetAxis("Mouse ScrollWheel") > .1f)
                {
                    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Task", 0, 0);
                    Player.GetComponent<Animator>().Play("Task", 0, 0);
                    GameObject.FindObjectOfType<CureInfo>().DestoryStars();

                }
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    //freeLook.m_XAxis.m_InputAxisName = "";
                    //freeLook.m_YAxis.m_InputAxisName = "";
                    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Idle", 0, 0);
                    Player.GetComponent<Animator>().Play("Idle", 0, 0);
                    GameObject.FindObjectOfType<CureInfo>().DestoryStars();
                    LockCamera = false;
                    tool.enabled = false;
                    Time.timeScale = 1f;
                    Cursor.lockState = wantedMode = CursorLockMode.Locked;
                }
                
                break;
            case CursorLockMode.Confined:

                break;
            case CursorLockMode.Locked:
                tool.enabled = false;
                LockCamera = false;
                //freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                //freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
                //realCamera.fieldOfView = 60;
                //StartCoroutine(CameraNear());
                //Player.transform.GetChild(0).GetComponent<Animator>().Play("Idle");
                //Time.timeScale = 1;
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    //freeLook.m_XAxis.m_InputAxisName = "";
                    //freeLook.m_YAxis.m_InputAxisName = "";
                    //Player.transform.GetChild(0).GetComponent<Animator>().Play("Task", 0, 0);
                    Player.GetComponent<Animator>().Play("Task", 0, 0);
                    LockCamera = true;
                    tool.enabled = true;
                    Time.timeScale = 0.3f;
                    Cursor.lockState = wantedMode = CursorLockMode.None;
                }
                break;
        }
        SetCursorState();
    }

    public void UnlockMouse()
    {
        Cursor.lockState = wantedMode = CursorLockMode.None;
    }

    public void LockMouse()
    {
        Cursor.lockState = wantedMode = CursorLockMode.Locked;
    }

    IEnumerator CameraFar()
    {
        //realCamera.enabled = false;
        AreaCamera.gameObject.SetActive(true);
        yield return null;
    }
    IEnumerator CameraNear()
    {
        AreaCamera.gameObject.SetActive(false);
        //realCamera.enabled = true ;
        yield return null;
    }

    private void ContinueGame(Button btn)
    {
        Debug.Log("click");
        Player.transform.GetChild(0).GetComponent<Animator>().Play("Walk", 0, 0);
        Time.timeScale = 1;
        wantedMode = CursorLockMode.Locked;
        btn.gameObject.SetActive(false);
    }
}
