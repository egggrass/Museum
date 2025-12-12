Shader"Custom/BlackWhiteRetro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

sampler2D _MainTex;
float4 _MainTex_ST;

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
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float3 col = tex2D(_MainTex, i.uv).rgb;
    float gray = dot(col, float3(0.3, 0.59, 0.11));

                // 添加复古效果：增加噪点 + 对比
    gray = pow(gray, 0.8);
    return fixed4(gray, gray, gray, 1);
}
            ENDCG
        }
    }
}