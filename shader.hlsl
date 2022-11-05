#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
int Data[9] = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 coords = input.TextureCoordinates;
	float4 color = tex2D(SpriteTextureSampler, coords) * input.Color;
	float texCoordsX = coords.x * 3;
	float texCoordsY = coords.y * 3;
	int indexX = (int)texCoordsX;
	int indexY = (int)texCoordsY;
	int x = Data[indexY * 3 + indexX];
	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};