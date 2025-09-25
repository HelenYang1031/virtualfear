Shader "Unlit/evaporate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma pos vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct a2v
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2g vert (a2v v)
            {
                v2f o;
                o.pos = i.pos;
                o.uv = i.uv;
                //o.pos = UnityObjectToClipPos(v.pos);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            [maxvertexcount(1)]
            void geom(triangle v2g IN[3], inout PointStream<g2f> pointstream)
            {
                g2f o;
                float3 tempPos = (IN[0].pos + IN[1].pos + IN[2].pos)/3;
                o.pos = UnityObjectToClipPos(tempPos);
                o.uv = IN[0].uv + IN[1].uv + IN[2].uv;
                pointstream.Append(o);

            }

            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
