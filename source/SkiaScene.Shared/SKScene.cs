﻿using System;
using SkiaSharp;

namespace SkiaScene
{
    public class SKScene : ISKScene
    {
        protected readonly ISKSceneRenderer _sceneRenderer;
        protected SKMatrix Matrix = SKMatrix.MakeIdentity();
        protected readonly SKPoint DefaultCenter;

        public SKScene(ISKSceneRenderer sceneRenderer, SKSize canvasSize)
        {
            _sceneRenderer = sceneRenderer;
            DefaultCenter = new SKPoint(canvasSize.Width / 2, canvasSize.Height / 2);
        }

        public void Render(SKCanvas canvas)
        {
            canvas.Concat(ref Matrix);
            var center = GetCenter();
            var angleInRadians = GetAngleInRadians();
            var scale = GetScale();
            _sceneRenderer.Render(canvas, angleInRadians, center, scale);
        }

        public void MoveByVector(SKPoint vector)
        {
            SKMatrix invertedMatrix;
            if (!Matrix.TryInvert(out invertedMatrix))
            { 
                return;
            }
            var resultPoint = invertedMatrix.MapVector(vector.X, vector.Y);
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeTranslation(resultPoint.X, resultPoint.Y));
        }
        
        public void MoveToPoint(SKPoint point)
        {
            var center = GetCenter();
            SKPoint diff = center - point;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeTranslation(diff.X, diff.Y));
        }
        
        public void Rotate(SKPoint point, float radians)
        {
            var currentAngle = GetAngleInRadians();
            var angleDiff = radians - currentAngle;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeRotation(angleDiff, point.X, point.Y));
        }

        public void RotateByRadiansDelta(SKPoint point, float radiansDelta)
        {
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeRotation(radiansDelta, point.X, point.Y));
        }

        public void Zoom(SKPoint point, float scale)
        {
            var currentScale = GetScale();
            var scaleFactor = scale / currentScale;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeScale(scaleFactor, scaleFactor, point.X, point.Y));
        }
        
        public void ZoomByScaleFactor(SKPoint point, float scaleFactor)
        {
            var currentScale = GetScale();
            currentScale *= scaleFactor;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.MakeScale(scaleFactor, scaleFactor, point.X, point.Y));
        }


        public SKPoint GetCanvasPointFromViewPoint(SKPoint viewPoint)
        {

            SKMatrix invertedMatrix;
            if (!Matrix.TryInvert(out invertedMatrix))
            {
                return SKPoint.Empty;
            }
            return invertedMatrix.MapPoint(viewPoint);
        }
        
        public SKPoint GetCenter()
        {
            return GetCanvasPointFromViewPoint(DefaultCenter);
        }

        /// <summary>
        /// Returns number from interval [-pi to pi]
        /// </summary>
        /// <returns></returns>
        public float GetAngleInRadians()
        {
            //https://stackoverflow.com/questions/4361242/extract-rotation-scale-values-from-2d-transformation-matrix
            var skewY = Matrix.SkewY;
            var scaleY = Matrix.ScaleY;
            var result = Math.Atan2(skewY, scaleY);
            return (float)result;
        }

        public float GetScale()
        {
            //https://stackoverflow.com/questions/4361242/extract-rotation-scale-values-from-2d-transformation-matrix
            var scaleX = Matrix.ScaleX;
            var skewY = Matrix.SkewY;
            var result = Math.Sqrt(scaleX * scaleX + skewY * skewY);
            return (float) result;
        }
    }
}
