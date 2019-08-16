using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force { public float m_Time = 0; public Vector4 m_Force; public Force(Vector4 force) { m_Force = force; } }
public class GrassForce : MonoBehaviour
{
    public List<Force> m_ForceList = null;
    public float m_WaveFrequency = 6.0f;
    public float m_Resistance = 0.25f;
    public float m_MaxForceMagnitude = 6.0f;
    public float m_AddForceTimeInterval = 0.5f;
    public int m_MaxForceNum = 3;
    private float m_LastAddTime = 0;
    private Material material;
    private MeshRenderer m_render;
    void Start() {
        m_render = GetComponent<MeshRenderer>();
        material = m_render.sharedMaterial;
    }
    void Update() {
        UpdateForce();
    }
    void OnBecameVisible() {
        enabled = true;
    }
    void OnBecameInvisible() {
        enabled = false;
    }
    public void AddForce(Vector3 force)
    {
        if (Time.time - m_LastAddTime > m_AddForceTimeInterval)
        {
            m_LastAddTime = Time.time;
            if (m_ForceList == null) m_ForceList = new List<Force>();
            if (m_ForceList.Count < m_MaxForceNum)
            {
                Vector4 newForce = new Vector4(force.x, 0, force.z, 0);
                if (newForce.magnitude > m_MaxForceMagnitude)
                    newForce = newForce.normalized * m_MaxForceMagnitude;
                m_ForceList.Add(new Force(newForce));
            }
        }
    }
    private void UpdateForce()
    {
        if (m_ForceList == null) return; Vector4 accForce = Vector4.zero; for (int i = m_ForceList.Count - 1; i >= 0; --i)
        {
            if (m_ForceList[i].m_Force.magnitude > 0.1f)
            {
                // [-1, 1] 正玄波模拟简谐运动                   
                float wave_factor = Mathf.Sin(m_ForceList[i].m_Time * m_WaveFrequency);
                // 力的指数衰减                        
                float resistance_factor = easeOutExpo(1, 0, m_Resistance * Time.deltaTime);
                m_ForceList[i].m_Force *= resistance_factor;
                m_ForceList[i].m_Time += Time.deltaTime;
                // 累加                    
                accForce += m_ForceList[i].m_Force * wave_factor;
            }
            else
            {
                m_ForceList.RemoveAt(i);
            }
        }
        if (accForce != Vector4.zero)
        {
            if (material.HasProperty("_Force"))
            {
                accForce = transform.InverseTransformVector(accForce);
                // 世界空间转换到模型本地空间
                material.SetVector("_Force", accForce);
            }
        }
    }
    public float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }

    public static void AddForceToGrass(int forceId, Transform transform)
    {
        //ForceTable force = ForceTableMgr.Instance.GetDataById(forceId);
        //if (force != null)
        //{
        //    Vector3 relativeCenter = new Vector3(force.RelativeCenterX, force.RelativeCenterY, force.RelativeCenterZ);
        //    Vector3 center = transform.TransformPoint(relativeCenter);
        //    //Vector3 size = new Vector3(force.Length, force.Height, force.Width);
        //    Vector3 size = new Vector3(force.Width, force.Height, force.Length);

        //    // 方向矩阵
        //    Matrix4x4 m44 = Matrix4x4.TRS(Vector3.zero, Quaternion.Inverse(transform.rotation), Vector3.one);

        //    PhysicsUtil.AddForceToGrass((RangeType)force.RangeType, (ForceDirType)force.DirType, force.Strength, center, size, transform.forward, m44, force.Degree);
        //}
    }

    //private static void AddForceToGrass(RangeType type, ForceDirType dirType, float strength, Vector3 center, Vector3 size, Vector3 direction, Matrix4x4 m44, float degree = 360.0f)
    //{
    //    if (type == RangeType.Sphere)
    //    {
    //        AddForceInSector(dirType, strength, center, size.x, direction, degree);
    //    }
    //    else if (type == RangeType.Cude)
    //    {
    //        AddForceInCube(dirType, strength, center, size, direction, m44, degree);
    //    }
    //}

}
