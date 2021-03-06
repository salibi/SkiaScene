﻿using SkiaSharp;
using System.Reflection;

namespace SkiaScene.NativeSample
{
    public class SvgSceneRenderer : ISKSceneRenderer
    {
        private SKPicture _svgScene;
        private string _fileName = "test.svg";

        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            if (_svgScene == null)
            {
                _svgScene = LoadScene();
            }
            canvas.Clear(SKColors.White);
            canvas.DrawPicture(_svgScene);
        }

        private SKPicture LoadScene()
        {
            var svg = new SkiaSharp.Extended.Svg.SKSvg();
            var fileName = $"SkiaScene.NativeSample.{_fileName}";
            var assembly = typeof(SvgSceneRenderer).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(fileName);
            var result = svg.Load(stream);
            return result;
        }
    }
}
