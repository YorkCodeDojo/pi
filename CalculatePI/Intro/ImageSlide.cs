using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Intro;

public class ImageSlide(string filename): Control, ISlide
{
    public DisplayResult Display(bool reset)
    {
        if (reset)
        {
            return DisplayResult.MoreToDisplay;
        }
        return DisplayResult.Completed;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var image = new Bitmap(filename);
        context.DrawImage(image, new Rect(
            (Bounds.Width - image.Size.Width) / 2,
            (Bounds.Height - image.Size.Height) / 2,
            image.Size.Width,image.Size.Height) );
    }
}