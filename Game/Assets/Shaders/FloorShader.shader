Shader "AA-TEAM/FloorShader" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_SpecularTex ("Roughness", 2D) = "white" {}
		_RoughnessTex ("Gloss", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}
	SubShader {
    Tags { "RenderType"="Opaque" "Queue"="Transparent"}
    LOD 600
   
CGPROGRAM
#pragma surface surf BlinnPhong 
#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SpecularTex;
		sampler2D _RoughnessTex;
		sampler2D _BumpMap;
fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);	
			o.Albedo = tex.rgb * _Color.rgb;
			fixed4 SpecTex = tex2D(_SpecularTex, IN.uv_MainTex);
			o.Gloss = SpecTex;
			fixed4 Glosstex = tex2D(_RoughnessTex, IN.uv_MainTex);
			o.Specular = Glosstex;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
