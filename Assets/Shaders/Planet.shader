Shader "Custom/Planet"
{
	Properties
	{
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_AtmoColor("Atmosphere Color", Color) = (0.46,0.56,1.0,1.0)
		_AtmoPower("Atmosphere Power", Range(0.5,8.0)) = 1.0
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert
	
		struct Input
		{
			float3 viewDir;
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _Color;
		float4 _AtmoColor;
		float _AtmoPower;
			
		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _Color * tex2D(_MainTex, IN.uv_MainTex).rgb;
			
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Albedo += _AtmoColor.rgb * pow(rim, _AtmoPower);
		}

		ENDCG
	}
		Fallback "Diffuse"
}
