Shader "Custom/LitToon"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess("Shininess", Range(0, 1)) = 0.5
        _LightIntensity("Light Intensity", Range(0, 1)) = 1.0
        _ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };
            
            float4 _Color;
            float4 _SpecularColor;
            float _Shininess;
            float _LightIntensity;
            float _ShadowIntensity;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                return o;
            }
            
            sampler2D _MainTex;
            
            half4 frag (v2f i) : SV_Target
            {
                half3 normal = normalize(i.normal);
                half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                half NdotL = max(0.0, dot(normal, lightDir));
                
                // Specular reflection
                half3 viewDir = normalize(i.viewDir);
                half3 halfDir = normalize(lightDir + viewDir);
                half specular = pow(max(0.0, dot(normal, halfDir)), _Shininess);
                
                // Toon shading threshold
                half threshold = 0.5;
                half3 toonColor = (NdotL > threshold) ? _Color.rgb : half3(0, 0, 0);
                
                half3 albedo = tex2D(_MainTex, i.texcoord).rgb;
                
                half3 finalColor = (albedo * toonColor + specular * _SpecularColor.rgb) * _LightIntensity;
                finalColor = lerp(finalColor, finalColor * _ShadowIntensity, 0.5); // Apply shadow
                
                return half4(finalColor, 1);
            }
            ENDCG
        }
    }
}
