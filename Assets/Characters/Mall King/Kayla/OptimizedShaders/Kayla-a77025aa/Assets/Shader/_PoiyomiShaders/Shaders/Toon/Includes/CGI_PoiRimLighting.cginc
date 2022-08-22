#ifndef POI_RIM
    #define POI_RIM
    
    float4 _RimLightColor;
    float _RimLightingInvert;
    float _RimWidth;
    float _RimStrength;
    float _RimSharpness;
    float _RimLightColorBias;
    float _ShadowMix;
    float _ShadowMixThreshold;
    float _ShadowMixWidthMod;
    float _EnableRimLighting;
    float _RimBrighten;
    float _RimLightNormal;
    
    POI_TEXTURE_NOSAMPLER(_RimTex);
    POI_TEXTURE_NOSAMPLER(_RimMask);
    POI_TEXTURE_NOSAMPLER(_RimWidthNoiseTexture);

    float _RimWidthNoiseStrength;
    
    float4 rimColor = float4(0, 0, 0, 0);
    float rim = 0;
    
    void applyRimLighting(inout float4 albedo, inout float3 rimLightEmission)
    {
        float rimNoise = POI2D_SAMPLER_PAN(_RimWidthNoiseTexture, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        rimNoise = (rimNoise - .5) * float(0.329);

        float viewDotNormal = abs(dot(poiCam.viewDir, poiMesh.normals[float(0)]));
        
        if (float(0))
        {
            viewDotNormal = 1 - abs(dot(poiCam.viewDir, poiMesh.normals[float(0)]));
        }
        float rimWidth = float(0.503);
        rimWidth -= rimNoise;
        float rimMask = POI2D_SAMPLER_PAN(_RimMask, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        rimColor = POI2D_SAMPLER_PAN(_RimTex, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0)) * float4(0.9235639,1,0,1);
        rimWidth = max(lerp(rimWidth, rimWidth * lerp(0, 1, poiLight.lightMap - float(0.5)) * float(0.5), float(0.062)),0);
        rim = 1 - smoothstep(min(float(0), rimWidth), rimWidth, viewDotNormal);
        rim *= float4(0.9235639,1,0,1).a * rimColor.a * rimMask;
        rimLightEmission = rim * lerp(albedo, rimColor, float(0.485)) * float(20);
        albedo.rgb = lerp(albedo.rgb, lerp(albedo.rgb, rimColor, float(0.485)) + lerp(albedo.rgb, rimColor, float(0.485)) * float(0), rim);
    }
#endif
