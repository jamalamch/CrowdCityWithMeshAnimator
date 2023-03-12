/*
Created by jiadong chen
https://jiadong-chen.medium.com/
*/

Shader "chenjd/BuiltIn/AnimTwoMapShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap1", 2D) ="white" {}
		_AnimLen("Anim1 Length", Float) = 0

        _AnimMap2 ("AnimMap2", 2D) ="white" {}
		_AnimLen2("Anim2 Length", Float) = 0

		_AnimOffcet("Anim offset",Float) = 0
		_AnimLerp("Anim lerp",  Range(0,1)) = 0
	}
	
	CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float2 uv : TEXCOORD0;
        float4 pos : POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f
    {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2fShadow {
        V2F_SHADOW_CASTER;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    sampler2D _MainTex;
    float4 _MainTex_ST;

    sampler2D _AnimMap,_AnimMap2;
    float4 _AnimMap_TexelSize;//x == 1/width
    float4 _AnimMap2_TexelSize;//x == 1/width

    float _AnimLen,_AnimLen2;
    float _AnimOffcet;
    float _AnimLerp;

    
    v2f vert (appdata v, uint vid : SV_VertexID)
    {
        UNITY_SETUP_INSTANCE_ID(v);

        float animMap_y = (_Time.y + _AnimOffcet) / _AnimLen;

        fmod(animMap_y, 1.0);

        float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;

        float4 pos1 = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));


        animMap_y = (_Time.y + _AnimOffcet) / _AnimLen2;
        fmod(animMap_y, 1.0);
        animMap_x = (vid + 0.5) * _AnimMap2_TexelSize.x;

        float4 pos2 = tex2Dlod(_AnimMap2, float4(animMap_x, animMap_y, 0, 0));

        float4 pos = lerp(pos1,pos2,_AnimLerp);

        v2f o;
        o.vertex = UnityObjectToClipPos(pos);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		//v2f o;
		//o.vertex = UnityObjectToClipPos(v.pos);
		//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        return o;
    }
    
    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv);
        return col;
    }

    float4 fragShadow(v2fShadow i) : SV_Target
    {
        SHADOW_CASTER_FRAGMENT(i)
    }
    ENDCG

    SubShader
    {
		Pass
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
			Cull off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //开启gpu instancing
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"


            ENDCG
        }

		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragShadow
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			ENDCG
		}
    }
}
