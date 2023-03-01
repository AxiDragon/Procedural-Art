Shader "Custom/Raymarch2"
{
    Properties
    {
        _Epsilon("Epsilon", Float) = 0.001
        _OtherTex("Main Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Epsilon;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _OtherTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float SphereSDF(float3 p, float3 c, float r) {
               return length(p - c) - r;
            }

            float DE(float3 p)
            {
                p = (p % 1.0) - float3(0.5,0.5,0.5);
                return length(p) - (0.25  + (sin(p.x * 11 + _Time.y) * .1)
                                          + ( cos(p.y * 13 + _Time.z) * .1)
                                          + ( sin(p.z * 7 + _Time.x) * .1) );
            }

            void RotateVector(float3 axis, float angle, inout float3 rotationDirection)
            {
                // Step 1: Normalize the rotation axis
                axis = normalize(axis);

                // Step 2: Compute the rotation matrix
                float c = cos(angle);
                float s = sin(angle);
                float t = 1 - c;
                float3x3 rotMatrix = float3x3(
                    t * axis.x * axis.x + c, t * axis.x * axis.y - s * axis.z, t * axis.x * axis.z + s * axis.y,
                    t * axis.x * axis.y + s * axis.z, t * axis.y * axis.y + c, t * axis.y * axis.z - s * axis.x,
                    t * axis.x * axis.z - s * axis.y, t * axis.y * axis.z + s * axis.x, t * axis.z * axis.z + c
                    );

                // Step 3: Apply the rotation
                rotationDirection = mul(rotationDirection, rotMatrix);
            }

            float map(float3 p) {
                //return SphereSDF(float3(0, 0, 0), p, 1);
                return DE(p);
            }

            float3 calcNormal(float3 p) {
                float dist = map(p);
                return normalize(float3(    map(p + float3(_Epsilon, 0, 0)) - dist,
                                            map(p + float3(0, _Epsilon, 0)) - dist,
                                            map(p + float3(0, 0, _Epsilon)) - dist
                                       )
                                );                                
            }

            float4 trimap(sampler2D tex, float3 p, float3 n, float k) {
                float3 m = pow(abs(n), float3(k, k, k));
                float4 x = tex2D(tex, p.yz);
                float4 y = tex2D(tex, p.xz);
                float4 z = tex2D(tex, p.xy);
                return (x * m.x + y * m.y + z * m.z) / (m.x + m.y + m.z);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = ((i.uv * 2.0 - 1.0 ) * (_ScreenParams.xy / 1000.0));

                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDirection = normalize(float3(uv, 1.0));
                float3 lightVec = normalize(float3(-1, -1, -1));

                RotateVector(float3(0, 1, 0), sin(_Time.z) * .1, rayDirection);
                RotateVector(float3(1, 0, 0), cos(_Time.z) * .1, rayDirection);

                float4 color = float4(rayDirection, 0);
                float totalDistance = 0.0;
                for (int j = 0; j < 64; j++)
                {
                    float3 p = rayOrigin + totalDistance * rayDirection;

                    float d = map(p);
                    if (d < _Epsilon) {
                        
                        float3 normal = calcNormal(p);
                        
                        float d3 = dot(lightVec, normal);
                        
                        float3 light = max(float3(0.025, 0.025, 0.025), float3(d3, d3, d3));
                        
                        float spec = pow(max(0.0, dot(reflect(rayDirection, normal), lightVec)), 64.0);
                        
                        
                        float3 pc = float3(p * .01);
                        float3 ccc = float3(sin(p.x/3.0), cos(p.y / 3.0), tan(p.z / 3.0));
                       
                        float4 texColor = trimap(_OtherTex, p, normal, 1.0);
                        /*
                        float3 rgb = (texColor.rgb * .5 + ccc * .5) * texColor.r * light + spec;

                        color = float4( lerp( rgb, rayDirection, totalDistance * .1), 1);
                        */
                        float3 diffuse = texColor;
                        color = float4((light* diffuse) + spec * float3(.8, .8, .8), 1);
                        break;
                    }

                    totalDistance += d;
                }

                return color;
            }
            ENDCG
        }
    }
}
