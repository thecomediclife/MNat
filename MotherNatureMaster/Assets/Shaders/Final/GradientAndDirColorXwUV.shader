Shader "Custom/GradientAndDirColorXwUV" {
	Properties {
		_MainTex ("Decal Texture", 2D) = "white" {}
		_ColorLow ("Color Low", COLOR) = (1,1,1,1)
      	_ColorHigh ("Color High", COLOR) = (1,1,1,1)
      	_xPosLow ("X Pos Low", Float) = 0
      	_xPosHigh ("X Pos High", Float) = 10
      	_GradientStrength ("Graident Strength", Float) = 1
      	_EmissiveStrengh ("Emissive Strengh ", Float) = 1
      	_ColorX ("Color X", COLOR) = (1,1,1,1)
	    _ColorY ("Color Y", COLOR) = (1,1,1,1)
	    _MainTexMoveSpeedU ("U Move Speed", Range(-50,50)) = 0
		_MainTexMoveSpeedV ("V Move Speed", Range(-50,50)) = 0
	}
	
	SubShader {
		Tags { 
      		"Queue" = "Geometry"
      		"RenderType"="Opaque" 
      	}
		
		CGPROGRAM
		#pragma surface surf Lambert
      	#define WHITE3 fixed3(1,1,1)
      	#define UP float3(0,1,0)
      	#define RIGHT float3(1,0,0)
      	
      	sampler2D _MainTex;
      	fixed4 _ColorLow;
      	fixed4 _ColorHigh;
      	fixed4 _ColorX;
      	fixed4 _ColorY;
      	half _xPosLow;
      	half _xPosHigh;
      	half _GradientStrength;
      	half _EmissiveStrengh;
      	fixed _MainTexMoveSpeedU;
		fixed _MainTexMoveSpeedV;
      
		struct Input {
			float2 uv_MainTex;
         	float3 worldPos;
         	float3 normal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			//	UV scrolling
			fixed2 MainTexMoveScrolledUV = IN.uv_MainTex;
			fixed MainTexMoveU = _MainTexMoveSpeedU * _Time;
			fixed MainTexMoveV = _MainTexMoveSpeedV * _Time;
			MainTexMoveScrolledUV += fixed2(MainTexMoveU, MainTexMoveV);
			
			fixed4 decal = tex2D(_MainTex, MainTexMoveScrolledUV);
		
        	// 	Gradient color
         	half3 gradient = lerp(_ColorLow, _ColorHigh,  smoothstep( _xPosLow, _xPosHigh, IN.worldPos.x )).rgb;
         	gradient = lerp(WHITE3, gradient, _GradientStrength);		
         	
         	//	Directional color
         	half3 finalColor = _ColorX.rgb * max(0,dot(o.Normal, RIGHT))* _ColorX.a;		// Add ColorX if the normal is facing positive X-ish (right)
         	finalColor += _ColorY.rgb * max(0,dot(o.Normal, UP)) * _ColorY.a;				// Add ColorY if the normal is facing positive Y-ish (up)
         
         	//	Combine gradient color, directional color, and UV scrolling
         	finalColor += gradient;															// Add the gradient color
         	finalColor = saturate(finalColor);												// Scale down to 0-1 values
         	finalColor = lerp(finalColor, decal.rgb, decal.a);								// Splits the pixels between the decal texture and gradient color
			
			// 	Finalize
         	o.Emission = lerp(half3(0,0,0), finalColor, _EmissiveStrengh);					// How much should go to emissive. 0 = diffuse color only (requires lighting to be lit)         	
         	o.Albedo = finalColor * saturate(1 - _EmissiveStrengh);							// the "color" before lighting is applied
			o.Alpha = 1;																	// opaque
		}
		ENDCG
	} 
  	fallback "Vertex Lit"
}
