Shader "Unlit/RandomdotLeft"
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
            float _Mul1;
            float _Mul2;
            float _Mul3;
            float _NoiseRate;
			float _RandomDotSeed;


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
                //fixed noise_shift_gain = 0.005;
                float screenres = _ScreenParams.x;
                //fixed noise_shift = random(i.uv, 10) * 2 - 1;

                /*
                no need to change the image of the left eye
                //calculate the amount of shift depending on noise rate
                fixed shift;
                fixed ran = random(i.uv, 20); //TODO how to decide ran (random value from 0 ~ 1)
                if (ran < _NoiseRate)
                {
                    shift = floor(noise_shift * noise_shift_gain * screenres) / screenres;
                }
                else
                {
                    shift = 0;
                }
                */


                fixed4 renderTex = tex2D(_MainTex, i.uv);
                //fixed mysample = random_dot((i.uv + float2(shift, 0)) * screenres, 0);
                fixed mysample = random_dot(i.uv * screenres, _RandomDotSeed);
                renderTex.rgb = mysample;
                return renderTex;
            }
        ENDCG
        }
    }
    FallBack off
}
