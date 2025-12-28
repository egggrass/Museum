Shader"Custom/RetroBW_URP"
{
    Properties
    {
        _Intensity ("Intensity", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
Name"RetroBWPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    float2 uv : TEXCOORD0;
};

            TEXTURE2D(_CameraColorTexture);
            SAMPLER(sampler_CameraColorTexture);

float _Intensity;

Varyings Vert(Attributes input)
{
    Varyings output;
    output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv = input.uv;
    return output;
}

half4 Frag(Varyings input) : SV_Target
{
    float4 col = SAMPLE_TEXTURE2D(
                    _CameraColorTexture,
                    sampler_CameraColorTexture,
                    input.uv
                );

    float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
    col.rgb = lerp(col.rgb, gray.xxx, _Intensity);

    return col;
}
            ENDHLSL
        }
    }
}
