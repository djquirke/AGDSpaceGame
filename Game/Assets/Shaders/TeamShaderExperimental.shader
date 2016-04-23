Shader "AA-TEAM/TeamShaderExperimental" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _Color ("1 Color", Color) = (1,1,1,1)
    _Color2 ("2 Color", Color) = (1,1,1,1)
    _Value("as",Float) = 0.2
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 600
   
CGPROGRAM
#pragma surface surf BlinnPhong vertex:vert
#pragma target 3.0
 
sampler2D _MainTex;
fixed4 _Color;
fixed4 _Color2;


struct Input {   
	float2 uv_MainTex;
    float3 viewDir;
    float4 screenPos;
};


float _Value;
void vert (inout appdata_full v) {
	//v.vertex.xyz += v.normal * IN.screenPos.x;
}
      
      
      
void surf (Input IN, inout SurfaceOutput o) {
/*
 	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
 	fixed4 texMask = tex2D(_EnergyMask, IN.uv_EnergyMask); 	
 	fixed4 texRamp = tex2D(_EnergyRamp, float2(IN.uv_MainTex.x+(_Time*10).x,IN.uv_MainTex.y));
 	texRamp = -texRamp * -1 ; 	
 	texMask = texMask * texRamp;
 	tex = tex*(-texMask + 1 );
 	texMask = _EnergyColor * texMask;
 	tex = tex + texMask;
 	o.Albedo = tex.rgb * _Color.rgb;
 	o.Emission = texMask;
 	
 	fixed4 SpecTex = tex2D(_SpecularTex, IN.uv_MainTex);
    o.Gloss = SpecTex;
    fixed4 Glosstex = tex2D(_RoughnessTex, IN.uv_MainTex);
    o.Specular = Glosstex;
    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
    o.Alpha = tex.a * _Color.a;
    */
   
    
    o.Albedo = _Color2;
    float Sin = _SinTime.r;
    Sin = Sin * 0.95;
    Sin = Sin + 0.001;   
    if(Sin < 0) Sin = -Sin;
    if(tex2D(_MainTex, IN.uv_MainTex).r<(Sin) && tex2D(_MainTex, IN.uv_MainTex).r+0.1>(Sin)){
	    	o.Albedo = _Color; 	
    }
    
    	
    	
    //if(tex2D(_MainTex, IN.uv_MainTex).r>0.5)
    //	o.Albedo = -(IN.screenPos.z*0.1)+1;
    //else
    //	o.Albedo = _Color;
}
ENDCG
} 
FallBack "Bumped Specular"
}