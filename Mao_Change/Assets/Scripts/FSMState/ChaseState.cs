using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    public ChaseState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        curRotSpeed = 6.0f;
        curSpeed = 160.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, AIController npc)
    {
        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.role.transform.position, destPos);
        if (dist <= attackDistance)
        {
            Debug.Log("Switch to Attack state");
            npc.SetTransition(Transition.ReachPlayer);
        }
        //Go back to patrol is it become too far
        else if (dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol state");
            npc.SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, AIController npc)
    {
        //Rotate to the target point
        destPos = player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.role.transform.position);
        npc.role.transform.rotation = Quaternion.Slerp(npc.role.transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        //npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
		CharacterController controller = npc.role.GetComponent<CharacterController>();
		controller.SimpleMove(npc.role.transform.forward * Time.deltaTime * curSpeed);

		//Animation animComponent = npc.role.GetComponent<Animation>();
		//animComponent.CrossFade("Run");
    }
}
