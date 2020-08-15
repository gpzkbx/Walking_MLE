using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderRandomdotLeft : MonoBehaviour {

    #region Variables
    public Shader curShader;
    public float mul1 = 12.9898f;
    public float mul2 = 78.233f;
    public float mul3 = 43758.55f;
    float noiseRate = 0f; //visual noise
    float random_dot_seed = 0f;
    private Material screenMat;

    float lifetime = 0.5f;
    float delta; //count the time since enabled (only live lifetime)
    #endregion

    #region Properties
    Material ScreenMat
    {
        get
        {
            if (screenMat == null)
            {
                screenMat = new Material(curShader);
                screenMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return screenMat;
        }
    }
    #endregion

    //noise switching function
    public void SwitchNoise(float noise_rate)
    {
        noiseRate = noise_rate;
        //Debug.Log("noise_rate = " + noiseRate);
    }

    public void SwitchRDSeed(float Seed)
    {
        random_dot_seed = Seed;
    }

    //enable function
    public void Enable_my_self()
    {
        enabled = true;
        this.delta = 0;
    }

    // Use this for initialization
    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (!curShader && !curShader.isSupported)
        {
            enabled = false;
        }
        //enabled = false;
        delta = 0;
    }

    private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (curShader != null)
        {
            ScreenMat.SetFloat("_Mul1", mul1);
            ScreenMat.SetFloat("_Mul2", mul2);
            ScreenMat.SetFloat("_Mul3", mul3);
            ScreenMat.SetFloat("_NoiseRate", noiseRate);
            ScreenMat.SetFloat("_RandomDotSeed", random_dot_seed);
            Graphics.Blit(sourceTexture, destTexture, ScreenMat);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.lifetime)
        {
            enabled = false;
        }
    }

    void OnDisable()
    {
        if (screenMat)
        {
            DestroyImmediate(screenMat);
        }
    }
}
