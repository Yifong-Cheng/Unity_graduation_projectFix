using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestLeafControl0 : MonoBehaviour {

    //
    public Vector4 Wind = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);
    public float WindFrequency = 0.75f;

    private float WaveFrequency = 4.0f;
    private float WaveAmplitude = 0.1f;
    //

    private MeshRenderer renderer;
    private Material mat;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<MeshRenderer>();
        mat = renderer.sharedMaterial;
        //mat.SetColor("Base (RGB)",Color.red);

        //Shader.SetGlobalVector("Direction", Wind);
        //Shader.SetGlobalVector("_Wind_Simple", Wind);
    }
	
	// Update is called once per frame
	void Update () {
        //mat.SetVector("Direction", new Vector4(1, 0, 0, 0) * Time.deltaTime);

        Vector4 Wind1 = Wind * ((Mathf.Sin(Time.realtimeSinceStartup * WindFrequency)));
        Wind1.w = Wind.w;

        //// wind 2q
        ////Vector4 Wind2 = Wind1 * Wind1.w;
        //Vector4 Wind2 = new Vector4();
        //Wind2.x += Mathf.Sin(Time.realtimeSinceStartup * WaveFrequency) * WaveAmplitude;
        //Wind2.y = 0;
        //Wind2.z += Mathf.Sin(Time.realtimeSinceStartup * WaveFrequency + Mathf.PI * 0.5f) * WaveAmplitude;
        //Wind2.w = 0;

        //Shader.SetGlobalVector("Direction", Wind1);
        //Shader.SetGlobalVector("_Wind_Simple", Wind2);
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            mat.SetVector("Direction", Wind1);
        }

        

    }


}
