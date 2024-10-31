Shader "Tutorial/DrawCircle"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}

        _PosAndSize1 ("PosAndSize1", Vector) = (0,0,1,1)

        _CanvasSize ("CanvasSize", Vector) = (1,1,0,0)
        _BorderWidth ("BorderWidth", Float) = 0.1
        [Toggle(DRAW_BORDER)] _DrawBorder("Draw border", Float) = 1
        [Toggle] _SoftBorder("Use soft border", Float) = 1
        _BorderColor ("BorderColor", Color) = (1,1,1,1)
        _BaseColor ("BaseColor", Color) = (0,0,0,0.3)
        _TransparentColor ("TransparentColor", Color) = (0,0,0,0)
    }

    SubShader
    {

        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #pragma shader_feature DRAW_BORDER
            #pragma multi_compile __ DRAW_BORDER

            struct v2f
            {
                float4 pos : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            uniform sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform half4 _PosAndSize1;

            uniform half2 _CanvasSize;

            #ifdef DRAW_BORDER
            uniform half _BorderWidth;
            uniform half _SoftBorder;
            uniform fixed4 _BorderColor;
            #endif

            uniform fixed4 _BaseColor;
            uniform fixed4 _TransparentColor;

            v2f vert(v2f v)
            {
                v2f o;
                o.color = v.color;
                o.pos = UnityObjectToClipPos(v.pos);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            half ellipse(half2 canvasUV, half4 posAndSize)
            {
                half x = 2 * (canvasUV.x - posAndSize.x) / posAndSize.z;
                half y = 2 * (canvasUV.y - posAndSize.y) / posAndSize.w;

                #ifdef DRAW_BORDER

                half radius = sqrt(x * x + y * y);

                half radiusH = 1.0 - _BorderWidth / posAndSize.z;
                half radiusV = 1.0 - _BorderWidth / posAndSize.w;
                half radiusAverage = (radiusH + radiusV) * 0.5;

                x = abs(x);
                y = abs(y);
                half minRadius = (x > y)
                                     ? lerp(radiusH, radiusAverage, y / x)
                                     : lerp(radiusV, radiusAverage, x / y);

                return smoothstep(minRadius, 1, radius);

                #else

                return step(1, x * x + y * y);

                #endif
            }

            fixed4 frag(v2f i) : COLOR
            {
                const half2 canvasUV = half2(i.uv.x * _CanvasSize.x, i.uv.y * _CanvasSize.y);
                const half alpha = ellipse(canvasUV, _PosAndSize1);

                half4 color = lerp(_TransparentColor, _BaseColor, alpha);

                #ifdef DRAW_BORDER

                const half borderFactor = alpha * (1 - _SoftBorder);
                if (borderFactor > 0 && borderFactor < 1)
                {
                    color = _BorderColor;
                }

                #endif

                return color * i.color;
            }
            ENDCG
        }
    }
}
