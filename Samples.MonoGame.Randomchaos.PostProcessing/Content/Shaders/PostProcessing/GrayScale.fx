#include "PPVertexShader.fxh"

Texture2D InputTexture;
sampler ScreenSampler
{
	Texture = <InputTexture>;
};


float4 GreyScalePixelShader(VertexShaderOutput input) : COLOR0
{
	float2 uv = input.TexCoord;


	float4 col = tex2D(ScreenSampler, uv);

	col.rgb = (col.r + col.g + col.b) / 3.0;

	return col;
}

technique DeRez
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL GreyScalePixelShader();
	}
}