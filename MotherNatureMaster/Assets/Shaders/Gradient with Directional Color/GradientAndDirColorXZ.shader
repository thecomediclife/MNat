Shader "Custom/GradientAndDirColorXZ" {
	Properties {
		_DecalTex ("Decal Texture", 2D) = "white" {}
		_ColorLow ("Color Low", COLOR) = (1,1,1,1)
      	_ColorHigh ("Color High", COLOR) = (1,1,1,1)
      	_ColorY ("Color Y", COLOR) = (1,1,1,1)
      	_xzPosLow ("XZ Pos Low", Float) = 0
      	_xzPosHigh ("XZ Pos High", Float) = 10
      	_GradientStrength ("Graident Strength", Float) = 1
      	_EmissiveStrengh ("Emissive Strengh ", Float) = 1
	}
	
	SubShader {
		Tags { 
      		"Queue" = "Geometry"
      		"RenderType"="Opaque" 
      	}
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0
      	#define WHITE3 fixed3(1,1,1)
      	#define UP float3(0,1,0)
      	#define DIAG float3(1,0,1)
      	
      	sampler2D _DecalTex;
      	fixed4 _ColorLow;
      	fixed4 _ColorHigh;
      	fixed4 _ColorY;
      	half _xzPosLow;
      	half _xzPosHigh;
      	half _GradientStrength;
      	half _EmissiveStrengh;
      
		struct Input {
			float2 uv_DecalTex;
//         	float3 worldPos;
         	float3 localPos;
         	float3 normal;
		};
		
		void vert (inout appdata_full v, out Input o) {
		 	UNITY_INITIALIZE_OUTPUT(Input,o);
		   	o.localPos = v.vertex.xyz;
		 }

		void surf (Input IN, inout SurfaceOutput o) {
			
			fixed4 decal = tex2D(_DecalTex, IN.uv_DecalTex);
		
        	// 	gradient color in the diagonal XZ direction
         	half3 gradient = lerp(_ColorLow, _ColorHigh,  smoothstep( _xzPosLow, _xzPosHigh, IN.localPos.x)).rgb;
         	gradient = lerp(WHITE3, gradient, _GradientStrength);		
         	
         	//	assign color coming in at the top
         	half3 finalColor = _ColorY.rgb * max(0,dot(o.Normal, UP)) * _ColorY.a;				// Add ColorY if the normal is facing positive Y-ish (up)
         
         	finalColor += gradient;															// Add the gradient color
         	finalColor = saturate(finalColor);												// Scale down to 0-1 values
         	finalColor = lerp(finalColor, decal.rgb, decal.a);								// Splits the pixels between the decal texture and gradient color

         	o.Emission = lerp(half3(0,0,0), finalColor, _EmissiveStrengh);					// How much should go to emissive. 0 = diffuse color only (requires lighting to be lit)         	
         	o.Albedo = finalColor * saturate(1 - _EmissiveStrengh);							// the "color" before lighting is applied
			o.Alpha = 1;																	// opaque
		}
		ENDCG
	} 
  	fallback "Vertex Lit"
}
