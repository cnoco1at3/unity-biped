Shader "Custom/MaskShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
    }

    Category
    {
		ZWrite Off
	   	ZTest LEqual
	   	ColorMask 0
        Cull Back
        SubShader

        {
            Pass
            {
			Stencil {
			Comp Always
			Pass IncrSat
			ZFail Zero
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