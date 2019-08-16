using UnityEngine;
using System.Collections;

public class AttackState : FSMState
{
    public AttackState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Attacking;
        curRotSpeed = 12.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, AIController npc)
    {
        //Check the distance with the player tank
        float dist = Vector3.Distance(npc.role.transform.position, player.position);

        if (dist >= attackDistance && dist < chaseDistance)
        {
            Debug.Log("Switch to Chase State");
            npc.SetTransition(Transition.SawPlayer);
        }
        //Transition to patrol is the tank become too far
        else if (dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol State");
            npc.SetTransition(Transition.LostPlayer);
        }  
    }

    public override void Act(Transform player, AIController npc)
    {
        //Set the target position as the player position
        destPos = player.position;        

		//Rotate to the target point
		Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.role.transform.position);
		npc.role.transform.rotation = Quaternion.Slerp(npc.role.transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Shoot bullet towards the player
		//Animation animComponent = npc.role.GetComponent<Animation>();
		//animComponent.CrossFade("StandingFire");
        npc.ShootBullet();

    }
}
