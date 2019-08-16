using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducationStage1 : MonoBehaviour {
    public int EducationState = 0;
    public GameObject player;
    public Text Text_Desprition;
    public string Desprition;
    public GameObject ShowBoard;
    public GameObject CloseBtn;
    public GameObject nextBtn;
    public GameObject EDU_SignTarget;
    public List<Transform> EduTransforms = new List<Transform>();

    private float currentTime;
    private float WaitTime;

    private float height = 50;

    private bool ChageState = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        gameObject.transform.LookAt(Camera.main.transform);
        transform.position = new Vector3( player.transform.position.x,player.transform.position.y+height,player.transform.position.z);
        StateDecide();
    }

    private void StateDecide()
    {
        EDU_SignTarget.transform.Rotate(0, 3, 0);
        Text_Desprition.text = Desprition;
        switch (EducationState)
        {
            case 0:

                {
                    Desprition = "HI"+"\n" + "\n" + " WELCOME TO THE MAO'S WORLD ";
                    nextBtn.gameObject.SetActive(true);
                    CloseBtn.SetActive(false);
                    //StartCoroutine(State0(1));
                }
                break;

            case 1:

                {
                    Desprition = "試著按下" + "\n" + "\n" + "W A S D 鍵移動看看吧 ";
                    CloseBtn.SetActive(true);

                }
                break;

            case 2:

                {
                    WaitForTry(8);
                }
                break;

            case 3:

                {
                    Desprition = "嘗試完移動了" + "\n" + "\n" + "相信你也發現滑鼠也是有甚麼功能的吧 ";
                    ShowBoard.SetActive(true);
                    
                    CloseBtn.SetActive(true);
                }
                break;

            case 4:

                {
                    WaitForTry(4);
                }
                break;

            case 5:

                {
                    Desprition = "是不是覺得視角很討厭" + "\n" + "\n" + "按按看 <<TAB>> 鍵吧 " + "\n" + "會有驚喜喔!!";

                    ShowBoard.SetActive(true);
                    
                    CloseBtn.SetActive(true);
                }
                break;

            case 6:

                {
                    WaitForTry(6);
                }
                break;

            case 7:

                {
                    Desprition = "看到鼠標上的紅色圓圈了吧" + "\n" + "\n" ;

                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 8:

                {
                    Desprition = "沒看到?" + "\n" + "\n" + " " + "\n" + "試著轉動滾輪看看!!";

                    ShowBoard.SetActive(true);
                    
                    CloseBtn.SetActive(true);
                }
                break;

            case 9:
                {
                    WaitForTry(6);
                }
                break;

            case 10:
                {
                    Desprition = "哎呀!!" + "\n" + "\n" + "相信你也看到旁邊有隻圓圓的可愛小生物了 " + "\n" + "他是這個世界原生的物種之一" + "\n" + "他有個可愛的名字叫<<小毛>>";

                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 11:
                {
                    Desprition = "還有其他跟他一樣的原生毛種喔!!" + "\n" + "\n" + "不管了!!我們先過去把它撿起來吧 " + "\n" + "" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 12:
                {
                    Desprition = "將那個紅色的圈圈對準小毛!!" + "\n" + "\n" + "按下<<E>>鍵及滑鼠左鍵就可以讓她跟著你囉~~ " + "\n" + "" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            case 13:
                {
                    WaitForTry(10);
                }
                break;

            case 14:
                {
                    Desprition = "接下來讓我們看看這裡還有甚麼驚喜可以讓我們發掘" + "\n" + "\n" + "" + "\n" + "" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 15:
                {
                    Desprition = "看到畫面上的箭頭了嗎" + "\n" + "\n" + "沒看到?" + "\n" + "沒關係~" + "\n" + "等一下關掉這討厭的教學就看的到了!!";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 16:
                {
                    EDU_SignTarget.transform.position = EduTransforms[0].transform.position;
                    
                    EDU_SignTarget.SetActive(true);

                    Desprition = "把我們滑鼠的紅色圈圈移到箭頭下面的物件" + "\n" + "\n" + "那個東西叫做天然毛孔" + "\n" + "可以無性生殖這裡的原生物種喔" + "\n" + "然後按下滑鼠左鍵吧";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            case 17:
                {
                    WaitForTry(15);
                }
                break;

            case 18:
                {
                    //EDU_SignTarget.SetActive(false);
                    Desprition = "" + "\n"  + "將產生的小毛和原來的小毛召喚回來吧!!" + "\n" + "" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            case 19:
                {
                    WaitForTry(8);
                }
                break;

            case 20:
                {
                    EDU_SignTarget.transform.position = EduTransforms[1].transform.position;
                    Desprition = "接下來" + "\n" + "看到箭頭底下的紅色孢子了嗎?" + "\n" + "你可以嘗試派小毛過去幫你檢過來" + "\n" + "或者自己走過去將它撿起來";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);

                }
                break;

            case 21:
                {
                    WaitForTry(12);
                }
                break;

            case 22:
                {
                    Desprition = "那紅色的孢子" + "\n" + "其實是小毛的一種養分來源" + "\n" + "你可以用它來產生小毛" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 23:
                {
                    Desprition = "等一下關掉教學的時候" + "\n" + "你可以用滑鼠點自己看看" + "\n" + "那邊會顯示你擁有的材料" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            case 24:
                {
                    WaitForTry(12);
                }
                break;

            case 25:
                {
                    EDU_SignTarget.transform.position = EduTransforms[2].transform.position;
                    Desprition = "我們朝下一個目標前進吧" + "\n" + "教學就快結束了" + "\n" + "再忍耐一下就好" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;


            case 26:
                {
                    WaitForTry(30);
                }
                break;

            case 27:
                {
                    Desprition = "箭頭指的是工廠" + "\n" + "可以用來產生小毛或其他種類的毛種" + "\n" + "不管是產生小毛還是其他毛種都是奧消耗能量的" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 28:
                {
                    Desprition = "小毛只要消耗最單純的能量" + "\n" + "就是剛剛我們撿到的孢子" + "\n" + "其他毛種就要看特殊需求了" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 29:
                {
                    Desprition = "" + "\n" + "試著產生一些小毛吧!!" + "\n" + "" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            case 30:
                {
                    WaitForTry(12);
                }
                break;

            case 31:
                {
                    Desprition = "基礎教學差不多就到這裡了" + "\n" + "後面的路上你會看到其他的毛種" + "\n" + "將他們也加入你的隊伍行列吧" + "\n" + "";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 32:
                {
                    Desprition = "等你收服了更多的毛種以後" + "\n" + "你可以按" + "\n" + "<<Q>>鍵" + "\n" + "切換派出的毛種";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(true);
                    CloseBtn.SetActive(false);
                }
                break;

            case 33:
                {
                    Desprition = "最後面會有幾隻小怪在等著你" + "\n" + "可以讓你體驗遊戲的整個流程" + "\n" + "真正的遊戲會在下一關開始" + "\n" + "感謝你耐心接受我們的教學";
                    ShowBoard.SetActive(true);
                    nextBtn.SetActive(false);
                    CloseBtn.SetActive(true);
                }
                break;

            default:
                break;
        }
    }

    //IEnumerator WaitState(float speed)
    //{

    //    yield return new WaitForSeconds(speed);
    //    EducationState++;
    //    //StopCoroutine(WaitState(8));
    //}

    private void WaitForTry(float time)
    {
        currentTime += Time.deltaTime;
        if (currentTime > time)
        {
            if (ChageState == false)
            {
                ChageState = true;
                EducationState++;
            }

        }
    }

    public void CloseTheBoard()
    {
        nextBtn.gameObject.SetActive(false);

        ShowBoard.SetActive(false);
        //StartCoroutine(WaitState(8));
        currentTime = 0;
        ChageState = false;
        EducationState++;

    }

    public void NextState()
    {
        EducationState++;
        nextBtn.SetActive(false);
    }
}
