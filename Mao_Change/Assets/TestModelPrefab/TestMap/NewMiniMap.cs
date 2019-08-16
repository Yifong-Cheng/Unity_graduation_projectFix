using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMiniMap : MonoBehaviour {
    public Vector3 CenterPos;
    public GameObject TestCenter;
    public GameObject Player;
    public RectTransform _player;
    public List<GameObject> Trees;
    public Transform MiniMapTransform;
    public GameObject Tower;
    public RectTransform _tower;
    private GameObject alert, warning;

    [Header("地圖極限位置 : ")]
    public Vector3 Top, Bottom, Right, Left;
    [Header("圖片比例 :  ")]
    public float ImageScale;
    public float MapScale;
    private float ChangeScale;

    private Vector3 MiddleHight;
    private Vector3 MiddleWeight;

    private void LateUpdate()
    {
        ChangeDistance();
    }

    void ChangeDistance()
    {
        Vector2 playertransform = new Vector2(Player.transform.position.x-CenterPos.x, Player.transform.position.z-CenterPos.z);
        _player.GetComponent<RectTransform>().anchoredPosition = playertransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;
        //_player.rotation = new Quaternion(0,0, Player.transform.rotation.y,0);
        _player.rotation = Quaternion.Euler(0,0,Player.transform.rotation.y*(-180));

    }

    public IEnumerator DoFade()
    {
        if(!alert.activeInHierarchy)
        {
            alert.SetActive(true);
            yield return new WaitForSeconds(.2f);
            warning.SetActive(true);
            yield return new WaitForSeconds(.5f);
            alert.SetActive(false);
            warning.SetActive(false);
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Map/" + "ShowAlert"));
            Destroy(go, .5f);
        }
        else
        {
            yield return null;
        }
        

    }

    public void InitializedMapRange(GameObject _Player)
    {
        Player = _Player;
        ChangeScale = MapScale / ImageScale;
        MiddleHight = (Top + Bottom) / 2;
        MiddleWeight = (Right + Left) / 2;

        //CenterPos = (MiddleHight + MiddleWeight);
        //TestCenter.GetComponent<RectTransform>().anchoredPosition = CenterPos / ChangeScale;

        Vector2 playertransform =new Vector2 (Player.transform.position.x-CenterPos.x,Player.transform.position.z-CenterPos.z);
        _player.GetComponent<RectTransform>().anchoredPosition = playertransform/ ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        for(int i=0;i<trees.Length;i++)
        {
            Trees.Add(trees[i].gameObject);
            GameObject t;
            Vector2 treetransform = new Vector2(trees[i].transform.position.x - CenterPos.x, trees[i].transform.position.z - CenterPos.z);
            switch (trees[i].GetComponent<MaoTreeControl>().MaoTreeType)
            {
                case "S0":
                    t = Resources.Load<GameObject>("Map/" + "Tree0");
                    GameObject.Instantiate(t, MiniMapTransform);
                    t.GetComponent<RectTransform>().anchoredPosition = treetransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

                    break;
                case "S1":
                    t = Resources.Load<GameObject>("Map/" + "Tree1");
                    GameObject.Instantiate(t, MiniMapTransform);
                    t.GetComponent<RectTransform>().anchoredPosition = treetransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

                    break;
                case "S2":
                    t = Resources.Load<GameObject>("Map/" + "Tree2");
                    GameObject.Instantiate(t, MiniMapTransform);
                    t.GetComponent<RectTransform>().anchoredPosition = treetransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

                    break;
                case "S3":
                    t = Resources.Load<GameObject>("Map/" + "Tree3");
                    GameObject.Instantiate(t, MiniMapTransform);
                    t.GetComponent<RectTransform>().anchoredPosition = treetransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

                    break;
                case "S4":
                    t = Resources.Load<GameObject>("Map/" + "Tree4");
                    GameObject.Instantiate(t, MiniMapTransform);
                    t.GetComponent<RectTransform>().anchoredPosition = treetransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

                    break;
            }
            //_Trees.Add
        }

        Vector2 towertransform = new Vector2(Tower.transform.position.x - CenterPos.x, Tower.transform.position.z - CenterPos.z);
        _tower.GetComponent<RectTransform>().anchoredPosition = towertransform / ChangeScale + TestCenter.GetComponent<RectTransform>().anchoredPosition;

        alert = _tower.gameObject.transform.Find("alert").gameObject;
        warning = _tower.gameObject.transform.Find("Warning").gameObject;
        alert.SetActive(false);
        warning.SetActive(false);
    }
}
