// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3098,x:32719,y:32712,varname:node_3098,prsc:2|emission-5974-OUT;n:type:ShaderForge.SFN_Tex2d,id:5107,x:31942,y:32766,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_5107,prsc:2,tex:9192184a1121bec4abd85c91975d3829,ntxv:0,isnm:False|UVIN-5123-OUT;n:type:ShaderForge.SFN_Append,id:1178,x:31515,y:32757,varname:node_1178,prsc:2|A-8552-OUT,B-3626-OUT;n:type:ShaderForge.SFN_Slider,id:8552,x:31075,y:32742,ptovrint:False,ptlb:U_Offset,ptin:_U_Offset,varname:node_8552,prsc:2,min:-2,cur:0,max:2;n:type:ShaderForge.SFN_Slider,id:3626,x:31075,y:32836,ptovrint:False,ptlb:V_Offset,ptin:_V_Offset,varname:node_3626,prsc:2,min:-2,cur:1,max:2;n:type:ShaderForge.SFN_TexCoord,id:970,x:31534,y:32916,varname:node_970,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:5123,x:31780,y:32766,varname:node_5123,prsc:2|A-1178-OUT,B-970-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:4018,x:31942,y:32942,varname:node_4018,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1127,x:32116,y:32766,varname:node_1127,prsc:2|A-5107-RGB,B-4018-RGB;n:type:ShaderForge.SFN_Multiply,id:226,x:32333,y:32790,varname:node_226,prsc:2|A-1127-OUT,B-1854-OUT;n:type:ShaderForge.SFN_Slider,id:1854,x:32116,y:32969,ptovrint:False,ptlb:Multiply,ptin:_Multiply,varname:node_1854,prsc:2,min:0,cur:1,max:3;n:type:ShaderForge.SFN_Color,id:4564,x:32333,y:32608,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_4564,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5974,x:32528,y:32708,varname:node_5974,prsc:2|A-4564-RGB,B-226-OUT;proporder:5107-4564-1854-8552-3626;pass:END;sub:END;*/

Shader "My/UV_Offset_Add" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Multiply ("Multiply", Range(0, 3)) = 1
        _U_Offset ("U_Offset", Range(-2, 2)) = 0
        _V_Offset ("V_Offset", Range(-2, 2)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _U_Offset;
            uniform float _V_Offset;
            uniform float _Multiply;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float2 node_5123 = (float2(_U_Offset,_V_Offset)+i.uv0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5123, _MainTex));
                float3 emissive = (_Color.rgb*((_MainTex_var.rgb*i.vertexColor.rgb)*_Multiply));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
