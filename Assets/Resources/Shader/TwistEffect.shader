// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Test/Twist Effect" {

    SubShader
    {
      Tags  {"Queue" = "Overlay"}
      Pass
      {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest 

        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;
        uniform float _Angle;
        uniform float4 _CenterRadius;

        struct v2f {
          float4 pos : POSITION;
          float2 origin : TEXCOORD0;
        };

        v2f vert(appdata_img v)
        {
          v2f o;
          o.pos = UnityObjectToClipPos(v.vertex);
          float2 uv = v.texcoord.xy - _CenterRadius.xy;
          o.origin = uv;
          return o;
        }

        float4 frag(v2f i) : COLOR
        {
          float2 offset = i.origin;
          //angle:点距离旋转中心的长度
          float angle = 1.0 - length(offset / _CenterRadius.zw);
          angle = max(0, angle);
          //angle:点旋转的角度
          angle = angle * angle * _Angle;
          float cosLength, sinLength;
          sincos(angle, sinLength, cosLength);
          //计算点采样颜色的uv坐标
          float2 uv;
          uv.x = cosLength * offset[0] - sinLength * offset[1];
          uv.y = sinLength * offset[0] + cosLength * offset[1];
          uv += _CenterRadius.xy;

          return tex2D(_MainTex, uv);
        }
        ENDCG

      }
    }

        Fallback off

}