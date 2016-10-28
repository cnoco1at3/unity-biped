Shader "Custom/MaskShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
    }

    Category
    {
		ZWrite On
	   	ZTest LEqual
	   	ColorMask 0
        Cull Back
        SubShader

        {
        	Tags {"Queue" = "Geometry+4"}
            Pass
            {
			Stencil {
			Comp Always
			Pass IncrSat
			}
                Lighting Off
                SetTexture [_MainTex]
                {
                    constantColor [_Color]
                    Combine texture * constant, texture * constant 
                } 
            }
        } 
    }
}