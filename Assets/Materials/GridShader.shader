Shader "Custom/GridShader"
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
//		Blend SrcAlpha Zero
//        Cull Off
		Lighting Off
        SubShader
        {
        	Tags {"Queue" = "Geometry"}
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