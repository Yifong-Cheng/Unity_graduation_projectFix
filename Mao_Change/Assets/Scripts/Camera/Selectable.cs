using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public GameObject showSelect;
    float R;
    [SerializeField]
    bool isSelect;
    public SoilderAIStateController ai;
    Vector3 hitend;
    // Use this for initialization
    void Start()
    {
        R = UnityEngine.Random.Range(30, 120);
        SetUnSelected();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up * Time.deltaTime * R, Space.World);
    }

    public void SetSelected()
    {
        //print("Select" + gameObject.name);
        isSelect = true;
        showSelect.gameObject.SetActive(isSelect);
    }
    public void SetUnSelected()
    {
        //print("UnSelect" + gameObject.name);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit);
        hitend = hit.point;
        if (isSelect)
        {
            isSelect = false;
            ai.m_aiState = SoilderAIStateController.AIState.IDLE;
            Vector3 POS = hitend;
            POS.y = 0;
            ai.targetPos.transform.position = POS;
            GameObject sign = GameObject.Find("Sign");
            if (sign==null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Sign"), POS, Quaternion.identity);
                go.name = "Sign";
            }
            else
            {
                sign.transform.position = POS;
            }
            ai.m_aiState = SoilderAIStateController.AIState.WALK;

        }
        isSelect = false;

        showSelect.gameObject.SetActive(isSelect);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SelectBox"))
        {
            //if (other.transform.parent.GetComponent<SelectTool>()) SetSelected();
            if (GameObject.FindObjectOfType<SelectTool>()) SetSelected();
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SelectBox"))
        {
            //if (other.transform.parent.GetComponent<SelectTool>()) SetUnSelected();
            if (GameObject.FindObjectOfType<SelectTool>()) SetUnSelected();
        }
        
    }

}
