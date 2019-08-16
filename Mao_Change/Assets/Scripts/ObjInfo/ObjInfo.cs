using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInfo : MonoBehaviour {
    public string Type;
    public string State;
    public bool IsDead=false;
    public string Profession=null;
    public int FollowerTypeID;
    public int FactoryNum;
    public CombineEnemyAIStateController m_ai;

    public bool IsProp;
    public bool GetProp;
    public List<GameObject> InsideObj = new List<GameObject>();
    public string PropName;
}
