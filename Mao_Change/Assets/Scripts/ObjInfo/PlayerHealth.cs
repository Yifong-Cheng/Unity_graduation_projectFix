using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    public int hp,currenthp,theLastHp;
    bool IsDead;
    int EvadePrecent=50;

    public AIStateController stateController;
    public AIStateController targetAiController;

    public Slider slider;

    public bool NoAiStateController,IsStageChallange;

    //0426 proprity
    public string Proprity;
    private const string HeavyProprity = "Heavy";
    private const string MediumProprity = "Medium";
    private const string GentleProprity = "Gentle";

    private NewMiniMap minimap;

    private void Awake()
    {
        currenthp = hp;
        theLastHp = hp;
    }

    // Use this for initialization
    void Start () {
        if (IsStageChallange)
        {
            slider = transform.GetComponentInChildren<Slider>();
            minimap = GameObject.FindObjectOfType<NewMiniMap>();
        }

    }
	
	// Update is called once per frame
	void Update () {
        
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    int ep = Random.Range(0, 80);
        //    TakeDamage(1, ep);
        //    Debug.Log("t");
        //}
        if(IsStageChallange)
        {
            if(IsDead)
            {
                GameObject.FindObjectOfType<BoxBackageUIControl>().FailState();
                Debug.Log("GameOver");
            }
            else
            {
                //slider.transform.LookAt(Camera.main.transform);
                //slider.value = currenthp;
                ShowHpUI();
                if(currenthp < 1)
                {
                    slider.fillRect.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/syringeUI/syringe_ui_in", typeof(Sprite));
                }
                /*else if(currenthp>30&& currenthp<80)
                {
                    slider.fillRect.GetComponent<Image>().color = Color.yellow;
                }*/
                else
                {
                    slider.fillRect.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("Image/syringeUI/syringe_ui_in2", typeof(Sprite));
                }
                if(currenthp!=theLastHp)
                {
                    theLastHp = currenthp;
                    minimap.StartCoroutine(minimap.DoFade());
                }
                else
                {

                }
            }
        }
        else
        {
            if (NoAiStateController)
            {
                DoAnim1();
            }
            else
            {
                DoAnim();
            }
        }
    }

    public void TakeDamage(int d, int ep)
    {
        Debug.Log("ShootTake");
        if (currenthp > 0 && ep >= EvadePrecent && !IsDead)
        {
            currenthp -= d;
            if (currenthp <= 0 && !IsDead)
            {
                IsDead = true;

            }
        }
        else if (currenthp > 0 && ep <= EvadePrecent && !IsDead)
        {
            //miss
            Avoid();

        }
        else if (currenthp <= 0 && IsDead)
        {
            //stateController.m_aiState = AIStateController.AIState.DEAD;
            //Destroy(this.gameObject, 1);
        }

    }

    //public void TakeDamage(int d, int ep,AIStateController AttackAI,string _proprity)
    //{
    //    Debug.Log("take");
    //    if (currenthp > 0 && ep >= EvadePrecent)
    //    {
    //        CheckProperity(_proprity,ref d);
    //        currenthp -= d;
    //        targetAiController = AttackAI;
    //        stateController.theTarget = targetAiController.Role;
    //        if (currenthp <= 0 && IsDead != true)
    //        {
    //            IsDead = true;

    //        }
    //    }
    //    else if (currenthp > 0 && ep <= EvadePrecent)
    //    {
    //        //miss
    //        Avoid();

    //    }
    //    else if (currenthp <= 0 && IsDead == true)
    //    {
    //        //stateController.m_aiState = AIStateController.AIState.DEAD;
    //        //Destroy(this.gameObject, 1);
    //        //stateController.PlayAnim("Dead", -1.0f);
    //    }

    //}
    public void TakeDamage(int d, AIStateController AttackAI, string _proprity)
    {
        Debug.Log("take");
        if (currenthp > 0)
        {
            CheckProperity(_proprity, ref d);
            currenthp -= d;
            targetAiController = AttackAI;
            stateController.theTarget = targetAiController.Role;
            if (currenthp <= 0 && IsDead != true)
            {
                IsDead = true;

            }
        }
        else if (currenthp <= 0 && IsDead == true)
        {
            //stateController.m_aiState = AIStateController.AIState.DEAD;
            //Destroy(this.gameObject, 1);
            //stateController.PlayAnim("Dead", -1.0f);
        }

    }

    private void CheckProperity(string _proprity , ref int damage)
    {
        switch(Proprity)
        {
            case "Gentle":
                if(_proprity==GentleProprity)
                {
                    damage = damage;
                }
                else if(_proprity==MediumProprity)
                {
                    damage =  (2)*damage;
                }
                else if(_proprity==HeavyProprity)
                {
                    damage =  5*damage;
                }
                break;
            case "Medium":
                if(_proprity==GentleProprity)
                {
                    damage = damage / 2;
                }
                else if(_proprity == MediumProprity)
                {
                    damage = damage;
                }
                else if(_proprity==HeavyProprity)
                {
                    damage = 3 * damage;
                }
                break;
            case "Heavy":
                if(_proprity==GentleProprity)
                {
                    damage = damage / 3;
                }
                else if(_proprity==MediumProprity)
                {
                    damage = 3 * damage;
                }
                else if(_proprity==HeavyProprity)
                {
                    damage = 2*damage;
                }

                break;

        }
    }

    private void DoAnim()
    {
        if(currenthp!=theLastHp&& currenthp>0)
        {
            ////this.transform.GetChild(1).GetComponent<Animator>().SetBool("BeAttack", true);
            if(stateController.m_anim!=null)
            {
                //stateController.m_anim.SetBool("BeAttack", true);
                stateController.PlayAnim("BeAttack",0f);

                stateController.BeAttack = true;
            }
            
            theLastHp = currenthp;
            //stateController.BeAttack = false ;
        }
        else
        {
            if(stateController.m_anim != null)
            {
                ////this.transform.GetChild(1).GetComponent<Animator>().SetBool("BeAttack", false);
                //stateController.m_anim.SetBool("BeAttack", false);
            }
            stateController.BeAttack = false;
        }
    }

    private void DoAnim1()
    {
        if (currenthp != theLastHp)
        {
            //this.transform.GetChild(1).GetComponent<Animator>().SetBool("BeAttack", true);
            GetComponent<Animator>().SetBool("BeAttack", true);
            theLastHp = currenthp;

        }
        else if (currenthp == theLastHp)
        {
            //this.transform.GetChild(1).GetComponent<Animator>().SetBool("BeAttack", false);
            GetComponent<Animator>().SetBool("BeAttack", false);

        }
    }

    private void ShowHpUI()
    {
        slider.value = currenthp;
    }

    void Avoid()
    {
        //this.transform.position -= Vector3.forward * Time.deltaTime*5;
        //this.GetComponent<AILocomotion>().velocity-= Vector3.forward * Time.deltaTime * 5;

        //stateController.m_aiState = AIStateController.AIState.Avoid;//doavoidanim

        Debug.Log("Avoid Attack");
    }
}
