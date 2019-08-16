using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class RefCameraInfo : CinemachineExtension
{
    [Tooltip("Amplitude of the Info")]
    public Vector3 CameraPos;
    public Quaternion CameraRotation;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body )
        {
            //Vector3 shakeAmount = GetOffset();
            //CameraPos = state.PositionCorrection;
            CameraPos = state.CorrectedPosition;
            CameraRotation = state.CorrectedOrientation;
        }
    }

    //Vector3 GetOffset()
    //{
    //    // Note: change this to something more interesting!
    //    return new Vector3(
    //        Random.Range(-m_Range, m_Range),
    //        Random.Range(-m_Range, m_Range),
    //        Random.Range(-m_Range, m_Range));
    //}
}
