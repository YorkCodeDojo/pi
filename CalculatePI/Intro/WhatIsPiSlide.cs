using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Intro;

public class WhatIsPiSlide : Control, ISlide
{
    private int _state = 0;
    private int _digitCount = 0;
    private string _pi;
    private readonly DispatcherTimer _timer;


    public WhatIsPiSlide()
    {
        _pi = File.ReadAllText("pi.txt").Replace(" ", "").Replace(System.Environment.NewLine, "");
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        _timer.Tick += (_, _) =>
        {
            if (_state == 2)
            {
                _digitCount += 1;
                if (_digitCount >= (_pi.Length))
                {
                    _timer.Stop();
                }
            }
            
            InvalidateVisual(); // Redraw
        };
        
    }
    
    public DisplayResult Display(bool reset)
    {
        if (reset)
        {
            _state = 1;
            return DisplayResult.MoreToDisplay;
        }
        
        if (_state == 1 )
        {
            _state++;
            _timer.Start();
            InvalidateVisual();
            return DisplayResult.MoreToDisplay;
        }
        
        return DisplayResult.Completed;
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
        if (_state > 1)
            DrawPi(context);
        
        DrawQuestion(context);
    }

    private void DrawPi(DrawingContext context)
    {
        var line = 1;
        var remaining = _digitCount;
        var start = 0;

        while (remaining > 0)
        {
            var toWrite = Math.Min(150, remaining);
            WritePiDigits(context, _pi[start..(start+toWrite)], line);
            line++;
            start += toWrite;
            remaining -= toWrite;
        }
    }

    private static void WritePiDigits(DrawingContext context, string pi, int lineNumber)
    {
        var formattedText = new FormattedText(pi, 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            20, 
            Brushes.Gray);

        var origin = new Point(0,10 + (lineNumber * 30));
        context.DrawText(formattedText, origin);
    }

    private void DrawQuestion(DrawingContext context)
    {
        var formattedText = new FormattedText(
            "Finding π", 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            100, 
            Brushes.White);

        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        var origin = new Point(center.X - formattedText.Width / 2, 
            center.Y - formattedText.Height / 2);

        context.DrawText(formattedText, origin);
    }
}