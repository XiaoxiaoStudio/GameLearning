Shader "Custom/TwoTexDiffuse" 
{
	Properties
	{
		_tex1 ("Base (RGB)", 2D) = "white" {}
		_tex2 ("Base (RGB)", 2D) = "white" {}
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
		sampler2D _tex2;
		fixed _light;
		
		struct Input 
		{
			fixed4 color:COLOR;
			float2 uv_tex1;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c;
			if(IN.color.r <0.5)
			{
				c = tex2D(_tex1,IN.uv_tex1);
			}
			else
			{
				c = tex2D(_tex2,IN.uv_tex1);
			}
			fixed3 l = _light;
			o.Albedo = c + l;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
