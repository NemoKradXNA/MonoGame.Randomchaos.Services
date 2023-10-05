#include "PPVertexShader.fxh"

uniform extern texture SceneTex;

sampler ScreenSampler = sampler_state
{
	Texture = <SceneTex>;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;

};

uniform extern texture NoiseTex;

sampler NoiseSampler = sampler_state
{
    Texture = <NoiseTex>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};

float CURVATURE = 8.0;//3.9
float BLUR = .05;//0.021;

float density = 0.95;
float opacityScanline = .025;
float opacityNoise = .125;
float flickering = 0.05;

float3 shift = float3(1.0, -0.0, -1.0);

float iTime;

float2 iResolution;

float random(float2 st) {
	return frac(sin(dot(st.xy,
		float2(12.9898, 78.233))) *
		43758.5453123);
}

float blend(const in float x, const in float y) {
	return (x < 0.5) ? (2.0 * x * y) : (1.0 - 2.0 * (1.0 - x) * (1.0 - y));
}

float3 blend(const in float3 x, const in float3 y, const in float opacity) {
	float3 z = float3(blend(x.r, y.r), blend(x.g, y.g), blend(x.b, y.b));
	return z * opacity + x * (1.0 - opacity);
}

float4 ChromaticAberrationShader(VertexShaderOutput input) : COLOR0
{
	float2 uv = input.TexCoord;

    // chromatic aberration
    float amount = 0.0;

    amount = (1.0 + sin(iTime * 6.0)) * 0.5;
    amount *= 1.0 + sin(iTime * 16.0) * 0.5;
    amount *= 1.0 + sin(iTime * 19.0) * 0.5;
    amount *= 1.0 + sin(iTime * 27.0) * 0.5;
    amount = pow(amount, 2.0);

    //amount *= 0.05;

    float os = tex2D(NoiseSampler, (uv * 3.0) - float2(0, -iTime) * amount).r;

    //amount *= 0.0005;
    //amount = 0.0125;
    amount = os * .025;

    float3 col;
    col.r = tex2D(ScreenSampler, float2(uv.x + (amount * shift.x), uv.y)).r;
    col.g = tex2D(ScreenSampler, float2(uv.x + (amount * shift.y), uv.y)).g;
    col.b = tex2D(ScreenSampler, float2(uv.x + (amount * shift.z), uv.y)).b;

    col *= (1.0 - amount * 0.5);

    //col =texture( iChannel0, uv ).rgb;

    // scan lines
    float count = iResolution.y * density;
    float2 sl = float2(sin(uv.y * count), cos(uv.y * count));
    float3 scanlines = float3(sl.x, sl.y, sl.x);

    col += col * scanlines * opacityScanline;
    col += col * random(uv * iTime) * opacityNoise;
    col += col * sin(110.0 * iTime) * flickering;

    //curving
    if (CURVATURE >= 8)
    {
        float2 crtUV = uv * 2. - 1.;
        float2 offset = crtUV.yx / CURVATURE;
        crtUV += crtUV * offset * offset;
        crtUV = crtUV * .5 + .5;

        float2 edge = smoothstep(0., BLUR, crtUV) * (1. - smoothstep(1. - BLUR, 1., crtUV));

        col *= edge.x * edge.y;
    }

	return float4(col,1);
}

technique ChromaticAberration
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL ChromaticAberrationShader();

		//ZWriteEnable = false;
	}
}