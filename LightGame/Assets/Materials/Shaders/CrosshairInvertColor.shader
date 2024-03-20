Shader "Custom/InvertColor"
{
    Properties {
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Tags { "Queue"="Overlay" "IgnoreProjector"="True" }
            Blend OneMinusDstColor OneMinusSrcColor
            ColorMask RGB
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float grayscale = dot(texColor.rgb, float3(0.299, 0.587, 0.114));
                if(texColor.a == 0) return float4(0, 0, 0, 0); // Keep transparent pixels
                else if(grayscale < 0.01) return _TintColor; // If black pixel, invert it with tint
                else return 1 - texColor; // If not black pixel, just invert its colors without tint
            }
            ENDCG
        }
    }
}
