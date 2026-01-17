using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tessera.App.ViewModels;

namespace Tessera.App.Views;

public partial class DrawingPageView : UserControl
{
    private DrawingPageViewModel? ViewModel => DataContext as DrawingPageViewModel;
    public DrawingPageView()
    {
        InitializeComponent();
    }
    
    private void Canvas_OnInitialized(object? sender, EventArgs e)
    {
        if (sender is not Canvas canvas)
            return;

        canvas.PointerPressed += (_, pe) =>
            ViewModel?.OnPointerPressed(pe.GetCurrentPoint(canvas));
    }
}