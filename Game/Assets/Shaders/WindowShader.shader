Shader "AA-TEAM/WindowShader" {
	Properties {
		_Cube ("Cubemap", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		samplerCUBE _Cube;

		struct Input {
          float3 worldRefl;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			
			o.Albedo = texCUBE (_Cube, float3(IN.worldRefl.x+(_SinTime.a*0.01),(IN.worldRefl.y+(_SinTime.a*0.02)),IN.worldRefl.z+(_SinTime.a*0.05))).rgb;
			//IN.worldRefl*_SinTime
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
