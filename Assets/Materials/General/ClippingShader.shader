Shader "Custom/ClippingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0


        [HDR] _CutoffColor("Cutoff Color", Color) = (1,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float4 _Plane;
        float4 _CutoffColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float facing : VFACE;
        };

    

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float distance = dot(IN.worldPos, _Plane.xyz);
            distance = distance +_Plane.w;

            clip(-distance);

            float facing = IN.facing * 0.5 + 0.5;

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
