Shader "AA-TEAM/WindowShader" {
	Properties {
		_Cube ("Cubemap", CUBE) = "" {}
		_Amount ("Cubemap Amount", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		samplerCUBE _Cube;

		struct Input {
          float3 worldRefl;
		  float3 viewDir;
		};
	float _Amount;
		void surf (Input IN, inout SurfaceOutput o) {			
			o.Albedo = texCUBE (_Cube, float3(IN.viewDir.x+(_SinTime.a*0.01),(-IN.viewDir.y+(_SinTime.a*0.02)),IN.viewDir.z+(_SinTime.a*0.05))).rgb*_Amount;
			//IN.worldRefl*_SinTime
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
