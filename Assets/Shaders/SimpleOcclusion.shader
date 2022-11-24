Shader "Unlit/SimpleOcclusion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent-1"}
        
        Blend Zero One
        
        
        LOD 100

        Pass
        {
            
        }
    }
}
