Shader "Custom/TransparentWallGlobal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 0.5
        _Fade ("Fade", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Fade;

            // Propiedad global para la posición del jugador
            float3 _GlobalPlayerPosition;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = TransformObjectToWorld(v.positionOS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 worldToPlayer = i.worldPosition - _GlobalPlayerPosition;
                float dist = length(worldToPlayer);

                // Smooth fade
                float alpha = saturate((dist - _Radius) / _Fade);

                half4 texColor = tex2D(_MainTex, i.uv);
                texColor.a *= alpha; // Apply transparency
                return texColor;
            }
            ENDHLSL
        }
    }
}