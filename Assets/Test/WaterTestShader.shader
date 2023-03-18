// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WaterSurfaceShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
        _Smoothing("Smoothing", Range(0,4)) = 2
			

        _MeshHeight("Height of mesh", int) = 500  

        _WaveASpeed_Scale("WaveA Speed and Scale", Vector) = (1,0.4,2,2)
        _WaveAHeight("WaveA Height", float) = 1.2
        _WaveAIsAlternate("WaveA is Alternate? 1 or 0", Range(0,1)) = 1

        _WaveBSpeed_Scale("WaveB Speed and Scale", Vector) = (2,0.3,4,1)
        _WaveBHeight("WaveB Height", float) = 1.9
        _WaveBIsAlternate("WaveB is Alternate? 1 or 0", Range(0,1)) = 1

        _WaveCSpeed_Scale("WaveC Speed and Scale", Vector) = (7,1.4,0.3,0.9)
        _WaveCHeight("WaveC Height", float) = 1.5
        _WaveCIsAlternate("WaveC is Alternate? 1 or 0", Range(0,1)) = 1
	}
		SubShader
		{
            
			Tags { "RenderType" = "Opaque" }
			LOD 200
            

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:vert addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

            #include "noiseSimplex.cginc"

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
            float4 _WaveASpeed_Scale,_WaveBSpeed_Scale,_WaveCSpeed_Scale;
            float _WaveAHeight,_WaveBHeight ,_WaveCHeight;
            float _Smoothing;
            int _WaveAIsAlternate, _WaveBIsAlternate, _WaveCIsAlternate;
            int _MeshHeight;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				float OctaveWave(float4 speed_scale, float height, int isAlternate, float3 p) 
                {
                    if(isAlternate == 1)
                    {
                        float forPerlX = (p.x *speed_scale.z) / _MeshHeight;
                        float forPerlY = (p.z * speed_scale.w) /_MeshHeight;
                        float perl = snoise(float2(forPerlX, forPerlY)) * UNITY_PI * 2;

                        return cos(perl + length(float2(speed_scale.x, speed_scale.y)) * _Time.y) *  height;
                    }
                    else
                    {
                        float forPerlX = (p.x * speed_scale.z + _Time.y * speed_scale.x) / _MeshHeight;
                        float forPerlY = (p.z * speed_scale.w + _Time.y * speed_scale.y) / _MeshHeight;     
                        float perl = snoise(float2(forPerlX, forPerlY)) - 0.5;
                        return perl * height;                 
                    }
			}

			void vert(inout appdata_full vertexData)
			{
                float3 p = mul(unity_ObjectToWorld, vertexData.vertex).xyz;

                float3 p2 = p + float3(0.05,0,0);
                float3 p3 = p + float3(0,0,0.05);
				

				p.y += OctaveWave(_WaveASpeed_Scale, _WaveAHeight, _WaveAIsAlternate, p);
                p.y += OctaveWave(_WaveBSpeed_Scale, _WaveBHeight, _WaveBIsAlternate, p);
                p.y += OctaveWave(_WaveCSpeed_Scale, _WaveCHeight, _WaveCIsAlternate, p);

                p2.y += OctaveWave(_WaveASpeed_Scale, _WaveAHeight, _WaveAIsAlternate, p2);
                p2.y += OctaveWave(_WaveBSpeed_Scale, _WaveBHeight, _WaveBIsAlternate, p2);
                p2.y += OctaveWave(_WaveCSpeed_Scale, _WaveCHeight, _WaveCIsAlternate, p2);
                
                p3.y += OctaveWave(_WaveASpeed_Scale, _WaveAHeight, _WaveAIsAlternate, p3);
                p3.y += OctaveWave(_WaveBSpeed_Scale, _WaveBHeight, _WaveBIsAlternate, p3);
                p3.y += OctaveWave(_WaveCSpeed_Scale, _WaveCHeight, _WaveCIsAlternate, p3);

                p2.y -= (p2.y - p.y) * _Smoothing;
                p3.y -= (p3.y - p.y) * _Smoothing;

                float3 vna = cross(p3 - p, p2 - p);
                float3 vn = mul(unity_WorldToObject,vna);

                vertexData.normal = normalize(vn);

				vertexData.vertex.xyz =  mul(unity_WorldToObject,p);
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
