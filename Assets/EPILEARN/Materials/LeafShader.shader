Shader "Custom/LeafShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.13, 0.55, 0.13, 1.0)
        _NoiseScale ("Noise Scale", Float) = 5.0
        _AmbientColor ("Ambient Color", Color) = (0.2, 0.3, 0.2, 1.0)
        _LightDirection ("Light Direction", Vector) = (0.0, 1.0, 0.0)
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            float4 _BaseColor;
            float _NoiseScale;
            float4 _AmbientColor;
            float3 _LightDirection;
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
                float n = noise(i.uv);
                
                // Variation de couleur basée sur le bruit
                float3 leafColor = _BaseColor.rgb * n + _AmbientColor.rgb * (1.0 - n);
                
                // Intégration du motif circulaire pour le feuillage
                float finalColorIntensity = ringPattern * (1.0 - n) + n;
                
                return float4(leafColor * finalColorIntensity, 1.0);
            }
            ENDCG
        }
    }
}
