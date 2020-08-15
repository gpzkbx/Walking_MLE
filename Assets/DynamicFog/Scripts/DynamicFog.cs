using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DynamicFogAndMist
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [HelpURL("http://kronnect.com/taptapgo")]
    [ImageEffectAllowedInSceneView]

    public class DynamicFog : DynamicFogBase
    {

        float lifetime = 0.5f; // added
        float delta = 0; //added

        // Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (fogMat == null || _alpha == 0 || currentCamera == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (shouldUpdateMaterialProperties)
            {
                shouldUpdateMaterialProperties = false;
                UpdateMaterialPropertiesNow();
            }

            if (currentCamera.orthographic)
            {
                if (!matOrtho)
                    ResetMaterial();
                fogMat.SetVector("_ClipDir", currentCamera.transform.forward);
            }
            else
            {
                if (matOrtho)
                    ResetMaterial();
            }

            if (_useSinglePassStereoRenderingMatrix && UnityEngine.XR.XRSettings.enabled)
            {
                fogMat.SetMatrix("_ClipToWorld", currentCamera.cameraToWorldMatrix);
            }
            else
            {
                fogMat.SetMatrix("_ClipToWorld", currentCamera.cameraToWorldMatrix * currentCamera.projectionMatrix.inverse);
            }
            Graphics.Blit(source, destination, fogMat);
        }

        public void Enable_my_self()
        {
            enabled = true;
            this.delta = 0;
            //Debug.Log("OK");
        }

        public void ChangeNoiseStrength(float noise_strength)
        {
            
            noiseStrength = noise_strength;
            Debug.Log("noise_strength" + noise_strength);
        }

        public void Initialize()
        {
            alpha = 1.0f;
            skySpeed = 0f;
            skyHaze = 0f;
            skyNoiseStrength = 0f;
            skyAlpha = 0f;
            distance = 0;
            distanceFallOff = 0f;
            height = 20f;
            heightFallOff = 0.5f;
            turbulence = 0;
            noiseStrength = 0f;
            noiseScale = 0f;
            speed = 0f;
            color = new Color(0.89f, 0.89f, 0.89f, 1);
            color2 = color;
            maxDistance = 1.2f;
            maxDistanceFallOff = 0f;
        }

        void Start()
        {
        }

        void Update()
        {
            this.delta += Time.deltaTime;
            if (this.delta > this.lifetime)
            {
                enabled = false;
            }
        }

    }

}