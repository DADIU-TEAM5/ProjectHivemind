// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Bug/MonsterShader"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Color1("Color 1", Color) = (1,1,1,0)
		_Color2("Color 2", Color) = (1,0,0,0)
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_MetallicValue("Metallic Value", Range( 0 , 1)) = 1
		_SmoothnessValue("Smoothness Value", Range( 0 , 1)) = 1
		[Normal]Normal("Normal", 2D) = "bump" {}
		_Emissive("Emissive", 2D) = "white" {}
		_EmissiveColor("Emissive Color", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D Normal;
		uniform float4 Normal_ST;
		uniform float4 _Color2;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _Color1;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _EmissiveColor;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _MetallicValue;
		uniform float _SmoothnessValue;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uvNormal = i.uv_texcoord * Normal_ST.xy + Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( Normal, uvNormal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode20 = tex2D( _Albedo, uv_Albedo );
			float4 appendResult21 = (float4(tex2DNode20.r , tex2DNode20.g , tex2DNode20.b , 0.0));
			float4 lerpResult35 = lerp( ( _Color2 * appendResult21 ) , ( _Color1 * appendResult21 ) , tex2DNode20.a);
			o.Albedo = lerpResult35.rgb;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			o.Emission = ( tex2D( _Emissive, uv_Emissive ) * _EmissiveColor ).rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode18 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			float4 appendResult8 = (float4(tex2DNode18.r , tex2DNode18.g , tex2DNode18.b , 0.0));
			o.Metallic = ( appendResult8 * _MetallicValue ).x;
			o.Smoothness = ( tex2DNode18.a * _SmoothnessValue );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
864;73;714;928;1371.965;163.4957;2.290939;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1876.522,-702.1946;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;07ae1d1f6da8b6e4fb04da51e0ff8ac2;07ae1d1f6da8b6e4fb04da51e0ff8ac2;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1581.1,319.6421;Float;True;Property;_MetallicSmoothness;MetallicSmoothness;3;0;Create;True;0;0;False;0;None;bf1d4f74a2592ae4aa1ceecbedab2ca5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;20;-1547.802,-713.5223;Float;True;Property;_TextureSample2;Texture Sample 2;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-958.0955,-453.4587;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;9;-989.0861,-693.2009;Float;False;Property;_Color1;Color 1;1;0;Create;True;0;0;False;0;1,1,1,0;0.8113208,0.7745336,0.7079922,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1092.732,331.4921;Float;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-973.1989,-233.9242;Float;False;Property;_Color2;Color 2;2;0;Create;True;0;0;False;0;1,0,0,0;0.509434,0.3003738,0.3003738,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;36;-1302.465,890.8486;Float;True;Property;_Emissive;Emissive;7;0;Create;True;0;0;False;0;07ae1d1f6da8b6e4fb04da51e0ff8ac2;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;37;-967.4964,892.9136;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-705.4819,332.3019;Float;False;Property;_MetallicValue;Metallic Value;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-733.9169,526.3381;Float;False;Property;_SmoothnessValue;Smoothness Value;5;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-668.6115,174.3242;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-712.858,-356.3112;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;34;-991.6533,-1008.18;Float;True;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-725.2869,-609.2534;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;39;-908.5377,695.7303;Float;False;Property;_EmissiveColor;Emissive Color;8;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-417.3617,446.6403;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-412.5292,319.3164;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;19;-1088.82,7.433971;Float;True;Property;Normal;Normal;6;1;[Normal];Create;False;0;0;False;0;None;ddb40858866e49141a0409a02cba9432;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;35;-289.305,-514.4631;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-553.0166,770.9414;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Bug/MonsterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;5;0
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;21;2;20;3
WireConnection;18;0;3;0
WireConnection;37;0;36;0
WireConnection;8;0;18;1
WireConnection;8;1;18;2
WireConnection;8;2;18;3
WireConnection;24;0;23;0
WireConnection;24;1;21;0
WireConnection;34;0;20;4
WireConnection;12;0;9;0
WireConnection;12;1;21;0
WireConnection;17;0;18;4
WireConnection;17;1;16;0
WireConnection;15;0;8;0
WireConnection;15;1;6;0
WireConnection;35;0;24;0
WireConnection;35;1;12;0
WireConnection;35;2;34;0
WireConnection;38;0;37;0
WireConnection;38;1;39;0
WireConnection;0;0;35;0
WireConnection;0;1;19;0
WireConnection;0;2;38;0
WireConnection;0;3;15;0
WireConnection;0;4;17;0
ASEEND*/
//CHKSM=97D1C195B22E2CE7F8E9A2A9206BD2A7B2479FCD