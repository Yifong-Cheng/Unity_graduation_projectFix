using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHouseControl0 : MonoBehaviour {

    //public GameObject currentHitObj;
    public GameObject m_player;
    [SerializeField]
    float SphereRadious=5;
    [SerializeField]
    float maxDistance = 0;
    [SerializeField]
    LayerMask layerMask;

    private Vector3 original;
    private Vector3 direction;

    private float currentHitDistance;

    public bool SthComming;
    public bool IsStay;
    float shaketime = 9.5f;
    public float currentshaketime=0f;
    //[SerializeField]
    //GameObject Sign;

    float delaytime = 0.8f;
    float timer = 0.04f;

    int totalMonster = 5;
    int currentMonster = 0;
    [SerializeField]
    List<GameObject> Monsters = new List<GameObject>();

    GameObject monster;
    EnemyAIStateController aI;

    private Transform C_Pos;

    // Use this for initialization
    void Start () {
        //Sign = transform.GetChild(1).transform.gameObject;
        original = transform.position;
        direction = transform.forward;
        C_Pos = transform.GetChild(2);

        for(int i=0;i<totalMonster;i++)
        {
            GameObject go = Resources.Load("TestMonster") as GameObject;
            go.transform.position = this.transform.forward;
            go.transform.rotation = Quaternion.identity;
            Monsters.Add(go);
        }
	}
	
	// Update is called once per frame
	void Update () {
        IfInRange();

        CreatMonster();

        PlayShake();

        //DrawSingleSize();

    }

    bool stopcreat;
    void DrawSingleSize()
    {
        #region sphereray
        //RaycastHit hit;
        //if(Physics.SphereCast(original, SphereRadious,direction,out hit,maxDistance,layerMask,QueryTriggerInteraction.UseGlobal))
        //{
        //    //currentHitObj = hit.transform.gameObject;
        //    //currentHitDistance = hit.distance;

        //    if (hit.transform.name == "Player")
        //    {
        //        SthComming = true;
        //    }
        //    else
        //    {
        //        SthComming = false;

        //    }
        //}
        //else
        //{

        //    //currentHitDistance = maxDistance;
        //    //currentHitObj = null;
        //}
        #endregion

    }

    #region 探測範圍

    void IfInRange()
    {
        if (Vector3.Distance(original, m_player.transform.position) < SphereRadious)
        {
            SthComming = true;
            currentshaketime = currentshaketime + Time.deltaTime;

            if (currentshaketime > shaketime)
            {
                IsStay = true;
                //currentshaketime = 0f;
            }
            else if (currentshaketime == 0 && SthComming == false)
            {
                IsStay = false;
            }


        }
        else
        {
            SthComming = false;
            IsStay = false;
            currentshaketime = 0f;
            stopcreat = false;
            waittime = 6;
        }
    }

    #endregion

    #region 抖動
    void Shake0()
    {
        this.transform.position = new Vector3(original.x + 0.25f, original.y, original.z);
        Invoke("Shake1", timer);
    }

    void Shake1()
    {
        this.transform.position = original;
        Invoke("Shake2", timer);
    }

    void Shake2()
    {
        this.transform.position = new Vector3(original.x - 0.25f, original.y, original.z);
        Invoke("Shake3", timer);
    }

    void Shake3()
    {
        this.transform.position = original;
        IsStay = true;
    }

    IEnumerator Shake(float speed,float m)
    {
        
        this.transform.position = new Vector3(original.x + m, original.y, original.z);
        yield return new WaitForSeconds(speed);
        this.transform.position = original;
        yield return new WaitForSeconds(speed);
        this.transform.position = new Vector3(original.x - m, original.y, original.z);
        yield return new WaitForSeconds(speed);
        this.transform.position = original;
        yield return new WaitForSeconds(speed);
    }

    void PlayShake()
    {
        if (SthComming && !IsStay)
        {
            //Sign.SetActive(true);

            //StartCoroutine(Shake(0.05f, 0.8f));
            Invoke("Shake0", delaytime);
        }
    }

    #endregion

    #region 產生
    float counttimer = 0;
    int waittime = 6;
    void CreatMonster()
    {
        if (IsStay)
        {
            //Instantiate(Monsters[currentMonster].gameObject);
            counttimer += Time.deltaTime;
            if(counttimer>waittime)
            {
                waittime -= 1;
                counttimer = 0;
                stopcreat = false;
            }

            if (currentMonster < totalMonster && !stopcreat && counttimer > (waittime-0.5f))
            {
                stopcreat = true;
                Monsters[currentMonster].transform.position = C_Pos.position;
                monster = Instantiate(Monsters[currentMonster].gameObject);
                Debug.LogWarning("Creat Monster Role Need Use AIGameManger");
                aI = monster.GetComponent<EnemyAIStateController>();
                aI.theTarget = m_player;
                aI.m_aiState = EnemyAIStateController.EnemyAIState.ATTACK;
                currentMonster++;
            }
        }
    }
    #endregion

    #region 派出

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(original, original + direction * currentHitDistance);
        Gizmos.DrawWireSphere(original + direction * currentHitDistance, SphereRadious);
    }
}
