using UnityEngine;
using System.Collections;

public class DeadState : FSMState
{
    public DeadState() 
    {
        stateID = FSMStateID.Dead;
    }

    public override void Reason(Transform player, AIController npc)
    {

    }

    public override void Act(Transform player, AIController npc)
    {        
		//Animation animComponent = npc.role.GetComponent<Animation>();
		//animComponent.CrossFade("death");
    }
}
