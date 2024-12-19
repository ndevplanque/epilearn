Shader "Custom/LeafShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.13, 0.55, 0.13, 1.0)
        _NoiseScale ("Noise Scale", Float) = 5.0
        _AmbientColor ("Ambient Color", Color) = (0.2, 0.3, 0.2, 1.0)
        _LightDirection ("Light Direction", Vector) = (0.0, 1.0, 0.0)
        _Transparency ("Transparency", Float) = 0.5
        _SpecularIntensity ("Specular Intensity", Float) = 0.3
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
            float _Transparency;
            float _SpecularIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _NoiseScale;
                o.normal = v.normal;
                return o;
            }

            float noise(float2 coord)
            {
                return frac(sin(dot(coord.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float n = noise(i.uv);
                
                // Normal perturbation
                float3 norm = normalize(i.normal);
                float lightIntensity = dot(norm, _LightDirection);
                lightIntensity = max(lightIntensity, 0.0);

                // Adjust leaf color with noise and ambient color
                float3 leafColor = _BaseColor.rgb * n + _AmbientColor.rgb * (1.0 - n);
                
                // Specular effect
                float specular = pow(max(dot(norm, _LightDirection), 0.0), 16) * _SpecularIntensity;
                leafColor += specular;

                // Adding transparency to simulate leaf thickness
                leafColor *= _Transparency;

                return float4(leafColor, 1.0);
            }
            ENDCG
        }
    }
}
