using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureInfo : MonoBehaviour {
    private LoadAndSave[] cureData;
    public int PassStage=1;
    public bool IsCureFinish;

    public Renderer[] renderObj;
    public int stagenum;

    public List<GameObject> CureFinish;
    private RefCameraInfo cinemachineCameraInfo;

    public List<GameObject> CureZhyin;

    [Header("sky star - ")]
    public GameObject Sky,starpos, Stars,startPfb;

    private void Awake()
    {
        cureData = GameObject.FindObjectsOfType<LoadAndSave>();
        GetPCureInfo();
        cinemachineCameraInfo = GameObject.FindObjectOfType<RefCameraInfo>();
        for(int i=0;i< CureZhyin.Count;i++)
        {
            CureZhyin[i].SetActive(false);
        }
    }
    
    void Start ()
    {
        for(int i=0;i<renderObj.Length;i++)
        {
            if (i == GameObject.FindObjectOfType<AIGameManger>().StageNum-1)
            {
                if(GameObject.FindObjectOfType<AIGameManger>().StageNum ==2)
                {
                    //renderObj[i].materials[1].SetColor("_OutlineColor", new Color(0, 255, 0));
                    //renderObj[i].materials[1].SetFloat("_Outline", .06f);
                }
                else
                {
                    //renderObj[i].materials[1].SetColor("_OutlineColor", new Color(0, 255, 0));
                    //renderObj[i].materials[1].SetFloat("_Outline", 2);
                }
                
            }

        }
        for(int i=0;i<PassStage;i++)
        {
            CureZhyin[i].SetActive(true);
        }
        
	}

    public void SetOnZhuyinModel()
    {
        //
        GetCureInfo();
        for(int i=0;i<CureZhyin.Count;i++)
        {
            //
            if(i==(PassStage)&&!IsCureFinish)
            {
                CureZhyin[i].SetActive(true);
                //Renderer renderer = CureZhyin[i].GetComponentInChildren<Renderer>();
                //renderer.materials[1].SetColor("_OutlineColor", new Color(0, 255, 0));
                //renderer.materials[1].SetFloat("_Outline", 1f);
            }
            else if(i!=(PassStage)&&!IsCureFinish)
            {
                CureZhyin[i].SetActive(false);
            }
            else if(i==PassStage&& IsCureFinish)
            {
                CureZhyin[i].SetActive(true);
            }
            else if(i!=PassStage && IsCureFinish)
            {
                CureZhyin[i].SetActive(false);
            }
        }
    }

    public void SetZhuyinRotation()
    {
        if(startPfb==null)
        {
            startPfb = GameObject.Instantiate(Stars, starpos.transform.position, starpos.transform.rotation, Sky.transform);
        }
        
        //transform.position = (cinemachineCameraInfo.CameraPos + new Vector3(0, -125, 0));
        transform.LookAt(cinemachineCameraInfo.CameraPos);
        //transform.rotation = cinemachineCameraInfo.CameraRotation;
        SetOnZhuyinModel();
    }

    public void DestoryStars()
    {
        Destroy(startPfb.gameObject);
    }

    public void GetCureInfo()
    {
        StageChallange stageChallange = GameObject.FindObjectOfType<StageChallange>();
        if(!stageChallange.IsPass)
        {
            PassStage = 0;
            if (stageChallange.PassChallange >= stageChallange.totalChallange - 1)
            {
                PassStage = stageChallange.totalChallange - 1;
                IsCureFinish = true;
            }
            else
            {
                PassStage = stageChallange.PassChallange;
            }
        }
        else
        {
            PassStage = stageChallange.totalChallange - 1;
            IsCureFinish = true;
        }
        
        

    }

    public void GetPCureInfo()
    {
        PassStage = 0;
        for(int i=0;i<cureData.Length;i++)
        {
            if(cureData[i].m_datainfolist.stageData.Pass)
            {
                PassStage = 4;
            }
        }
        //return PassStage;
    }
}
