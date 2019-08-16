using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UITool {
    private static GameObject m_CanvasObj = null;// 場景上的2D畫布物件

    public static void ReleaseCanvas()
    {
        m_CanvasObj = null;
    }

    // 找尋限定在Canvas畫布下的UI界面
    /// <summary>
    /// 用來找ui物件，兩個變數，一個是要找的UI物件，一個是該UI的Canvas的名字
    /// </summary>
    /// <param name="UIName"></param>
    /// <param name="UICanvasName"></param>
    /// <returns></returns>
    public static GameObject FindUIGameObject(string UIName, string UICanvasName)
    {
        if (m_CanvasObj == null)
            m_CanvasObj = UnityTool.FindGameObject(UICanvasName);
        if (m_CanvasObj == null)
            return null;
        return UnityTool.FindChildGameObject(m_CanvasObj, UIName);//
    }

    // 取得UI元件
    public static T GetUIComponent<T>(GameObject Container,string UIName) where T:UnityEngine.Component
    {
        GameObject ChildGameObject = UnityTool.FindChildGameObject(Container, UIName);
        if (ChildGameObject == null)
            return null;

        T tempObj = ChildGameObject.GetComponent<T>();
        if(tempObj==null)
        {
            Debug.LogWarning("元件[" + UIName + "]不是[" + typeof(T) + "]");
            return null;
        }
        return tempObj;

    }

    public static Button GetButton(string BtnName, string UICanvasName)
    {
        // 取得Canvas
        GameObject UIRoot = GameObject.Find(UICanvasName);
        if(UIRoot == null)
        {
            Debug.LogWarning("場景上沒有UI Canvas");
            return null;
        }

        // 找出對應的Button
        Transform[] allChildren = UIRoot.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren)
        {
            if(child.name == BtnName)
            {
                Button tmpBtn = child.GetComponent<Button>();
                if (tmpBtn == null)
                    Debug.LogWarning("UI原件[" + BtnName + "]不是Button");
                return tmpBtn;
            }
        }
        Debug.LogWarning("UI Canvas中沒有Button[" + BtnName + "]存在");
        return null;
    }

    // 取得UI元件
    public static T GetUIComponent<T>(string UIName,string UICanvasName) where T : UnityEngine.Component
    {
        // 取得Canvas
        GameObject UIRoot = GameObject.Find(UICanvasName);
        if (UIRoot == null)
        {
            Debug.LogWarning("場景上沒有UI Canvas");
            return null;
        }
        return GetUIComponent<T>(UIRoot, UIName);
    }
}
