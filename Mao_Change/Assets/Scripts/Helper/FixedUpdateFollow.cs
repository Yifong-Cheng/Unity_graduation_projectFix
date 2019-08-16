using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(9999)]
public class FixedUpdateFollow : MonoBehaviour
{
    public Transform toFollow;

    public Transform OriginalPos, ShootPos, BoostPos, HandPos;

    public void ReturnPos()
    {
        transform.parent = OriginalPos;
        transform.position = OriginalPos.transform.position;
        transform.rotation = OriginalPos.transform.rotation;
    }

    public void UsePos()
    {
        transform.parent = ShootPos;
        transform.position = ShootPos.transform.position;
        transform.rotation = ShootPos.transform.rotation;
    }

    public IEnumerator Shoot()
    {
        transform.parent = HandPos;
        transform.position = HandPos.transform.position;
        transform.rotation = HandPos.transform.rotation;
        yield return new WaitForSeconds(.5f);
        transform.parent = ShootPos;
        transform.position = ShootPos.transform.position;
        transform.rotation = ShootPos.transform.rotation;
        yield return new WaitForSeconds(.3f);
        transform.parent = HandPos;
        transform.position = HandPos.transform.position;
        transform.rotation = HandPos.transform.rotation;
    }

    public void Boost()
    {
        transform.parent = BoostPos;
        transform.position = BoostPos.transform.position;
        transform.rotation = BoostPos.transform.rotation;
    }
}
