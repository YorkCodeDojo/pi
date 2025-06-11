using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Intro;

public class CircumferenceSlide : Control, ISlide
{
    private double _sweepAngle = 0;
    private double _lineLength = 0;
    private readonly DispatcherTimer _timer;
    private int _state = 0;
    
    
    public CircumferenceSlide()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        _timer.Tick += (_, _) =>
        {
            if (_state == 1)
            {
                _sweepAngle += 2; // Increase angle
                if (_sweepAngle >= 360)
                {
                    _sweepAngle = 360;
                    _timer.Stop();
                }
            }

            if (_state == 2)
            {
                var radius = Math.Min(Bounds.Width, Bounds.Height) / 2 - 20;
                _lineLength += 10;
                if (_lineLength >= (radius * 2))
                {
                    _lineLength = radius * 2;
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
            _sweepAngle = 0;
            _timer.Start();
            _state = 1;
            return DisplayResult.MoreToDisplay;
        }

        if (_state == 1)
        {
            _state = 2;
            _lineLength = 0;
            _timer.Start();
            InvalidateVisual();
            return DisplayResult.MoreToDisplay;
        }

        if (_state is >= 2 and < 5 )
        {
            _state++;
            InvalidateVisual();
            return DisplayResult.MoreToDisplay;
        }
        
        return DisplayResult.Completed;
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        var radius = Math.Min(Bounds.Width, Bounds.Height) / 2 - 20;
        var circlePen = new Pen(Brushes.Red, 2);
        var pen = new Pen(Brushes.Green, 2);
        
        if (_state == 1)
        {
            if (_sweepAngle >= 360)
            {
                DrawCompletedCircle(context, circlePen, center, radius);
            }
            else
            {
                var geometry = CreateArcGeometry(center, radius, 0, _sweepAngle);
                context.DrawGeometry(null, circlePen, geometry);
            }
        }

        if (_state == 2)
        {
            DrawCompletedCircle(context, circlePen, center, radius);
            context.DrawLine(pen, 
                new Point(center.X - radius, Bounds.Height / 2), 
                new Point(center.X - radius + _lineLength, Bounds.Height / 2));

            if (_lineLength >= radius * 2)
            {
                DrawCompletedLine(context, pen, center, radius);
            }
        }

        if (_state >= 3)
        {
            DrawCompletedCircle(context, circlePen, center, radius);
            DrawCompletedLine(context, pen, center, radius);
            EquationOne(context, center);
        }
        
        if (_state >= 4)
        {
            EquationTwo(context, center);
        }
        
        if (_state >= 5)
        {
            EquationThree(context, center);
        }
    }

    private static void EquationOne(DrawingContext context, Point center)
    {
        var formattedText = new FormattedText(
            "π = c / d", 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            100, 
            Brushes.White);

        var origin = new Point(center.X - 200, center.Y - 200);

        context.DrawText(formattedText, origin);
    }

    private static void EquationTwo(DrawingContext context, Point center)
    {
        var formattedText = new FormattedText(
            "c = π d",
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            100,
            Brushes.White);

        var origin = new Point(center.X - 200, center.Y + 50);

        context.DrawText(formattedText, origin);
    }
    private static void EquationThree(DrawingContext context, Point center)
    {
        var formattedText = new FormattedText(
                "c = 2 π r", 
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, 
                new Typeface("Segoe UI"), 
                100, 
                Brushes.White);

        var origin = new Point(center.X - 200, center.Y + 150);

        context.DrawText(formattedText, origin);
    }
    
    private void DrawCompletedLine(DrawingContext context, Pen pen, Point center, double radius)
    {
        context.DrawLine(pen, 
            new Point(center.X - radius, Bounds.Height / 2), 
            new Point(center.X + radius, Bounds.Height / 2));
                
        var formattedText = new FormattedText(
            "d", 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            100, 
            Brushes.White);

        var origin = new Point(center.X + radius + 50, (Bounds.Height / 2) - 50);

        context.DrawText(formattedText, origin);
    }

    private static void DrawCompletedCircle(DrawingContext context, Pen circlePen, Point center, double radius)
    {
        context.DrawEllipse(null, circlePen, center, radius, radius);
                
        var formattedText = new FormattedText(
            "c", 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            100, 
            Brushes.White);

        var origin = new Point(center.X + 300, 30);

        context.DrawText(formattedText, origin);
    }

    private static StreamGeometry CreateArcGeometry(Point center, double radius, double startAngle, double endAngle)
    {
        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        var startRad = startAngle * Math.PI / 180;
        var endRad = endAngle * Math.PI / 180;

        var startPoint = new Point(
            center.X + radius * Math.Cos(startRad),
            center.Y + radius * Math.Sin(startRad));

        var endPoint = new Point(
            center.X + radius * Math.Cos(endRad),
            center.Y + radius * Math.Sin(endRad));

        var isLargeArc = (endAngle - startAngle) > 180;

        context.BeginFigure(startPoint, false);
        context.ArcTo(endPoint, new Size(radius, radius), 0,
            isLargeArc, SweepDirection.Clockwise);

        return geometry;
    }
}