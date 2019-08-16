using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateControllerDeBug : MonoBehaviour {

    [SerializeField]
    private SoilderAIStateController aiStateController;

    private void Awake()
    {
        if(aiStateController==null)
        {
            aiStateController = gameObject.transform.parent.GetComponent<SoilderAIStateController>();
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 150, 500, 200),"AIStateControl");
        GUI.TextField(new Rect(240, 180, 180, 20),"Name : " + gameObject.transform.parent.name);
        GUI.TextField(new Rect(20, 180, 180, 20),"AIState : "+ aiStateController.m_aiState.ToString());
        if (GUI.Button(new Rect(20,200,50,20),"Idle"))
        {
            aiStateController.m_aiState = SoilderAIStateController.AIState.IDLE;
        }
        if (GUI.Button(new Rect(80, 200, 50, 20), "Walk"))
        {
            aiStateController.m_aiState = SoilderAIStateController.AIState.WALK;
            //aiStateController.MotionFollow();
        }
        if (GUI.Button(new Rect(140, 200, 50, 20), "Attack"))
        {
            aiStateController.theTarget = GameObject.Find("Target");

            aiStateController.m_aiState = SoilderAIStateController.AIState.ATTACK;
        }
        if (GUI.Button(new Rect(200, 200, 50, 20), "Dead"))
        {
            aiStateController.m_aiState = SoilderAIStateController.AIState.DEAD;
            //aiStateController.MotionFollow();
        }
        if (GUI.Button(new Rect(260, 200, 50, 20), "Defend"))
        {
            aiStateController.m_aiState = SoilderAIStateController.AIState.DEFEND;
            //aiStateController.MotionFollow();
        }
        if (GUI.Button(new Rect(450, 150, 50, 20), "X"))
        {
            aiStateController.HideAIStateController(this.gameObject);
        }
    }
}
