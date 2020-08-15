Shader "Unlit/RandomdotRIght"
{
	Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Luminosity("Luminosity", Range(0.0, 1)) = 1.0
        _Mul1 ("Mul1", float) = 12.9898
        _Mul2 ("Mul2", float) = 78.233
        _Mul3 ("Mul3", float) = 43758.55
        _NoiseRate ("NoiseRate", float) = 0
		_RandomDotSeed ("RandomDotSeed", float) = 0
		_NoiseShiftSeed ("NoiseShiftSeed", float) = 10
		_RanSeed ("RanSeed", float) = 20
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float _Mul1;
            float _Mul2;
            float _Mul3;
            float _NoiseRate;
			float _RandomDotSeed;
			float _NoiseShiftSeed;
			float _RanSeed;


            fixed random (fixed2 p, fixed seed)
            {
                fixed ran = frac(sin(dot(p, fixed2(_Mul1, _Mul2)) + floor(seed)) * _Mul3);
                return ran;
            }

            fixed random_dot (fixed2 uv, fixed seed)
            {
                fixed2 p = floor(uv);
                fixed ran = random(p, seed);
                if (ran > 0.1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            fixed4 frag(v2f_img i) :COLOR
            {
                fixed background_shift_gain = 0.06;
                fixed noise_shift_gain = 0.005; // should be 1/10 of the object_shift_gain
                fixed object_shift_gain = 0.05; //should be 0.1 if the ball is 5m from the eye (from rough calculation)
                float screenres = _ScreenParams.x;

                /*
                if (i.uv.x > 0.8 && i.uv.y > 0.8)
                    return 0.1;
                */
                

                //get the depth of the scene
                fixed depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));

                //calculate the amount of shift depending on depth information
                fixed noise_shift = random(i.uv, _NoiseShiftSeed);
                fixed shift;
                fixed ran = random(i.uv, _RanSeed); //to compare with the value of _NoiseRate
                if (ran < _NoiseRate)
                {
                    shift = floor((background_shift_gain - noise_shift_gain + noise_shift * (2 * noise_shift_gain + object_shift_gain)) * screenres) / screenres;
                }
                else
                {
                    shift = floor((background_shift_gain + depth * object_shift_gain) * screenres) / screenres;
                }

                //determine the value of each pixel
                fixed4 renderTex = tex2D(_MainTex, i.uv);
                fixed mysample = random_dot((i.uv + float2(shift, 0)) * screenres, _RandomDotSeed);
                renderTex.rgb = mysample;
                return renderTex;
            }
        ENDCG
        }
    }
    FallBack off
}
