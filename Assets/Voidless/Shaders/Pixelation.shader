Shader "Voidless/Pixelation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width("Width", float) = 64
        _Height("Height", float) = 64
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Width;
            float _Height;
            float _dx;
            float _dy;
            float _p;

            fixed4 frag (v2f i) : SV_Target
            {
                _p = _Width * _Height;
                _dx = (_Width / _p);
                _dy = (_Height / _p);

                float2 coord = float2(_dx * floor(i.uv.x / _dx), _dy * floor(i.uv.y / _dy));

                return tex2D(_MainTex, coord);
            }
            ENDCG
        }
    }
}
