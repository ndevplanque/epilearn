Shader "Custom/WoodShader"
{
    Properties
    {
        _MainColor ("Base Color", Color) = (0.54, 0.27, 0.07, 1.0)
        _LightWoodColor ("Light Wood Color", Color) = (0.8, 0.6, 0.4, 1.0)
        _DarkWoodColor ("Dark Wood Color", Color) = (0.4, 0.2, 0.1, 1.0)
        _NoiseScale ("Noise Scale", Float) = 5.0
        _RingFrequency ("Ring Frequency", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float4 _LightWoodColor;
            float4 _DarkWoodColor;
            float _NoiseScale;
            float _RingFrequency;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _NoiseScale;
                return o;
            }

            float noise(float2 coord)
            {
                return frac(sin(dot(coord.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            float rings(float2 coord, float frequency)
            {
                float radius = length(coord - 0.5);
                return abs(sin(radius * frequency));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float ringPattern = rings(i.uv, _RingFrequency);
                float colorVariation = noise(i.uv);
                float3 woodColor = lerp(_LightWoodColor.rgb, _DarkWoodColor.rgb, colorVariation);
                return float4(woodColor * ringPattern, 1.0);
            }
            ENDCG
        }
    }
}