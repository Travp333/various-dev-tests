#ifndef SUBSURFACE
    #define SUBSURFACE
    
    float _SSSThicknessMod;
    float _SSSSCale;
    float _SSSPower;
    float _SSSDistortion;
    float4 _SSSColor;
    float _EnableSSS;
    
    POI_TEXTURE_NOSAMPLER(_SSSThicknessMap);

    float3 calculateSubsurfaceScattering()
    {
        float SSS = 1 - POI2D_SAMPLER_PAN(_SSSThicknessMap, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        
        half3 vLTLight = poiLight.direction + poiMesh.normals[0] * float(0);
        half flTDot = pow(saturate(dot(poiCam.viewDir, -vLTLight)), float(1)) * float(0.413);
        #ifdef FORWARD_BASE_PASS
            half3 fLT = (flTDot) * saturate(SSS + - 1 * float(0.761));
        #else
            half3 fLT = poiLight.attenuation * (flTDot) * saturate(SSS + - 1 * float(0.761));
        #endif
        
        return fLT * poiLight.color * float4(0,1,0.9331324,1);
    }
    
#endif
