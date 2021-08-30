// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Celeritas/Scrollable"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Texture("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                sampler2D _Texture;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float4 _Texture_ST;

                void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
                {
                    //rotation matrix
                    Rotation = Rotation * (3.1415926f / 180.0f);
                    UV -= Center;
                    float s = sin(Rotation);
                    float c = cos(Rotation);

                    //center rotation matrix
                    float2x2 rMatrix = float2x2(c, -s, s, c);
                    rMatrix *= 0.5;
                    rMatrix += 0.5;
                    rMatrix = rMatrix * 2 - 1;

                    //multiply the UVs by the rotation matrix
                    UV.xy = mul(UV.xy, rMatrix);
                    UV += Center;

                    Out = UV;
                }

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color * _Color;
                    return OUT;
                }

                float nrand(float x, float y)
                {
                    return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    //float2 rotUV;
                    //Unity_Rotate_Degrees_float(IN.texcoord, float2(0.5, 0.5), 45, rotUV);

                    IN.texcoord.y += _Time.x * 4;

                    half4 base = tex2D(_Texture, IN.texcoord) * _Color;

                    return half4(base);
                }
            ENDCG
            }
        }
}
