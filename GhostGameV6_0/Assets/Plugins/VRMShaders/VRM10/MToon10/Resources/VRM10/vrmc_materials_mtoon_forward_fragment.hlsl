// vrmc_materials_mtoon_forward_fragment.hlsl
// Patched fragment include — defines input struct and the MToonFragment entry.

///////////////////////////////////////////////////////////////////////////////
// Simple input struct used by the fragment shader.
// This must match what your vertex stage outputs. If your vertex file uses a
// different name or different field names, let me know and I'll adapt it.
///////////////////////////////////////////////////////////////////////////////
struct MToonFragmentInput
{
    float2 uv : TEXCOORD0;
    float3 normalWS : TEXCOORD1; // world-space normal
    float3 viewDirectionWS : TEXCOORD2; // world-space view dir (cameraPos - pos)
    float3 mainLightDir : TEXCOORD3; // primary light direction (world space)
    float3 mainLightColor : COLOR; // primary light color
    float shadingShift : TEXCOORD4; // shading shift factor (if used)
    float fogCoord : TEXCOORD5; // fog coordinate, optional
    float4 color : COLOR0; // base color (if provided)
};

///////////////////////////////////////////////////////////////////////////////
// Minimal helper: safe normal unpack (if normal map used)
///////////////////////////////////////////////////////////////////////////////
#ifndef UNITY_SAMPLE_TEX2D_UNCLAMPED
#define UNITY_SAMPLE_TEX2D_UNCLAMPED(tex, uv) tex2D(tex, uv)
#endif

float3 UnpackNormalSafe(float4 n)
{
    // Assumes normal stored in DXT5nm or standard normal map; try standard unpack
    float3 nor = n.xyz * 2.0 - 1.0;
    return normalize(nor);
}

///////////////////////////////////////////////////////////////////////////////
// Main fragment entry
///////////////////////////////////////////////////////////////////////////////
float4 MToonFragment(MToonFragmentInput IN) : SV_Target
{
    // Create local aliases so older code using 'i' / 'IN' / 'input' works.
   // const MToonFragmentInput i = IN;
    const MToonFragmentInput input = IN;

    // Sample base color texture (if present)
    float4 baseColor = float4(1.0, 1.0, 1.0, 1.0);
#ifdef _MainTex
    baseColor = UNITY_SAMPLE_TEX2D_UNCLAMPED(_MainTex, IN.uv) * _Color;
#endif

    // Sample shade/multiply texture
    float4 shadeTex = float4(1, 1, 1, 1);
#ifdef _ShadeTex
    shadeTex = UNITY_SAMPLE_TEX2D_UNCLAMPED(_ShadeTex, IN.uv) * _ShadeColor;
#endif

    // Resolve normal (use normal map if enabled)
    float3 normalWS = IN.normalWS;
#ifdef _NORMALMAP
    // if _BumpMap exists, try to use it — vertex tangent space required for true normal mapping.
    float4 bump = UNITY_SAMPLE_TEX2D_UNCLAMPED(_BumpMap, IN.uv);
    normalWS = UnpackNormalSafe(bump); // fallback; ideally vertex provides TBN and tangent-space normal
#endif

    // Basic directional lighting (Lambert)
    float3 lightDir = normalize(IN.mainLightDir);
    float NdotL = saturate(dot(normalWS, lightDir));

    // Simple toony shading: mix base and shade using a threshold curve
    float shadeFactor = saturate(IN.shadingShift + _ShadingShiftFactor); // user tweak
    float3 lit = lerp(shadeTex.rgb, baseColor.rgb, shadeFactor) * NdotL * IN.mainLightColor;

    // Rim lighting (optional)
#ifdef _MTOON_RIMMAP
    float rim = pow(1.0 - saturate(dot(normalWS, -IN.viewDirectionWS)), _RimFresnelPower);
    float3 rimTex = UNITY_SAMPLE_TEX2D_UNCLAMPED(_RimTex, IN.uv).rgb * _RimColor.rgb;
    lit += rim * rimTex * _RimLightingMix;
#endif

    // Emission
    float3 emission = _EmissionColor.rgb;
#ifdef _MTOON_EMISSIVEMAP
    emission *= UNITY_SAMPLE_TEX2D_UNCLAMPED(_EmissionMap, IN.uv).rgb;
#endif

    // Final color
    float3 finalColor = lit + emission;

    // Apply fog if available (Unity fog macros require proper fogCoord; this is conservative)
#ifdef UNITY_APPLY_FOG
    UNITY_APPLY_FOG(IN.fogCoord, float4(finalColor, baseColor.a));
#endif

    return float4(finalColor, baseColor.a);
}
