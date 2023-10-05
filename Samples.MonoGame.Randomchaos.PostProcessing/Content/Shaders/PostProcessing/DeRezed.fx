#include "PPVertexShader.fxh"



Texture2D InputTexture;
sampler ScreenSampler
{
	Texture = <InputTexture>;
};

int  numberOfTiles = 8;

float4 DeRezPixelShader(VertexShaderOutput input) : COLOR0
{
	float2 uv = input.TexCoord;

	// Get Pixel size based on number of tiles..
	float size = 1.0 / numberOfTiles;


	float4 col = tex2D(ScreenSampler, (uv - fmod(uv, size.xx)) + (size / 2.0).xx);

	//col = tex2D(ScreenSampler, uv);

	return col;
}

technique DeRez
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL DeRezPixelShader();

		//ZWriteEnable = false;
	}
}