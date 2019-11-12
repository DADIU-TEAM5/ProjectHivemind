// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Bug/Standard"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Color("Color", Color) = (1,1,1,0)
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_MetallicValue("Metallic Value", Range( 0 , 1)) = 1
		_SmoothnessValue("Smoothness Value", Range( 0 , 1)) = 1
		[Normal]Normal("Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
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
		uniform float4 _Color;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _MetallicValue;
		uniform float _SmoothnessValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uvNormal = i.uv_texcoord * Normal_ST.xy + Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( Normal, uvNormal ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode20 = tex2D( _Albedo, uv_Albedo );
			float4 appendResult21 = (float4(tex2DNode20.r , tex2DNode20.g , tex2DNode20.b , 0.0));
			o.Albedo = ( _Color * appendResult21 ).rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode18 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			float4 appendResult8 = (float4(tex2DNode18.r , tex2DNode18.g , tex2DNode18.b , 0.0));
			o.Metallic = ( appendResult8 * _MetallicValue ).x;
			o.Smoothness = ( tex2DNode18.a * _SmoothnessValue );
			o.Alpha = 1;
			clip( tex2DNode20.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1045;73;534;928;1030.767;513.175;1.837954;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1581.1,319.6421;Float;True;Property;_MetallicSmoothness;MetallicSmoothness;3;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1575.402,-310.7451;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;18;-1092.732,331.4921;Float;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-1101.348,-311.6705;Float;True;Property;_TextureSample2;Texture Sample 2;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-724.3479,690.6069;Float;False;Property;_SmoothnessValue;Smoothness Value;5;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-719.8355,448.7253;Float;False;Property;_MetallicValue;Metallic Value;4;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-682.9651,290.7477;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;9;-671.101,-533.1891;Float;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-711.1274,-337.3753;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-426.8828,435.7398;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-391.8443,618.8832;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-1088.82,7.433971;Float;True;Property;Normal;Normal;6;1;[Normal];Create;False;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-381.1911,-419.0543;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Bug/Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;3;0
WireConnection;20;0;5;0
WireConnection;8;0;18;1
WireConnection;8;1;18;2
WireConnection;8;2;18;3
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;21;2;20;3
WireConnection;15;0;8;0
WireConnection;15;1;6;0
WireConnection;17;0;18;4
WireConnection;17;1;16;0
WireConnection;12;0;9;0
WireConnection;12;1;21;0
WireConnection;0;0;12;0
WireConnection;0;1;19;0
WireConnection;0;3;15;0
WireConnection;0;4;17;0
WireConnection;0;10;20;4
ASEEND*/
//CHKSM=E2E8D1B367DA51648D1726B731238233AD4A115C