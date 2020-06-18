Shader "Custom/Water-WorldSpace"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Scale ("Texture Scale", Float) = 0.1
    }
    SubShader
    {
         Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }      
         LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        float _Scale;

        struct Input
        {
            float3 worldNormal;
            float3 worldPos;
            float opacity;

        };

    
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float2 UV;
            fixed4 c;

            if (abs(IN.worldNormal.x) > 0.5) {
                UV = IN.worldPos.yz;
                c = tex2D(_MainTex, UV * _Scale);
            }
            else if (abs(IN.worldNormal.z) > 0.5) {
                UV = IN.worldPos.xy;
                c = tex2D(_MainTex, UV * _Scale);
            }
            else {
                UV = IN.worldPos.xz;
                c = tex2D(_MainTex, UV * _Scale);
            }

            o.Albedo = c.rgb * _Color;
        }
        ENDCG
    }
    FallBack "VertexLit"
}
