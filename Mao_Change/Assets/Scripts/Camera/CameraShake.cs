using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// An add-on module for Cinemachine to shake the camera
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraShake : CinemachineExtension {

    [Tooltip("Amplitude of the shake")]
    public float m_Range = 0.5f;
    public bool UseShake;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && UseShake)
        {
            Vector3 shakeAmount = GetOffset();
            state.PositionCorrection += shakeAmount;
        }
    }

    Vector3 GetOffset()
    {
        // Note: change this to something more interesting!
        return new Vector3(
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range, m_Range));
    }

    #region Oldshake
    //static protected List<CameraShake> s_Cameras = new List<CameraShake>();
    //public const float k_PlayerHitShakeAmount = 0.05f;
    //public const float k_PlayerHitShakeTime = 0.4f;

    //protected float m_ShakeAmount;
    //protected float m_RemainingShakeTime;

    //protected CinemachineVirtualCameraBase m_CinemachineVCam;
    //protected bool m_IsShaking = false;
    //protected Vector3 m_OriginalLocalPosition;

    //private void Awake()
    //{
    //    m_CinemachineVCam = GetComponent<CinemachineVirtualCameraBase>();
    //}

    //private void OnEnable()
    //{
    //    s_Cameras.Add(this);
    //}

    //private void OnDisable()
    //{
    //    s_Cameras.Remove(this);
    //}

    //private void LateUpdate()
    //{
    //    if (m_IsShaking)
    //    {
    //        m_CinemachineVCam.LookAt.localPosition = m_OriginalLocalPosition + Random.insideUnitSphere * m_ShakeAmount;

    //        m_RemainingShakeTime -= Time.deltaTime;
    //        if (m_RemainingShakeTime <= 0)
    //        {
    //            m_IsShaking = false;
    //            m_CinemachineVCam.LookAt.localPosition = m_OriginalLocalPosition;
    //        }
    //    }
    //}

    //private void StartShake(float amount, float time)
    //{
    //    if (!m_IsShaking)
    //    {
    //        m_OriginalLocalPosition = m_CinemachineVCam.LookAt.localPosition;
    //    }

    //    m_IsShaking = true;
    //    m_ShakeAmount = amount;
    //    m_RemainingShakeTime = time;
    //}

    //static public void Shake(float amount, float time)
    //{
    //    for (int i = 0; i < s_Cameras.Count; ++i)
    //    {
    //        s_Cameras[i].StartShake(amount, time);

    //    }
    //}

    //void StopShake()
    //{
    //    m_OriginalLocalPosition = m_CinemachineVCam.LookAt.localPosition;
    //    m_IsShaking = false;
    //    m_ShakeAmount = 0f;
    //    m_RemainingShakeTime = 0f;
    //}

    //public static void Stop()
    //{
    //    for (int i = 0; i < s_Cameras.Count; i++)
    //    {
    //        s_Cameras[i].StopShake();
    //    }
    //}
    #endregion

    #region no cinemachine
    //IEnumerator Shake(float duration, float magnitude)
    //{
    //    Vector3 originalPos = transform.localPosition;
    //    float elapsed = 0.0f;
    //    while (elapsed < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, originalPos.z);
    //        yield return null;
    //    }
    //    transform.localPosition = originalPos;

    //}
    #endregion

}
