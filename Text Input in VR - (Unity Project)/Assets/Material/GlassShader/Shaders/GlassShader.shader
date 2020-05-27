// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4662,x:32719,y:32712,varname:node_4662,prsc:2|diff-1457-RGB,spec-1961-OUT,gloss-9130-OUT,normal-4010-OUT,alpha-1457-A,refract-1748-OUT;n:type:ShaderForge.SFN_Slider,id:1961,x:32661,y:32564,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_1961,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.75,max:1;n:type:ShaderForge.SFN_Slider,id:4389,x:32090,y:32429,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_4389,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.95,max:1;n:type:ShaderForge.SFN_Fresnel,id:462,x:31994,y:32505,varname:node_462,prsc:2|EXP-6071-OUT;n:type:ShaderForge.SFN_Slider,id:6071,x:31661,y:32532,ptovrint:False,ptlb:FresnelGlossExponent,ptin:_FresnelGlossExponent,varname:node_6071,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.5,cur:1.5,max:5;n:type:ShaderForge.SFN_Slider,id:4039,x:31837,y:32646,ptovrint:False,ptlb:FresnelGlossImpact,ptin:_FresnelGlossImpact,varname:node_4039,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7,max:1;n:type:ShaderForge.SFN_Clamp01,id:9130,x:32488,y:32537,varname:node_9130,prsc:2|IN-3858-OUT;n:type:ShaderForge.SFN_Subtract,id:3858,x:32326,y:32537,varname:node_3858,prsc:2|A-4389-OUT,B-1956-OUT;n:type:ShaderForge.SFN_Multiply,id:1956,x:32169,y:32557,varname:node_1956,prsc:2|A-462-OUT,B-4039-OUT;n:type:ShaderForge.SFN_Color,id:1457,x:32214,y:32712,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1457,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:0.4;n:type:ShaderForge.SFN_Multiply,id:1748,x:32488,y:33375,varname:node_1748,prsc:2|A-3734-OUT,B-74-OUT;n:type:ShaderForge.SFN_Multiply,id:3734,x:32293,y:33308,varname:node_3734,prsc:2|A-6649-OUT,B-3357-OUT;n:type:ShaderForge.SFN_Slider,id:3357,x:31943,y:33383,ptovrint:False,ptlb:Refraction,ptin:_Refraction,varname:node_3357,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.08,max:1;n:type:ShaderForge.SFN_ComponentMask,id:6649,x:32000,y:33218,varname:node_6649,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-1867-XYZ;n:type:ShaderForge.SFN_Transform,id:1867,x:31841,y:33218,varname:node_1867,prsc:2,tffrom:0,tfto:3|IN-2821-OUT;n:type:ShaderForge.SFN_NormalVector,id:2821,x:31682,y:33218,prsc:2,pt:False;n:type:ShaderForge.SFN_Power,id:74,x:32293,y:33436,varname:node_74,prsc:2|VAL-6868-OUT,EXP-4242-OUT;n:type:ShaderForge.SFN_Vector1,id:4242,x:32278,y:33568,varname:node_4242,prsc:2,v1:2;n:type:ShaderForge.SFN_Clamp01,id:6868,x:32100,y:33464,varname:node_6868,prsc:2|IN-1306-OUT;n:type:ShaderForge.SFN_OneMinus,id:1306,x:31943,y:33464,varname:node_1306,prsc:2|IN-6600-OUT;n:type:ShaderForge.SFN_Clamp01,id:6600,x:31785,y:33464,varname:node_6600,prsc:2|IN-8160-OUT;n:type:ShaderForge.SFN_Dot,id:8160,x:31623,y:33464,varname:node_8160,prsc:2,dt:0|A-279-OUT,B-5036-OUT;n:type:ShaderForge.SFN_NormalVector,id:279,x:31446,y:33379,prsc:2,pt:False;n:type:ShaderForge.SFN_Normalize,id:5036,x:31446,y:33525,varname:node_5036,prsc:2|IN-5643-OUT;n:type:ShaderForge.SFN_Subtract,id:5643,x:31284,y:33525,varname:node_5643,prsc:2|A-5340-XYZ,B-2873-XYZ;n:type:ShaderForge.SFN_ViewPosition,id:5340,x:31097,y:33406,varname:node_5340,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:2873,x:31097,y:33525,varname:node_2873,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:9469,x:31565,y:32962,ptovrint:False,ptlb:Bump,ptin:_Bump,varname:_Bump_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:7662,x:31408,y:32859,ptovrint:False,ptlb:Bump Power,ptin:_BumpPower,varname:node_658,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:2300,x:31755,y:32888,varname:node_2300,prsc:2|A-7662-OUT,B-9469-RGB;n:type:ShaderForge.SFN_Vector1,id:7308,x:31867,y:33042,varname:node_7308,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Vector3,id:4256,x:31867,y:32818,varname:node_4256,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:4010,x:32037,y:32930,varname:node_4010,prsc:2|A-4256-OUT,B-2300-OUT,T-7308-OUT;proporder:1457-9469-7662-3357-1961-4389-6071-4039;pass:END;sub:END;*/

Shader "Glass/Glass" {
    Properties {
        _Color ("Color", Color) = (1,1,1,0.4)
        _Bump ("Bump", 2D) = "bump" {}
        _BumpPower ("Bump Power", Range(0, 1)) = 1
        _Refraction ("Refraction", Range(0, 1)) = 0.08
        _Metallic ("Metallic", Range(0, 1)) = 0.75
        _Gloss ("Gloss", Range(0, 1)) = 0.95
        _FresnelGlossExponent ("FresnelGlossExponent", Range(0.5, 5)) = 1.5
        _FresnelGlossImpact ("FresnelGlossImpact", Range(0, 1)) = 0.7
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        //LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _FresnelGlossExponent;
            uniform float _FresnelGlossImpact;
            uniform float4 _Color;
            uniform float _Refraction;
            uniform sampler2D _Bump; uniform float4 _Bump_ST;
            uniform float _BumpPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Bump_var = UnpackNormal(tex2D(_Bump,TRANSFORM_TEX(i.uv0, _Bump)));
                float3 normalLocal = lerp(float3(0,0,1),(_BumpPower*_Bump_var.rgb),0.7);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + ((mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg*_Refraction)*pow(saturate((1.0 - saturate(dot(i.normalDir,normalize((_WorldSpaceCameraPos-i.posWorld.rgb)))))),2.0));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = saturate((_Gloss-(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelGlossExponent)*_FresnelGlossImpact)));
                float perceptualRoughness = 1.0 - saturate((_Gloss-(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelGlossExponent)*_FresnelGlossImpact)));
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = _Color.rgb; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Color.a),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _FresnelGlossExponent;
            uniform float _FresnelGlossImpact;
            uniform float4 _Color;
            uniform float _Refraction;
            uniform sampler2D _Bump; uniform float4 _Bump_ST;
            uniform float _BumpPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Bump_var = UnpackNormal(tex2D(_Bump,TRANSFORM_TEX(i.uv0, _Bump)));
                float3 normalLocal = lerp(float3(0,0,1),(_BumpPower*_Bump_var.rgb),0.7);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + ((mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg*_Refraction)*pow(saturate((1.0 - saturate(dot(i.normalDir,normalize((_WorldSpaceCameraPos-i.posWorld.rgb)))))),2.0));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation, i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = saturate((_Gloss-(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelGlossExponent)*_FresnelGlossImpact)));
                float perceptualRoughness = 1.0 - saturate((_Gloss-(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelGlossExponent)*_FresnelGlossImpact)));
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = _Color.rgb; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Color.a,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}