using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticileControl : MonoBehaviour {
    public GameObject[] Particiles;

    public void Play(int  index ,float destorytime,Vector3 pos)
    {
        PlayParticile(index, destorytime, pos);
    }

    private void PlayParticile(int index , float time,Vector3 pos)
    {
        GameObject go = Instantiate(Particiles[index], pos, Quaternion.identity);
        Destroy(go.gameObject, time);
    }

    public void Play(int index, float destorytime, Vector3 pos,Transform parent)
    {
        PlayParticile(index, destorytime, pos, parent);
    }
    private void PlayParticile(int index, float time, Vector3 pos,Transform parent)
    {
        GameObject go = Instantiate(Particiles[index], pos, Quaternion.identity,parent);
        Destroy(go.gameObject, time);
    }

    public void ContinePlay(int index, float time, Vector3 pos,float delayTime)
    {
        StartCoroutine(PlayParticle(index, time, pos, delayTime));
    }
    private IEnumerator PlayParticle(int index, float time, Vector3 pos,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject go = Instantiate(Particiles[index], pos, Quaternion.identity);
        Destroy(go.gameObject, time);
    }

    public GameObject PlayBomb(int index, float time)
    {
        return PlayBombParticile(index, time);
    }
    private GameObject PlayBombParticile(int index, float time)
    {
        GameObject go = Instantiate(Particiles[index], this.transform.position,new Quaternion());
        //go.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
        //Destroy(go.gameObject, time);
        return go;
    }
}
