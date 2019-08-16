using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DistortionPostEffect : MonoBehaviour
{
    private Camera distortionCam;
    public Material DistortionMat;
    private RenderTexture rt;
    void Awake()
    {
        Transform go = transform.Find("Distortion");
        if (null == go)
        {
            go = (new GameObject("Distortion")).transform;
        }
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = Quaternion.identity;


        distortionCam = go.GetComponent<Camera>();
        if (null == distortionCam)
        {
            distortionCam = go.gameObject.AddComponent<Camera>();
        }
        distortionCam.clearFlags = CameraClearFlags.Color;
        distortionCam.backgroundColor = Color.black;
        //rt = RenderTexture.GetTemporary (Screen.width / 2, Screen.height / 2);
        rt = RenderTexture.GetTemporary(200, 100);
        rt.wrapMode = TextureWrapMode.Repeat;
        distortionCam.targetTexture = rt;
        distortionCam.cullingMask = LayerMask.GetMask("Distortion");

        gameObject.GetComponent<Camera>().cullingMask &= (~distortionCam.cullingMask);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        DistortionMat.SetTexture("_DistortionMask", rt);
        Graphics.Blit(src, dest, DistortionMat);
    }


}