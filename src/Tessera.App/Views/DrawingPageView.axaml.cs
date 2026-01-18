using System;
using Avalonia.Controls;
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
        canvas.PointerMoved += (_, pe) =>
            ViewModel?.OnPointerMoved(pe.GetCurrentPoint(canvas));
        canvas.PointerReleased += (_, pe) =>
            ViewModel?.OnPointerReleased(pe.GetCurrentPoint(canvas));
    }
}