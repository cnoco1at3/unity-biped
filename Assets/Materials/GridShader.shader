Shader "Custom/TestShader"
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
	   	Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
		Lighting Off
        SubShader
        {
        	Tags {"Queue" = "Geometry-1"}
            Pass
            {
                SetTexture [_MainTex]
                {
                    constantColor [_Color]
                    Combine texture * constant, texture * constant 
                } 
            }
        } 
    }
}