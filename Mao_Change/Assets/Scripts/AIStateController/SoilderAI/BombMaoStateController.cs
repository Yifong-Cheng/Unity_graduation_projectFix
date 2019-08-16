using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BombMaoStateController : SoilderAIStateController
{

    /// <summary>
    /// Bomb S2
    /// </summary>
    public float BombTime = 1.5f;
    //private CinemachineFreeLook camFreeLook;
    CameraShake cameraShake;
    private float BombcurrentTime;
    private int BeHitTime;

    private float power = 50f;
    private float upforce = 10f;
    private float radious = 30f;
    private float shake = 0.2f;

    private enum bombState
    {
        Null,
        IDLE,
        FLY,
        FLYDEAD,
        DEAD,
    }
    private bombState m_BombState = bombState.IDLE;

    public override void Intialize(AIGameManger _aIGameManger)
    {
        base.Intialize(_aIGameManger);
        m_objInfo.Type = "Soilder";
        m_objInfo.FollowerTypeID = 2;
        PfbTypeName = "S2";
        m_BombState = bombState.Null;
        //camFreeLook = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponentInChildren<CinemachineFreeLook>();
        cameraShake = GameObject.FindObjectOfType<CameraShake>();
    }

    protected override void FixUpdateAI()
    {
        StateAction();
        if(BeAttack)
        {
            BeAttack = false;
            BeHitTime++;
        }

        
    }


    protected override void StateAction()
    {
        //base.StateAction();
        switch (m_BombState)
        {
            case (bombState.Null):
                {
                    m_BombState = bombState.IDLE;
                    
                }
                break;
            case (bombState.IDLE):
                {
                    MotionIdle(WaitTime);
                }
                break;

            case (bombState.FLY):
                {
                    nav.enabled = false;
                    Debug.LogWarning("Hit over 3" + theTarget.name);
                    
                    
                    m_BombState = bombState.FLYDEAD;
                }
                break;

            case (bombState.FLYDEAD):
                {
                    MotionFlyDead();
                }
                break;

            case (bombState.DEAD):
                {
                    MotionDead();
                }
                break;

            default:

                break;
        }
    }
    protected override void MotionIdle(float w_time)
    {
        if (BeHitTime > 0)
        {
            m_BombState = bombState.FLY;
        }

        BombcurrentTime += Time.deltaTime;

        if (IsTimeOut())
        {
            DoBomb();
        }
    }

    bool IsTimeOut()
    {
        return BombcurrentTime > (BombTime * 2) ? true : false;
    }

    private void MotionFlyDead()
    {
        aIGameManger.StartCoroutineEvent(FlyExplosion());

        aIGameManger.ReleaseSoilderAI(this, 3);
    }

    private IEnumerator FlyExplosion()
    {
        float startTime = 0;
        float shakeTime = Random.Range(.5f, 1.0f);
        while (startTime < shakeTime)
        {
            Role.transform.Translate(Random.Range(-shake, shake), 0.0f, Random.Range(-shake, shake));
            Role.transform.Rotate(0.0f, Random.Range(-shake * 100, shake * 100), 0.0f);
            startTime += Time.deltaTime;
            yield return null;
        }
        particileControl.Play(3, 1, Role.transform.position);
        yield return new WaitForSeconds(.5f);
        Role.GetComponent<Rigidbody>().AddExplosionForce(power, theTarget.transform.position, radious, upforce, ForceMode.Impulse);
        particileControl.Play(4, 1, Role.transform.up, Role.transform);
        float rotattime = 0;
        float rotateEnd = 2f;
        float xangle = Random.Range(0, 180), yangle = Random.Range(0, 180), zangle = Random.Range(0, 180);
        
        while (rotattime<rotateEnd)
        {
            rotattime += Time.deltaTime;
            //Role.transform.Rotate(xangle, yangle, zangle);
            //Role.transform.localScale -= new Vector3(3, 3, 3) * rotattime;
            Role.transform.Rotate(xangle, yangle, zangle);
        }
        
    }

    public override void MotionDead()
    {
        //aIGameManger.StartCoroutineEvent(HitShader(.3f));
        Explocion();
        aIGameManger.StartCoroutineEvent(ExecuteAfterTime(3));
        aIGameManger.DamageAround(this, 25, 30);
        aIGameManger.ReleaseSoilderAI(this, 3);

    }

    private IEnumerator ExecuteAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        //CameraShaker.Instance.ShakeOnce(4f, 4f, 1f, 1f);
        cameraShake.UseShake = true;
        yield return new WaitForSeconds(delay);
        cameraShake.UseShake = false;

    }

    void DoBomb()
    {
        //Role.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
        //if (Role.transform.localScale.x > 2)
        //{
        //    m_aiState = AIState.DEAD;
        //}
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            m_BombState = bombState.DEAD;
        }

    }

    void Explocion()
    {
        GameObject bomb = particileControl.PlayBomb(2, 3F);

        Debug.Log("Cb");

    }

    public override IEnumerator HitShader(float waitTime)
    {
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", .3f);
        }
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {

            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
        }
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", .3f);
        }

        yield return new WaitForSeconds(waitTime/2);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {

            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
        }
        yield return new WaitForSeconds(waitTime/2);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {
            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", .3f);
        }
        yield return new WaitForSeconds(waitTime/2);
        for (int i = 0; i < RenderGamObjs.Length; i++)
        {

            RenderGamObjs[i].GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0);
        }
    }
}
