using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCompose1 : MonoBehaviour {
    //public Dictionary<Transform,string> ComposeTeam = new Dictionary<Transform,string>();
    public List<GameObject> ComposeTeam = new List<GameObject>();
    [SerializeField]
    int count;
    Radar neighbor;
    public int totalCompose=0;
    public int countDownCompose=1;

    public Transform[] bodys = new Transform[4];

    public bool RemoveComposeCheck;
    public bool IsCompose;

    // Use this for initialization
    void Start () {
        neighbor = this.GetComponent<Radar>();
	}
	
	// Update is called once per frame
	void Update () {
		if(neighbor.neighbors.Count!=ComposeTeam.Count)
        {
            for(int i=0;i<neighbor.neighbors.Count;i++)
            {
                if(ComposeTeam.Contains(neighbor.neighbors[i]))
                {

                }
                else if(!ComposeTeam.Contains(neighbor.neighbors[i]))
                {
                    ComposeTeam.Add(neighbor.neighbors[i]);
                }
                
            }
        }
        count = ComposeTeam.Count;

        if(Input.GetKey(KeyCode.LeftControl))
        {
            StartCompose();
            IsCompose = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            RemoveCompose();
        }

        if(IsCompose&&!RemoveComposeCheck)
        {
            Invoke("AutoRemove",1);
        }

    }
    #region 具集合體
    void StartCompose()
    {
        for (int i = 0; i < neighbor.neighbors.Count;i++)
        {
            
            if (i<bodys.Length && ComposeTeam[i]!=this.gameObject)
            {
                
                ComposeTeam[i].GetComponent<SteeringForCohesion>().enabled = false;
                ComposeTeam[i].GetComponent<Vehicle>().velocity = Vector3.zero;
                ComposeTeam[i].GetComponent<Vehicle>().enabled = false;
                ComposeTeam[i].GetComponent<CharacterController>().enabled = false;
                ComposeTeam[i].transform.position = bodys[i].position;
                ComposeTeam[i].transform.parent = bodys[i].transform;

            }
            else
            {
                return;
            }
        }
        totalCompose = ComposeTeam.Count;
    }
    void Compose()
    {
        
    }

    void RemoveCompose()
    {
        ComposeTeam[totalCompose - countDownCompose].transform.parent = null;
        ComposeTeam[totalCompose - countDownCompose].GetComponent<CharacterController>().enabled = true;
        countDownCompose++;
    }

    void AutoRemove()
    {

        for(int i =0;i<ComposeTeam.Count;i++)
        {
            ComposeTeam[i].transform.parent = null;
            ComposeTeam[i].GetComponent<CharacterController>().enabled = true;
            ComposeTeam[i].GetComponent<Vehicle>().enabled = true;
            if(ComposeTeam[i].GetComponent<SteeringForWander>()!=null)
            {
                ComposeTeam[i].GetComponent<SteeringForWander>().enabled = true;
            }
            
        }

        ComposeTeam.Clear();
        RemoveComposeCheck = true;
    }

    #endregion

}
