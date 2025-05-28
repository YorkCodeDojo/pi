using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Intro;

public class AreaControl : Control, IPage
{
    private double _sweepAngle = 0;
    private double _divideAngle = 0;
    private readonly DispatcherTimer _timer;
    private int _state = 0;
    
    
    public AreaControl()
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
                _divideAngle += 18;
                if (_divideAngle >= 360)
                {
                    _divideAngle =360;
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
            _divideAngle = 0;
            _timer.Start();
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
            
            var rad = _divideAngle * Math.PI / 180;
            var endPoint = new Point(
                center.X + radius * Math.Cos(rad),
                center.Y + radius * Math.Sin(rad));
            context.DrawLine(pen, center, endPoint);
        }

    }

    private static void DrawCompletedCircle(DrawingContext context, Pen circlePen, Point center, double radius)
    {
        context.DrawEllipse(null, circlePen, center, radius, radius);
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