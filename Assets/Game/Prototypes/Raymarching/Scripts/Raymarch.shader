Shader "Unlit/Raymarch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Divider ("Divider", Float) = 3
        _Torus_Width ("Torus Width", Range(0,1)) = .5
        _Torus_Hole_Width ("Torus Ring Width", Range(0,1)) = .5
        _Box_Hole_Width ("Box Hole Width", Range(0,1)) = .5
        _Sphere_Radius ("Sphere Radius", Range(0,1)) = .5
        _Surf_Dist ("Surfing Distance", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define MAX_STEPS 100
            #define MAX_DIST 100

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 ro : TEXCOORD1;
                float3 hitPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1)); //world space
                o.hitPos = v.vertex; //object space
                return o;
            }

            float _Torus_Hole_Width;
            float _Torus_Width;
            float _Sphere_Radius;

            float GetDist(float3 p)
            {
                float d = length(p) - _Sphere_Radius; //sphere
                float d2 = length(float2(length(p.xz) - _Torus_Width, p.y)) - _Torus_Hole_Width; //torus
                return min(d,d2);
            }

            float _Surf_Dist;
            
            float Raymarch(float3 ro, float3 rd)
            {
                float dO = 0;
                float dS;

                for (int i = 0; i < MAX_STEPS; i++)
                {
                    float3 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;

                    if (dS < _Surf_Dist / 1000 || dO > MAX_DIST)
                        break;
                }

                return dO;
            }

            float3 GetNormal(float3 p)
            {
                float2 e = float2(1e-2, 0);
                float3 n = GetDist(p) - float3(
                    GetDist(p - e.xyy),
                    GetDist(p - e.yxy),
                    GetDist(p - e.yyx)
                );

                return normalize(n);
            }

            float _Divider;
            float _Box_Hole_Width;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - .5;
                float3 ro = i.ro;
                float3 rd = normalize(i.hitPos - ro);
                float d = Raymarch(ro, rd);

                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = tex / 6;
                
                float m = dot(uv, uv);

                if (d < MAX_DIST)
                {
                    float3 p = ro + rd * d;
                    float3 n = GetNormal(p);
                    col.rgb = n;
                }

                // col = lerp(col, tex, smoothstep(.1, .2, m));
                if (m > _Box_Hole_Width / 2)
                    col = tex;
                
                return col;
            }
            ENDCG
        }
    }
}