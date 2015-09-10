Shader "Custom/Rain" {
	Properties {
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_MainTexMoveSpeedU ("U Move Speed", Range(-50,50)) = 0
		_MainTexMoveSpeedV ("V Move Speed", Range(-50,50)) = 0
	}
	SubShader {
		Tags {"RenderType" = "Transparent"}
		LOD 200

		CGPROGRAM
//		#pragma surface surf Lambert
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		fixed _MainTexMoveSpeedU;
		fixed _MainTexMoveSpeedV;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		
			fixed2 MainTexMoveScrolledUV = IN.uv_MainTex;
			
			fixed MainTexMoveU = _MainTexMoveSpeedU * _Time;
			fixed MainTexMoveV = _MainTexMoveSpeedV * _Time;
			
			MainTexMoveScrolledUV += fixed2(MainTexMoveU, MainTexMoveV);
		
			half4 c = tex2D (_MainTex, MainTexMoveScrolledUV);

			o.Albedo = c.rgb;
			o.Alpha = tex2D (_MainTex, MainTexMoveScrolledUV).a;
		}
		ENDCG
	} 
}