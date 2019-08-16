using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureAction : MonoBehaviour {
    private ParticileControl particileControl;
    private GameObject medicine;
    Vector3 ParticlePos;

    private void OnEnable()
    {
        EventManager.StartListening("CureFinish", CureFinish);
    }

    private void OnDisable()
    {
        EventManager.StopListening("CureFinish", CureFinish);
    }
    // Use this for initialization
    void Start () {
        ParticlePos = this.transform.position;
        ParticlePos.y = 0;
        ParticlePos += new Vector3(0, 1, 0);
        medicine = transform.GetChild(3).gameObject;
        particileControl = GetComponent<ParticileControl>();
        StartCoroutine(MedicineInPut(3f, .05f));
       
        particileControl.Play(1, 3f, ParticlePos + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)));
        particileControl.Play(1, 3f, ParticlePos + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
        particileControl.Play(1, 3f, ParticlePos + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CureFinish()
    {
        GameObject.FindObjectOfType<StageChallange>().PassChallange += 1;
        particileControl.Play(0, 8, ParticlePos);
        //EventManager.TriggerEvent("Spawn");
        //StartCoroutine(DeadEnemy(1f));
        EventManager.TriggerEvent("DeadEnemy");
        Destroy(this.gameObject, .3f);
        //sound
        //if(teach.Instance.FirstFinsh == false)
        //{
        //    teach.Instance.SkipTeach = false;
        //    teach.Instance.TeachLevel = 156;
        //}

        //-----
        EventManager.TriggerEvent(" ShowChallangeInfo");
        
    }

    private IEnumerator DeadEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.TriggerEvent("DeadEnemy");
    }

    public IEnumerator MedicineInPut(float delayTime , float downRange)
    {
        //yield return new WaitForSeconds(delayTime);
        //if (medicine.transform.localScale.y <= 0.1f)
        //{

        //    Debug.Log("PassStage");
        //    //aIGameManger.DeadAllEnemy();
        //    //timeline
        //    //IsPass = true;
        //    //stageData.m_datainfolist.stageData.Pass = IsPass;



        //    EventManager.TriggerEvent("CureFinish");
        //    StopAllCoroutines();
        //    //if (PlaceObj != null)
        //    //{
        //    //    PlaceObj.SetActive(true);
        //    //}

        //}
        //else if (medicine.transform.localScale.y > 0.1f)
        //{

        //    medicine.transform.localScale -= new Vector3(0, downRange, 0);
        //    StartCoroutine(MedicineInPut(delayTime, downRange));
        //}
        //yield return new WaitForSeconds(delayTime);
        float currentCut=0;
        while(medicine.transform.localScale.y > 0.1f)
        {
            yield return new WaitForSeconds(delayTime);
            //currentCut = Time.deltaTime;
            medicine.transform.localScale -= new Vector3(0, downRange, 0);
        }

        EventManager.TriggerEvent("CureFinish");
    }


}
