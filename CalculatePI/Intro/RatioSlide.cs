using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace Intro;

public class RatioSlide : Control, ISlide
{
    private int _state = 0;
    
    public DisplayResult Display(bool reset)
    {
        if (reset)
        {
            _state = 1;
            return DisplayResult.MoreToDisplay;
        }

        if (_state is >= 1 and < 7 )
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
        var radius = Math.Min(Bounds.Width, Bounds.Height) / 2 - 100;
        var circlePen = new Pen(Brushes.Red, 2);
        var squarePen = new Pen(Brushes.Green, 2);
        var smallSquarePen = new Pen(Brushes.Blue, 2);
        
        if (_state == 1)
        {
            DrawCompletedCircle(context, circlePen, center, radius);
            DisplayText(context, new Point((Bounds.Width / 2) - radius + 50, Bounds.Height / 2), "Area = π r\u00b2");
        }
        
        if (_state == 2)
        {
            DrawCompletedCircle(context, circlePen, center, radius);
            DisplayText(context, new Point((Bounds.Width / 2) - radius + 50, Bounds.Height / 2), "Area = π r\u00b2");

            DrawFullSquare(context, radius, squarePen);
        }
        
        if (_state is >= 3 and < 6)
        {
            DrawFullSquare(context, radius, squarePen);
            
            var topLeft = new Point((Bounds.Width / 2),(Bounds.Height / 2) - radius);
            var bottomRight = new Point((Bounds.Width / 2) + radius,(Bounds.Height / 2));
            context.DrawRectangle(null, smallSquarePen, new Rect(topLeft, bottomRight));
            
            DisplayText(context, new Point((Bounds.Width / 2) + radius + 50, (Bounds.Height / 2) - radius), "r");
            DisplayText(context, new Point((Bounds.Width / 2) + (radius / 2), (Bounds.Height / 2) - radius - 100), "r");
        }

        if (_state is >= 4 and < 6)
        {
            DisplayText(context, new Point((Bounds.Width / 2) + (radius / 2), (Bounds.Height / 2)- (radius / 2)), "r\u00b2");
        }
        
        if (_state == 5)
        {
            DisplayText(context, new Point((Bounds.Width / 2) - (radius / 2), (Bounds.Height / 2) + (radius / 2)), "4 x r\u00b2");
        }
        
        if (_state == 6)
        {
            var middle = new Point((Bounds.Width / 2) - (radius / 2) - 100, (Bounds.Height / 2) - (radius / 2) + 50);
            DrawCompletedCircle(context, circlePen, middle, 70);
            DisplayText(context, new Point((Bounds.Width / 2) - (radius / 2), (Bounds.Height / 2) - (radius / 2)), " = π x r\u00b2");
            
            var topLeft = new Point((Bounds.Width / 2) - (radius / 2) - 150, (Bounds.Height / 2) + (radius / 4));
            var bottomRight = new Point(topLeft.X + 100,topLeft.Y + 100);
            context.DrawRectangle(null, smallSquarePen, new Rect(topLeft, bottomRight));
            DisplayText(context, new Point((Bounds.Width / 2) - (radius / 2), (Bounds.Height / 2) + (radius / 4)), " = 4 x r\u00b2");
        }
        
        if (_state == 7)
        {
            var middle = new Point((Bounds.Width / 2) - (radius / 2) - 100, (Bounds.Height / 2) - (radius / 2) + 50);
            DrawCompletedCircle(context, circlePen, middle, 70);
            DisplayText(context, new Point((Bounds.Width / 2) - (radius / 2), (Bounds.Height / 2) - (radius / 2)), " = π");
            
            context.DrawLine(squarePen,
                new Point((Bounds.Width / 2) - radius - 100, (Bounds.Height / 2)),
                new Point((Bounds.Width / 2) + radius - 100, (Bounds.Height / 2)));
            
            var topLeft = new Point((Bounds.Width / 2) - (radius / 2) - 150, (Bounds.Height / 2) + (radius / 4));
            var bottomRight = new Point(topLeft.X + 100,topLeft.Y + 100);
            context.DrawRectangle(null, smallSquarePen, new Rect(topLeft, bottomRight));
            DisplayText(context, new Point((Bounds.Width / 2) - (radius / 2), (Bounds.Height / 2) + (radius / 4)), " = 4");
        }
        
    }

    private void DrawFullSquare(DrawingContext context, double radius, Pen pen)
    {
        var topLeft = new Point((Bounds.Width / 2) - radius,(Bounds.Height / 2) - radius);
        var bottomRight = new Point((Bounds.Width / 2) + radius,(Bounds.Height / 2) + radius);
        context.DrawRectangle(null, pen, new Rect(topLeft, bottomRight));
    }

    private static void DrawCompletedCircle(DrawingContext context, Pen circlePen, Point center, double radius)
    {
        context.DrawEllipse(null, circlePen, center, radius, radius);
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
}