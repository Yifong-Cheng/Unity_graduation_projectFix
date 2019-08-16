using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUIInfo : MonoBehaviour {
    public Text showInfo;
    private UITypeChange1 changeControl;
    
	// Use this for initialization
	void Start () {
        changeControl = transform.parent.parent.GetComponent<UITypeChange1>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            Debug.Log("click" + changeControl.L_InfoText.text);
        }

        changeControl.OnPointerClick();
    }
}
