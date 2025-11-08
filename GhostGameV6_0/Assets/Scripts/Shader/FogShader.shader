Shader "Custom/SelectiveFog"
{
    Properties
    {
        [Header(Fog Settings)]
        _FogColor ("Fog Color", Color) = (0.5,0.5,0.5,1)
        _FogStartDistance ("Fog Start Distance", Float) = 10
        _FogEndDistance ("Fog End Distance", Float) = 100
        _FogDensity ("Fog Density", Range(0.1,3)) = 1.0
        
        [Header(Advanced)]
        _FogHeight ("Fog Height", Float) = 0
        _FogHeightFalloff ("Height Falloff", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True"
        }
        LOD 100

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back
            ColorMask RGB

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _FogColor;
                float _FogStartDistance;
                float _FogEndDistance;
                half _FogDensity;
                float _FogHeight;
                half _FogHeightFalloff;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionWS = vertexInput.positionWS;
                output.positionCS = vertexInput.positionCS;
                
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // World-Space-Position des Pixels
                float3 worldPos = input.positionWS;
                float3 cameraPos = GetCameraPositionWS();

                // Distanz zur Kamera berechnen
                float cameraDistance = distance(cameraPos, worldPos);
                
                // Basis Fog-Faktor berechnen
                float distanceRange = max(0.001, _FogEndDistance - _FogStartDistance);
                half fogFactor = saturate((cameraDistance - _FogStartDistance) / distanceRange);
                
                // Exponentieller Fog
                fogFactor = 1.0 - exp(-fogFactor * _FogDensity);
                
                // Höhen-basierter Fog
                half heightDiff = worldPos.y - _FogHeight;
                half heightFactor = saturate(heightDiff * _FogHeightFalloff);
                fogFactor *= (1.0 - heightFactor);
                
                // Schutz vor negativen Werten
                fogFactor = max(0.0, fogFactor);

                half4 finalColor = _FogColor;
                finalColor.a *= fogFactor;

                return finalColor;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/Unlit"
}