Shader "Custom/Floor"
{
    Properties
    {
        _MinBrightness("Min Brightness", Float) = 0.3
        _TileSize("Tile Size", Float) = 1.0
        _FadeDistanceBegin("Fade Distance Begin", Float) = 10.0
        _FadeDistanceEnd("Fade Distance Ebd", Float) = 20.0
        _LightColour ("Light Colour", Color) = (1,1,1,1)
        _DarkColour("Dark Colour", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Basic

        #include "math.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 viewDir; 
            float3 vertexNormal;
        };

        fixed3 _LightColour;
        fixed3 _DarkColour;
        fixed _TileSize;
        fixed _FadeDistanceBegin;
        fixed _FadeDistanceEnd;
        fixed _MinBrightness;

        void vert(inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.vertexNormal = abs(v.normal);
        }

        half4 LightingBasic(SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot(s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * clamp(_MinBrightness, 10.0, (0.7 + 0.3 * NdotL));// *(0.5 + 0.5 * step(0.5, atten))));
            c.a = s.Alpha;
            return c;
        }

        float mod(float x, float y) {
            return x % y;
        }

        /*float checker(float3 pos)
        {
            float fmodResult = mod(floor(pos.x) + floor(pos.y) + floor(pos.z), 2.0);
            return max(sign(fmodResult), 0.0);
        }*/

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color

            float3 worldScale = float3(
                length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
                length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
                length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
                );


            float3 pos = mul(unity_WorldToObject, IN.worldPos / _TileSize) * worldScale;
            fixed3 c = lerp(_DarkColour, _LightColour, mod((abs(floor(pos.x)) + abs(floor(pos.y)) + abs(floor(pos.z))), 2.0f));
            
            fixed NdotL = clamp(0, 1, 5.0 * dot(normalize(IN.viewDir), o.Normal));
            fixed dist_mod = lerp_inv_01(_FadeDistanceEnd, _FadeDistanceBegin, length(IN.worldPos - _WorldSpaceCameraPos));
            //c = lerp(_LightColour, c, clamp(0, 1, 5.0 * dot(normalize(IN.viewDir), o.Normal)));//lerp_inv_01(_FadeDistanceBegin, _FadeDistanceEnd, length(IN.worldPos - _WorldSpaceCameraPos)));
            c = lerp(_LightColour, c, max(dist_mod, NdotL * 0.8));

            o.Albedo = c;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
