using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Intro;

public class AreaSlide : Control, ISlide
{
    private double _sweepAngle = 0;
    private double _divideAngle = 0;
    private double _slide = 0;
    private readonly DispatcherTimer _timer;
    private int _state = 0;
    private readonly List<Point> _divisions = [];
    
    public AreaSlide()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
        };
        _timer.Tick += (_, _) =>
        {
            var radius = Math.Min(Bounds.Width, Bounds.Height) / 2 - 20;
            
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
                var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
                var rad = _divideAngle * Math.PI / 180;
                var endPoint = new Point(
                    center.X + radius * Math.Cos(rad),
                    center.Y + radius * Math.Sin(rad));
                _divisions.Add(endPoint);
            }
            
            if (_state == 4)
            {
                _slide += 10;
                if (_slide >= (radius / 2) - 5)
                {
                    _slide = (radius / 2) - 5;
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

        if (_state == 2)
        {
            _state = 3;
            _slide = 0;
            InvalidateVisual();
            return DisplayResult.MoreToDisplay;
        }
        
        if (_state == 3)
        {
            _state = 4;
            _slide = 0;
            _timer.Start();
            InvalidateVisual();
            return DisplayResult.MoreToDisplay;
        }
        
        if (_state <= 9)
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
            
            foreach (var point in _divisions)
            {
                context.DrawLine(pen, center, point);    
            }
            
        }

        if (_state >= 3)
        {
            // Slide slices together
            for (var xOffset = 0; xOffset < 10; xOffset++)
            {
                var piece = DrawBottomSlice(new Point(90 + (xOffset * 160), center.Y - _slide), radius);
                context.DrawGeometry(Brushes.Blue, new Pen(Brushes.Yellow, 2), piece);
            }

            for (var xOffset = 0; xOffset < 10; xOffset++)
            {
                var piece = DrawTopSlice(new Point(170 + (xOffset * 160), center.Y + _slide), radius);
                context.DrawGeometry(Brushes.Red, new Pen(Brushes.Green, 2), piece);
            }
        }

        if (_state >= 5)
        {
            //Display radius
            var formattedText = new FormattedText(
                "r", 
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, 
                new Typeface("Segoe UI"), 
                100, 
                Brushes.White);

            var origin = new Point(Bounds.Width - 60, center.Y - 40);
            context.DrawText(formattedText, origin);
        }

        if (_state >= 6)
        {
            DisplayText(context, new Point(60, center.Y - _slide - 200), "1/2 x c   ");
        }

        if (_state >= 7)
        {
            DisplayText(context, new Point(400, center.Y - _slide - 200), "= 1/2 x (2 π r)");
        }
        if (_state >= 8)
        {
            DisplayText(context, new Point(400, center.Y - _slide - 100), "= π r");
        }
        if (_state >= 9)
        {
            DisplayText(context, new Point(60, Bounds.Height - 150), "Area = π r\u00b2");
        }
    }

    private void DisplayText(DrawingContext context, Point origin, string text)
    {
        var formattedText = new FormattedText(
            text, 
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight, 
            new Typeface("Segoe UI"), 
            100, 
            Brushes.White);

        context.DrawText(formattedText, origin);
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

    private static StreamGeometry DrawBottomSlice(Point center, double radius)
    {
        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        var startRad = (90-9) * Math.PI / 180;
        var endRad = (90+9) * Math.PI / 180;

        var startPoint = new Point(
            center.X + radius * Math.Cos(startRad),
            center.Y + radius * Math.Sin(startRad));

        var endPoint = new Point(
            center.X + radius * Math.Cos(endRad),
            center.Y + radius * Math.Sin(endRad));

        const bool isLargeArc = false;

        context.BeginFigure(startPoint, false);
        context.ArcTo(endPoint, new Size(radius, radius), 0,
            isLargeArc, SweepDirection.Clockwise);

        context.LineTo(center);
        context.LineTo(startPoint);   
        context.EndFigure(true);
        return geometry;
    }
    
    private static StreamGeometry DrawTopSlice(Point center, double radius)
    {
        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        var startRad = (90-9) * Math.PI / 180;
        var endRad = (90+9) * Math.PI / 180;

        var startPoint = new Point(
            center.X + radius * Math.Cos(startRad),
            center.Y - radius * Math.Sin(startRad));

        var endPoint = new Point(
            center.X + radius * Math.Cos(endRad),
            center.Y - radius * Math.Sin(endRad));

        const bool isLargeArc = false;

        context.BeginFigure(startPoint, false);
        context.ArcTo(endPoint, new Size(radius, radius), 0,
            isLargeArc, SweepDirection.CounterClockwise);

        context.LineTo(center);
        context.LineTo(startPoint);   
        
        context.EndFigure(true);
        
        return geometry;
    }
}