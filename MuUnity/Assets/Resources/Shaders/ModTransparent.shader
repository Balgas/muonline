Shader "Custom/ModTransparent" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 150
	Cull Off

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
