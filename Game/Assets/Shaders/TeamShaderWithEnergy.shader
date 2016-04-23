Shader "AA-TEAM/TeamShaderWithEnergy" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _SpecularTex ("Roughness", 2D) = "white" {}
    _RoughnessTex ("Gloss", 2D) = "white" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _EnergyColor ("Main Color", Color) = (1,1,1,1)
    _EnergyMask ("_EnergyMask", 2D) = "white" {}
    _EnergyRamp ("_EnergyRamp", 2D) = "white" {}
    _Speed("Speed",float) = 5
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 600
   
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0
 
sampler2D _MainTex;
sampler2D _SpecularTex;
sampler2D _RoughnessTex;
sampler2D _BumpMap;
sampler2D _Occlusion;
fixed4 _Color;
half _Shininess;
half _OcclusionPWR;
fixed4 _EnergyColor;
sampler2D _EnergyMask;
sampler2D _EnergyRamp;
float _Speed;

struct Input {
    float2 uv_MainTex;
    float2 uv_EnergyMask;
    float2 uv_EnergyRamp;
    float2 uv_BumpMap;
    float3 viewDir;
};
void surf (Input IN, inout SurfaceOutput o) {
 	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
 	
 	o.Albedo = tex.rgb * _Color.rgb;
 	fixed4 Energy = 0;
 	float Sin = _Time*_Speed;
 	float SinMinus = 0.0;
 	for(int ticker = 0; ticker < Sin-1; ticker += 1){
 	SinMinus += 1.0;
 	}
 	Sin = Sin - SinMinus;
    Sin = Sin * 0.99;
    Sin = Sin + 0.001;   
    if(Sin < 0) Sin = -Sin;
    if(tex2D(_EnergyRamp, IN.uv_MainTex).r<(Sin) && tex2D(_EnergyRamp, IN.uv_MainTex).r+0.1>(Sin)){
	    	Energy = _EnergyColor; 	
    }
    Energy = Energy*tex2D(_EnergyMask, IN.uv_MainTex);
    o.Albedo += Energy;
    
    
 	fixed4 SpecTex = tex2D(_SpecularTex, IN.uv_MainTex);
    o.Gloss = SpecTex;
    fixed4 Glosstex = tex2D(_RoughnessTex, IN.uv_MainTex);
    o.Specular = Glosstex;
    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
    o.Alpha = tex.a * _Color.a;
}
ENDCG
} 
FallBack "Bumped Specular"
}