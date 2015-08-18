Shader "Tutorial/Basic" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)	
	}
	
	SubShader {
		Pass {
			CGPROGRAM
				//	Tells unity to switch coding language from ShaderLab to CG
				//	CG code doesn't know what the variables are, that's why we need to define them here
			
			//	pragmas
			#pragma vertex vert
			#pragma fragment frag
				//	These are instructions which tell Unity what to look for and where
				//	vert and frag are the struct functions we created below
			
			// 	user defined variables
			float4 _Color;		
			
			//	base input structs
			struct vertexInput {
				//	an object comes with a lot of information (like normals, tangents,
				//	vertex coordinates, UV maps etc.). We can access that information
				//	and assign them variables. 
				
				float4 vertex : POSITION;
				//	Here we assigned position information from the object and assign
				//	it to a float4 variable vertex.
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				//	This is required in vertexOut to convert the input position into 
				//	a world space that Unity can understand. 
				//	SV_ is added because of some change Microsoft added for DirectX 11. 
			};
			
			//	vertex function
			vertexOutput vert(vertexInput v) {
				//	We created the struct vertexOutput above in the base input structs. 
				//	In this function, it will take in the vertexInput, then return the 
				//	output back into the struct vertexOutput. 
				//	This is how a function is written in CG. vert() is the name of the
				//	function. vertexInput is the input assigned with a variable v. That
				//	way we don't have to write vertexInput everytime and can just write
				//	v instead.  
				
				vertexOutput o;								//	Same as saying "float 4o = vertexOutput" in java
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				//	mul = multiply
				//	What this is doing is moving the object's position into the Unity
				//	matrix position so that Unity can understand it. 
				//	How it works if written in code:
				//	UNITY_MATRIX_MVP xyzw
				//	v.vertex xyzw
				//	UNITY_MATRIX_MVP1.xyzw * v.vertex.x
				//	UNITY_MATRIX_MVP2.xyzw * v.vertex.y
				//	UNITY_MATRIX_MVP3.xyzw * v.vertex.z
				//	UNITY_MATRIX_MVP4.xyzw * v.vertex.w
				//	The matrix has 4 floats in it, and we're going to multiply each of
				//	them by the vertex cordinate
				
				return o;
			}
			
			//	fragment function
			float4 frag(vertexOutput i) : COLOR {
				//	Create new function frag, using vertexOutput as the base and calling
				//	it as i. Tell Unity that it's a color with float4.
				return _Color;
				//	This function's output is put into _Color, which we defined above
			}
				
			
			ENDCG
		}
	}
	//	fallback commented out for now
	//	Fallback "Diffuse"
}
