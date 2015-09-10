Shader "Custom/NormalLighting" {
     Properties {
         _ColorStart ("Light Color", Color) = (0.5,0.5,1,1)
         _ColorEnd ("Shadow Color", Color) = (0,0,1,1)
         _Dir ("Direction Light Direction", Vector) = (1,1,1,0)
     }
     SubShader {
         Pass {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma target 3.0
 
             #include "UnityCG.cginc"
             
             uniform float4 _ColorStart;
             uniform float4 _ColorEnd;
             uniform float4 _Dir;
 
             struct appdata {
                 float4 vertex : POSITION;
                 float3 normal : NORMAL;
                 fixed4 color : COLOR;
             };
 
             struct v2f {
                 float4 pos : SV_POSITION;
                 fixed4 color : COLOR;
             };
 
             v2f vert (appdata v) {
                 v2f o;
                 o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
                 float3 worldNormal = normalize( mul(float4(v.normal, 0.0), _Object2World).xyz);
                 
                 float3 b = worldNormal;
                 float3 a = normalize ( _Dir.xyz );
                 float d = 0.5+(a.x*b.x+a.y*b.y+a.z*b.z)/(a.x*a.x + a.y*a.y + a.z*a.z*2);
                 o.color = lerp(_ColorStart, _ColorEnd, d);
                 return o;
             }
         
             fixed4 frag (v2f i) : COLOR0 { return i.color; }
             ENDCG
         }
     }
     FallBack "Diffuse"
 }