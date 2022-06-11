#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    //#define VS_SHADERMODEL vs_4_0_level_9_1
    //#define PS_SHADERMODEL ps_4_0_level_9_1
    #define VS_SHADERMODEL vs_4_0
    #define PS_SHADERMODEL ps_4_0
#endif

// Global Variables
float4x4 world : WORLD;
float4x4 wvp : WorldViewProjection;
float4x4 vp : ViewProkjection;

float3 color = 1;
float2 UVMultiplier = float2(1, 1);
float viewDistance = 10;

float3 worldUp = half3(0, 1, 0);

int _StaticCylinderSpherical = 0; // 0 = static 1 = cylinder 2 = spherical


texture textureMat;
sampler textureSample = sampler_state
{
    texture = <textureMat>;
    AddressU = Wrap;
    AddressV = Wrap;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};



float3 EyePosition = float3(0, 0, 0);

float ambientPower = .05;

struct vIn
{
    float4 pos : SV_POSITION;
    float2 texCoords : TEXCOORD0;
    float3 normal : NORMAL0;
    float3 tangent : TANGENT0;
    float4 color : COLOR0;
};

struct vOut
{
    float4 pos : SV_POSITION;
    float2 texCoords : TEXCOORD0;
    float3 normal : NORMAL0;
    float3x3 tangent : TANGENT0;
    float4 color : COLOR0;
    
    float3 eyeVector : NORMAL1;
    float4 SPos : TexCoord1;
};

struct PixelShaderOutput
{
    float4 Color : COLOR0;
    float4 Depth : COLOR1;
};


vOut VertexShaderFunction(vIn input)
{
    vOut output = (vOut) 0;

    float3 center = mul(input.pos, world).xyz;
    float3 eyeVector = center - EyePosition;

    float3 finalPos = center;

    float3 scale = float3(world._11, world._22, world._33);
	
    float3 sideVector;
    float3 upVector;

    if (_StaticCylinderSpherical == 0)
    {
        sideVector = float3(1, 0, 0);
        upVector = worldUp;
    }
    else
    {
        if (_StaticCylinderSpherical == 1)
            eyeVector.y = 0;

        sideVector = normalize(cross(eyeVector, worldUp));
        upVector = normalize(cross(sideVector, eyeVector));
    }

    finalPos += (input.texCoords.x - 0.5) * sideVector * scale.x; // *input.Extras.y;
    finalPos += (0.5 - input.texCoords.y) * upVector * scale.y; // *(input.Extras.z);

    float4 finalPos4 = half4(finalPos, 1);

    output.pos = mul(finalPos4, vp);

    output.color = input.color;
    output.texCoords = input.texCoords;

    output.normal = mul(input.normal, (float3x3) world);
    output.tangent[0] = mul(input.tangent, (float3x3) world);
    output.tangent[1] = mul(cross(input.tangent, input.normal), (float3x3) world);
    output.tangent[2] = output.normal;
    
    output.SPos = output.pos;
    
    return output;
}

vOut VertexShaderFunctionInstanced(vIn input, float4x4 instanceTransform : POSITION1, float4 instanceColor : COLOR1)
{
    vOut output = (vOut) 0;
    
    float4x4 iWorld = transpose(instanceTransform);
    
    
    float3 center = float3(iWorld._41, iWorld._42, iWorld._43);
    float3 eyeVector = center - EyePosition;

    output.eyeVector = eyeVector;
    float3 finalPos = center;

    float3 scale = float3(length(iWorld._11_12_13), length(iWorld._21_22_23), length(iWorld._31_32_33)); //float3(iWorld._11, iWorld._22, iWorld._33);
    //scale = float3(iWorld._11, iWorld._22, iWorld._33);
    
    float3x3 rot = float3x3(float3(iWorld._11, iWorld._12, iWorld._13) / scale.x,
                            float3(iWorld._21, iWorld._22, iWorld._23) / scale.y,
                            float3(iWorld._31, iWorld._32, iWorld._33) / scale.z);
    
    float3 sideVector;
    float3 upVector;

    if (_StaticCylinderSpherical == 0)
    {
        sideVector = mul(float3(1, 0, 0), rot);
        upVector = mul(worldUp, rot);
    }
    else
    {
        if (_StaticCylinderSpherical == 1)
            eyeVector.y = 0;

        sideVector = normalize(cross(eyeVector, worldUp));
        upVector = normalize(cross(sideVector, eyeVector));
    }

    finalPos += (input.texCoords.x - 0.5) * sideVector * scale.x; // *input.Extras.y;
    finalPos += (0.5 - input.texCoords.y) * upVector * scale.y; // *(input.Extras.z);    
        
    float4 finalPos4 = half4(finalPos, 1);
        
    output.pos = mul(finalPos4, vp);    
    
    output.color = instanceColor;

    float d = viewDistance/distance(finalPos,EyePosition);

    output.color.a = lerp(0,output.color.a,saturate(d));
    output.texCoords = input.texCoords;

    output.normal = mul(input.normal, rot);
    output.tangent[0] = mul(input.tangent, rot);
    output.tangent[1] = mul(cross(input.tangent, input.normal), rot);
    output.tangent[2] = output.normal;

    output.SPos = output.pos;
    
    return output;
}

PixelShaderOutput PSBasicTexture(vOut input) : COLOR0
{
    PixelShaderOutput output = (PixelShaderOutput) 0;
    
    output.Color = tex2D(textureSample, input.texCoords * UVMultiplier);
    output.Color.rgb *= color * input.color.rgb;
    output.Color.a = output.Color.r * output.Color.g * output.Color.b;

    output.Color.a = input.color.a;

    //output.Color = input.color.a;
    
    float d = 1;//pow(abs(dot(input.normal, normalize(input.eyeVector))), 4);
    
    output.Color *= d;
    
    // Depth
    output.Depth.r = 0;//    -(input.SPos.z / input.SPos.w) * output.Color.a;
    output.Depth.a = 1;
   
    return output;
}

BlendState AlphaBlendingOn
{
    BlendEnable[0] = TRUE;
    DestBlend = INV_SRC_ALPHA;
    SrcBlend = SRC_ALPHA;
};


technique Deferred
{
    pass Pass1
    {
        //AlphaBlendEnable = TRUE;

        //Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
        //Blend One OneMinusSrcAlpha // Premultiplied transparency
        //Blend One One // Additive
        //Blend OneMinusDstColor One // Soft additive
        //Blend DstColor Zero // Multiplicative
        //Blend DstColor SrcColor // 2x multiplicative

        // Traditional transparency
        //SrcBlend = SRCALPHA;
        //DestBlend = INVSRCALPHA;

        // Premultiplied transparency
        //SrcBlend = ONE;
        //DestBlend = INVSRCALPHA;

        // Additive
        //SrcBlend = ONE;
        //DestBlend = ONE;

        // Soft additive
        //SrcBlend = INVDESTCOLOR;
        //DestBlend = ONE;

        // Multiplicative
        //SrcBlend = DESTCOLOR;
        //DestBlend = DESTCOLOR;

        // 2x multiplicative
        //SrcBlend = DESTCOLOR;
        //DestBlend = SRCCOLOR;

        //VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        VertexShader = compile VS_SHADERMODEL VertexShaderFunctionInstanced();
        PixelShader = compile PS_SHADERMODEL PSBasicTexture();
    }
}
