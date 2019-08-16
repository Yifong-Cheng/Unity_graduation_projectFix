using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 01 
// 所有操控行為的基礎類別，
// 包含操空行為共有的變數和方法，
// 操控ai角色的尋找、逃跑等都可由此衍生
public abstract class Steering : MonoBehaviour
{
    //表示操控力的權數;
    public float weight = 1;
    public bool IsEnable;

    //計算操控力的方法，由衍生類別實現;
    public virtual Vector3 Force()
    {
        return new Vector3(0, 0, 0);
    }
}
