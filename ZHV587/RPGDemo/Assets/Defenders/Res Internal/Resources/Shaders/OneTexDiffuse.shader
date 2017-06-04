Shader "Custom/OneTexDiffuse" 
{
	Properties
	{
		_tex1 ("Base (RGB)", 2D) = "white" {}
		_light("light",Range (0,1.0)) = 0.0
	}
	
	SubShader 
	{
		LOD 200
		Tags 
		{
			"IgnoreProjector" = "True"
			"RenderType" = "Opaque"
			//"CastShadows" = "True"				
		}
		



		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _tex1;
		fixed _light;
		
		struct Input 
		{
			fixed4 color:COLOR;
			float2 uv_tex1;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c;
			c = tex2D(_tex1,IN.uv_tex1);
			fixed3 l = _light;
			o.Albedo = c + l;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}