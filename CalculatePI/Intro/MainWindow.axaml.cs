using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace Intro;

public partial class MainWindow : Window
{
    private readonly IPage[] _pages =
    [
        new AreaControl(),
        new TextControl("π"),
        new AnimatedCircleControl()
    ];

    private int _pageNumber = 0;
   
    public MainWindow()
    {
        InitializeComponent();
        Switcher.Content = _pages[_pageNumber];
        _pages[_pageNumber].Display(true);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.P)
        {
            _pageNumber = Math.Max(0, _pageNumber - 1);
            var previousPage = _pages[_pageNumber];
            Switcher.Content = previousPage;
            previousPage.Display(true);
        }
        else if (e.Key == Key.Space)
        {
            if (_pages[_pageNumber].Display(false) == DisplayResult.Completed)
            {
                // Page is complete, display the next page
                _pageNumber = (_pageNumber + 1) % _pages.Length;
                var nextPage = _pages[_pageNumber];
                Switcher.Content = nextPage;
                nextPage.Display(true);
            }
        }
    }
}