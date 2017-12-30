﻿using TouchTracking;
using SkiaSharp;

namespace SkiaScene.TouchManipulation
{
    public interface ITouchManipulationManager
    {
        TouchManipulationMode TouchManipulationMode { get; set; }
        void ProcessTouchEvent(long id, TouchActionType type, SKPoint location);
    }
}