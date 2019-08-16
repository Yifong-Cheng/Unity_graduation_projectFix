using UnityEngine;
using System.Collections;

public class AIController : AdvancedFSM 
{
    public GameObject Bullet;
	public Transform bulletSpawnPoint;
    private int health;

    

    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {
        health = 100;

        elapsedTime = 0.0f;
        shootRate = 0.5f;

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            Debug.Log("Player doesn't exist.. Please add one with Tag named 'Player'");        

        //Start Doing the Finite State Machine
        ConstructFSM();

        
    }

    float lifetime = 0;

    //Update each frame
    protected override void FSMUpdate()
    {
        //Check for health
        elapsedTime += Time.deltaTime;

        //lifetime += Time.deltaTime;
        //if (lifetime > 10)
        //    RemoveSelf();
    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(playerTransform, this);
        CurrentState.Act(playerTransform, this);
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    private void ConstructFSM()
    {
        //Get the list of points
        //pointList = GameObject.FindGameObjectsWithTag("PatrolPoint");

        //Transform[] waypoints = new Transform[pointList.Length];
        //int i = 0;
        //foreach(GameObject obj in pointList)
        //{
        //    waypoints[i] = obj.transform;
        //    i++;
        //}

        int pointlength = Random.Range(3, 5);
        Debug.Log("pointlength = " + pointlength);
        Transform[] waypoints = new Transform[pointlength];
        float maxdistance = 50;
        float mindistance = -30;
        GameObject partpointparent =new GameObject();
        partpointparent.transform.position = role.transform.position;
        partpointparent.name = role.name + "[partolpoints]"; 

        for (int i=0 ; i<pointlength;i++)
        {
            Vector3 pos = new Vector3(role.transform.position.x + CreatPatrolRange(maxdistance,mindistance), 0, role.transform.position.z + CreatPatrolRange(maxdistance, mindistance));
            GameObject go = new GameObject();
            go.transform.position = pos;
            go.transform.parent = partpointparent.transform; ;
            waypoints[i] = go.transform;
        }
        Debug.Log("waypoints length -> " + waypoints.Length);

        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 50;

            if (health <= 0)
            {
                Debug.Log("Switch to Dead State");
                SetTransition(Transition.NoHealth);                
            }
        }
    }


    
    // Shoot the bullet    
    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            //GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.position, role.transform.rotation) as GameObject;
            //bulletObj.GetComponent<Bullet>().Go();
            Debug.Log("Shoot");
            elapsedTime = 0.0f;
        }
    }

    private float CreatPatrolRange(float maxdistance, float mindistance)
    {
        float value = Random.Range(mindistance, maxdistance);
        return value;

    }
    public AIGameManger aIGameManger = null;
    private void RemoveSelf()
    {
        aIGameManger.RemoveAIObj(this);
        Debug.Log("Destory this game obj " + role.name);
        aIGameManger.aIControllers.Remove(this);
        Debug.Log("{ aicontrollers.count  =  " + aIGameManger.aIControllers.Count + " } ");
    }
}
