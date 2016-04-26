Shader "AA-TEAM/Hubworld_GlassReflections" {

	 Properties {
         _Color ("Color", Color) = (1,1,1,1)
		 _Cube ("Cubemap", CUBE) = "" {}
		_Amount ("Cubemap Amount", Range(0,1)) = 0.0
     }
     SubShader {
         Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
         CGPROGRAM
         #pragma surface surf Lambert alpha
         
         struct Input {
                float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
			float3 worldRefl;
			INTERNAL_DATA
        };
         
        float4 _Color;
		samplerCUBE _Cube;
		float _Amount;
         void surf (Input IN, inout SurfaceOutput o) {
		 fixed3 refection = texCUBE (_Cube, float3(IN.viewDir.x+(_SinTime.a*1),(-IN.viewDir.y+(_SinTime.a*1)),IN.viewDir.z+(_SinTime.a*1))).rgb*_Amount;
             o.Albedo = refection;
             o.Alpha = refection;
             o.Gloss = 0.0;
         }
         ENDCG
     }
     Fallback "VertexLit"
	
}
