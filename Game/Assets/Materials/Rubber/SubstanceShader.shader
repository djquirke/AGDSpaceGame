Shader "AA-TEAM/SubstanceShader" {
	Properties {
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Diffuse ("Diffuse", 2D) = "white" {}
		_Metalness ("Metalness", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "white" {}
		_Roughness ("Roughness", 2D) = "white" {}
		_Cube ("Cubemap", CUBE) = "" {}
		_CubeInt ("_Cube name", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 600
   
		CGPROGRAM
		#pragma surface surf BlinnPhong 
		#pragma target 3.0

	
		struct Input {
			float2 uv_Diffuse;
			float2 uv_BumpMap;
			float3 worldRefl;
          INTERNAL_DATA
		};
		sampler2D _Diffuse;
		sampler2D _Metalness;
		sampler2D _Roughness;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		float _CubeInt;
		void surf (Input IN, inout SurfaceOutput o) {
			
			fixed4 diffuse = tex2D (_Diffuse, IN.uv_Diffuse);
			fixed4 metalness = tex2D (_Metalness, IN.uv_Diffuse);
			fixed4 roughness = tex2D (_Roughness, IN.uv_Diffuse);
			fixed3 normals = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			fixed4 cubeTx = texCUBE (_Cube, WorldReflectionVector (IN, normals)); // This must be placed after the normals have been written :O Who knew! //

			o.Albedo = diffuse + 0.05 + cubeTx.rgb * (0.65 + -roughness.rgb);		
			o.Gloss = metalness;
			o.Specular = (1 + -roughness);
			o.Normal = normals;
			//o.Emission = cubeTx.rgb * (0.65 + -roughness.rgb);
		}
		ENDCG
	} 

FallBack "Reflective/VertexLit"

}
