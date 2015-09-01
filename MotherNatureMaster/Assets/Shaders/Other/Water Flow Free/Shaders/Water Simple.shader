Shader "Water Flow/Water Simple" {
	Properties {
		_DecalTex ("Base (RGB)", 2D) = "white" {}
		_DecalTexMoveSpeedU ("U Move Speed", Range(-50,50)) = 0
		_DecalTexMoveSpeedV ("V Move Speed", Range(-50,50)) = 0
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _DecalTex;
		fixed _DecalTexMoveSpeedU;
		fixed _DecalTexMoveSpeedV;

		struct Input {
			float2 uv_DecalTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		
			fixed2 DecalTexMoveScrolledUV = IN.uv_DecalTex;
			
			fixed DecalTexMoveU = _DecalTexMoveSpeedU * _Time;
			fixed DecalTexMoveV = _DecalTexMoveSpeedV * _Time;
			
			DecalTexMoveScrolledUV += fixed2(DecalTexMoveU, DecalTexMoveV);
		
			half4 c = tex2D (_DecalTex, DecalTexMoveScrolledUV);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
						
		}
		ENDCG
	} 
}