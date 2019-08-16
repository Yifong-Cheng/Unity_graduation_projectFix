using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaoTreeControl : MonoBehaviour
{
    public string MaoTreeType;
    public GameObject Art;
    public GrowMaoControl growMaoControl;
    private GameObject parent;
    private ParticileControl particileControl;
    //public GameObject Player;
    public GameObject showInRange;
    public GameObject showTutorial;
    public LoadAndSave data;
    public BoxBackageUIControl bc;
    public GameObject Player;

    public bool NearPlayer;
    public bool CreatFinish ;
    public void Initialized(string type)
    {
        particileControl = GetComponent<ParticileControl>();
        MaoTreeType = type;
        growMaoControl = Art.GetComponent<GrowMaoControl>();
        //growMaoControl = _growMaoControl;
        growMaoControl.seed = Resources.Load<GameObject>("SeedMao/" + MaoTreeType);
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            growMaoControl.ShootPos.Add(this.transform.GetChild(0).GetChild(i).gameObject);
        }
        CreatFinish = true;
    }
    //private bool PlayerInRange()
    //{
    //    RaycastHit hit;
    //    if (Physics.SphereCast(this.transform.position, 5, this.transform.forward, out hit, 5))
    //    {
    //        if (hit.collider.CompareTag("Player"))
    //        {
    //            Debug.Log("Near Player" + ": " + hit.transform.name);
    //            return true;
    //        }

    //    }
    //    return false;
    //}
    private void Start()
    {
        bc = GameObject.FindObjectOfType<BoxBackageUIControl>();
    }

    private void Update()
    {
        if(NearPlayer)
        {
            if(growMaoControl.growState!=0)
            {
                showInRange.SetActive(true);
                showTutorial.SetActive(true);

            }
            else if(growMaoControl.growState==5)
            {
                showInRange.SetActive(false);
                showTutorial.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Space) && bc.PropInfo() == "GUN")
            {
                if(Player.GetComponent<NewPlayerController>().m_anim.GetCurrentAnimatorStateInfo(0).IsName("Boost"))
                {

                }
                else
                {
                    //Player.GetComponent<NewPlayerController>().PlayAnim("Boost", 0f);
                    Player.GetComponent<NewPlayerController>().DoBoost();
                }
                //if(Vector3.Distance(Player.transform.position,this.transform.position)<.8f)
                //{
                //    Vector3 treePos = new Vector3(this.transform.position.x, .5f, this.transform.position.z);
                //    Player.transform.LookAt(treePos);
                //}
                //else
                //{
                //    Vector3 treePos = new Vector3(this.transform.position.x, Player.transform.position.y, this.transform.position.z);
                //    Player.transform.LookAt(treePos);
                //}
                //HideAndShowTubs(1);
                GetDoBoost();
            }
            else
            {
                //m_anim.SetBool("Boost", false);
            }
        }
        else
        {
            showInRange.SetActive(false);
            showTutorial.SetActive(false);
        }
    }
    public void GetDoBoost()
    {
        if (growMaoControl.growState == 1)
        {
            growMaoControl.currentBoostIndex++;
            //growMaoControl.ScaleModel();
        }
        particileControl.Play(0, 2, this.transform.position + new Vector3(0, 0.5f, 0));
    }

    private void OnTriggerStay(Collider other)
    {}
}
