Shader "Retro/Retro Shader" {
	Properties
	{
		_WireframeVal("Line Width", Range(0., 0.34)) = 0.05
		[Toggle] _IsRectangular("Rectangular", Float) = 1
		_FrontColor("Line Color", color) = (1., 1., 1., 1.)
		_BackColor("Inner Color", color) = (1., 1., 1., 1.)
		[Toggle] _IsGlossy("Glossy", Float) = 0
		_GlossVal("Glossiness", Range(0., 1.)) = 0.05
	}
		
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		// Reflection Phase
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float3 worldPos : TEXCOORD0;
				half3 worldNormal : TEXCOORD1;
				float4 pos : SV_POSITION;
			};

			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}

			fixed3 _BackColor;
			float _GlossVal;
			float _IsGlossy;

			fixed4 frag(v2f i) : SV_Target
			{
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				half3 worldRefl = reflect(-worldViewDir, i.worldNormal);
				if (_IsGlossy == 0.f)
					worldRefl *= 0;
			
				half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
				half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);
				skyColor.rgb *= _GlossVal;
				fixed4 c = 0;
				if (_IsGlossy != 0.f) {
					c.rgb = skyColor;
					c.rgb += _BackColor;
				}
				else {
					c.rgb = _BackColor;
				}
				return c;
			}
			ENDCG
		}
		// Drawing the lines - Retrowave Effect
		Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#include "UnityCG.cginc"

			struct v2g {
				float4 pos : SV_POSITION;
			};

			struct g2f {
				float4 pos : SV_POSITION;
				float3 bary : TEXCOORD0;
			};

			uniform float4 _RimColor;
			v2g vert(appdata_base v) {
				v2g o;
				o.pos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			float _IsRectangular;
			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
				float3 param = float3(0., 0., 0.);

				float EdgeA = length(IN[0].pos - IN[1].pos);
				float EdgeB = length(IN[1].pos - IN[2].pos);
				float EdgeC = length(IN[2].pos - IN[0].pos);

				float ParamBase = 1.;
				if (_IsRectangular == 0)
					ParamBase = 0;

				if (EdgeA > EdgeB && EdgeA > EdgeC)
					param.y = ParamBase;
				else if (EdgeB > EdgeC && EdgeB > EdgeA)
					param.x = ParamBase;
				else
					param.z = ParamBase;

				g2f o;
				o.pos = mul(UNITY_MATRIX_VP, IN[0].pos);
				o.bary = float3(1., 0., 0.) + param;
				triStream.Append(o);
				o.pos = mul(UNITY_MATRIX_VP, IN[1].pos);
				o.bary = float3(0., 0., 1.) + param;
				triStream.Append(o);
				o.pos = mul(UNITY_MATRIX_VP, IN[2].pos);
				o.bary = float3(0., 1., 0.) + param;
				triStream.Append(o);
			}

			float _WireframeVal;
			fixed4 _FrontColor;
			
			fixed4 frag(g2f i) : SV_Target {
			if (!any(bool3(i.bary.x < _WireframeVal, i.bary.y < _WireframeVal, i.bary.z < _WireframeVal)))
					discard;

				return _FrontColor;
			}

			ENDCG
		}

	}
}
