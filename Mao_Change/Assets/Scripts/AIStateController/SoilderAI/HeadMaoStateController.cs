using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeadMaoStateController : SoilderAIStateController
{
    /// <summary>
    /// HEADMAO _S3
    /// </summary>
    public GameObject Head;

    float angle = 120;
    int num = 0;
    public GameObject[] ShootObj = new GameObject[3];
    private Transform ShootPos;

    private int currentShoot = 0;
    AudioSource Audio;

    protected override void FixUpdateAI()
    {
        //InputProcess();
        //StateAction();
        //Spring(ref x, ref v, xt, 0.23f, 8.0f * Mathf.PI, Time.deltaTime);
        //Role.transform.rotation = Quaternion.AngleAxis(x, Vector3.Cross(Vector3.up, dir));
        RolePos = Role.transform.position;
        StateAction();
        //InputProcess();
        AnimationFinishResetPosition();
        CallHelp();
        if (BeAttack)
        {
            aIGameManger.StartCoroutineEvent(HitShader(.3f));
        }
    }

    public override void Intialize(AIGameManger _aIGameManger)
    {
        proprity = MediumProprity;
        //ShootPos = ShootObj[0].transform;
        for (int i = 0; i < 3; i++)
        {
            ShootObj[i] = Role.transform.GetChild(3).transform.GetChild(i).gameObject;
            Debug.Log(Role.transform.GetChild(3).transform.GetChild(i).name);
        }
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 3;
        SetValue();
        //spring
        x = 0.0f;
        v = 0.0f;
        xt = 0.0f;

        mx = Role.transform.position.x;
        my = Role.transform.position.z;
        Audio = Role.GetComponent<AudioSource>();
    }

    public override void SetValue()
    {
        base.SetValue();
        //ChaseDistance = 18;
        //AttackTime = 1.5f;
        //AttackDistance = 12f;
        //LookRadious = 9f;
        //wanderTime = Random.Range(1, 2);
        WaitTime = Random.Range(15, 20);
        PfbTypeName = "S3";
    }

    protected override void MotionIdle(float w_time)
    {
        base.MotionIdle(w_time);

    }

    public override void MotionWalk(GameObject target)
    {
        base.MotionWalk(target);
        //Vector3 Dir = target.transform.position - transform.position;
        //Dir.y = 0;
        //this.transform.rotation = Quaternion.LookRotation(Dir);
        //if (m_anim != null)
        //{
        //    m_anim.SetBool("IsWalk", true);
        //}

    }

    public override void MotionAttack(GameObject target)
    {
        //base.MotionAttack(target);
        //Vector3 toTarget = target.transform.position - Role.transform.position;
        //float distance = toTarget.magnitude;
        ////m_anim.SetFloat("Walk", distance - StopDistance);

        //if (Role.transform.position.y < theTarget.transform.position.y)
        //{
        //    Role.transform.position += new Vector3(0, 3 * Time.deltaTime, 0);
        //}
        //else
        //{
        //    Role.transform.position = new Vector3(Role.transform.position.x, theTarget.transform.position.y, Role.transform.position.z);
        //}

        //if (distance < StopDistance && CanAttack())
        //{
        //    //transform.forward = target.transform.forward;
        //    //RotateAngle(.2f,true);
        //    //Invoke("Attack", .3f);
        //    if(m_anim!=null)
        //    {
        //        m_anim.SetBool("IsAttack", CanAttack());
        //    }

        //    Attack();
        //}
        //else if (distance < StopDistance && !CanAttack())
        //{
        //    m_anim.SetBool("IsAttack", CanAttack());
        //    currentTime += Time.deltaTime;
        //    //m_anim.SetBool("Attack", false);
        //    //audioController.StopSound(1);
        //}
        ////Attack();
        /////---------------------
        //target.GetComponent<ObjInfo>().IsDead == true
        if (target == null)
        {
            Debug.Log("Enemy Is Dead");
            theTarget = null;
            //m_anim.SetBool("Attack", false);
            PlayAnim("Idle",-1.0f);
            m_aiState = AIState.IDLE;
        }
        else
        {
            if (currentTime > AttackTime)
            {
                Debug.Log("DoAttack 0.0");
                currentTime = -1.5f;
                Attack();
            }
            else if (currentTime < AttackTime)
            {
                currentTime += Time.deltaTime;

                //m_anim.SetBool("Walk", true);
                //m_anim.SetBool("Attack", false);
                if (audioController != null)
                {
                    audioController.StopSound(0);
                }
            }
            else if (currentTime < 0)
            {
                Vector3 backpos = Role.transform.position + target.transform.forward * 6f;
                nav.SetDestination(backpos);
                m_anim.SetBool("Walk", true);
                //m_anim.SetBool("Attack", false);
            }

            Vector3 Dir = target.transform.position - Role.transform.position;
            Dir.y = 0;
            Role.transform.rotation = Quaternion.LookRotation(Dir);
            Vector3 targetp = target.transform.position - Role.transform.forward * 5;
            if (!EnemyIsAround(AttackDistance, target,targetp))
            {
                Debug.Log("Start Chase Target");
                //m_anim.SetBool("Attack", false);
                m_aiState = AIState.Chase;
            }
        }
    }

    bool CanAttack()
    {
        return currentTime > AttackTime ? true : false;
    }

    private void RotateAngle(float speed, bool b)
    {
        if (b == true)
        {
            b = false;
            //有空優化旋轉
            Head.transform.rotation = Quaternion.AngleAxis(angle * (currentShoot % 3), Head.transform.up);
        }
        //yield return new WaitForSeconds(speed);
    }

    public override void MotionChase(GameObject target)
    {
        if(target!=null)
        {
            Vector3 targetp = target.transform.position - Role.transform.forward * 5;
            if (EnemyIsAround(AttackDistance, target,targetp))
            {
                //Debug.Log("Enemy IS In AttackRange");
                m_anim.SetBool("Walk", false);
                m_aiState = AIState.ATTACK;
            }

            if (Vector3.Distance(Role.transform.position, target.transform.position) > ChaseDistance)
            {
                //Debug.Log("Enemy TooFar");
                theTarget = null;
                //m_anim.SetBool("Walk", false);
                m_aiState = AIState.IDLE;
            }
            else
            {
                m_anim.SetBool("Walk", true);
                //Vector3 pos = target.transform.position;
                Vector3 pos = target.transform.position - target.transform.forward * 5;
                //Debug.Log(" _ ChaseDistance _ " + Vector3.Distance(Role.transform.position, pos));
                nav.SetDestination(pos);
            }
        }
        else
        {
            //m_anim.SetBool("Attack", false);
            m_anim.SetBool("Walk", false);
            m_aiState = AIState.IDLE;
        }
        
    }

    public override void Attack()
    {
        Audio.Play();
        base.Attack();
        ShootPos = ShootObj[currentShoot % 3].transform;
        //GameObject go = Resources.Load<GameObject>("Shoot");
        //Role.Instantiate(go, ShootPos, Quaternion.identity);
        //GameObject go = aIGameManger.CreatItem("Poke", ShootPos.position, Quaternion.identity);
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>( "Poke"), ShootPos.transform.position + ShootPos.forward, Quaternion.identity);

        //go.transform.parent = null;
        go.GetComponent<Rigidbody>().AddForce(Role.transform.forward * 20, ForceMode.Impulse);
        //Debug.Log(go.name + " / " + go.transform.position + "IsCreatInHeare");
        for (int i = 0; i < ShootObj.Length; i++)
        {
            if (i == (currentShoot % 3))
            {
                ShootObj[i].SetActive(false);
            }
            else
                ShootObj[i].SetActive(true);
        }
        currentTime = 0;
        currentShoot += 1;

        //Head.transform.rotation = Quaternion.AngleAxis(angle,Head.transform.up);
        //Head.transform.rotation = Quaternion.EulerAngles(0,angle, 0);
    }

    public override void MotionDead()
    {
        aIGameManger.StartCoroutineEvent(HitShader(.3f));
        //aIGameManger.StartCoroutineEvent(DissloveShader(.3f));
        base.MotionDead();
        //m_anim.SetBool("IsDead", true);
    }

    public override void MotionDefend()
    {
        base.MotionDefend();
    }

    public override void MotionWander(CapsuleCollider t)
    {
        base.MotionWander(t);
    }

    #region Aglur Spring
    float x, v, xt;
    Vector3 dir;
    float mx, my;

    void Spring
        (
         ref float x, ref float v, float xt,
         float zeta, float omega, float h
         )
    {
        float f = 1.0f + 2.0f * h * zeta * omega;
        float oo = omega * omega;
        float hoo = h * oo;
        float hhoo = h * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + h * v + hhoo * xt;
        float detV = v + hoo * (xt - x);
        x = detX * detInv;
        v = detV * detInv;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        mx = Role.transform.position.x;
        my = Role.transform.position.z;
        Vector3 m = other.transform.position;

        float dx = m.x - mx;

        float dy = m.z - my;

        float r = Mathf.Sqrt(dx * dx + dy * dy);

        if (r > 1.0f)
        {

        }

        x = r * 50;
        dir.Set(dx * 100, 0.0f, dy * 100);
    }
}
