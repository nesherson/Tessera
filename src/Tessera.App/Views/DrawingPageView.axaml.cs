using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        // canvas.PointerPressed += (_, pe) =>
        //     ViewModel?.OnPointerPressed(pe.GetPosition(canvas.Parent as Visual));
        // canvas.PointerMoved += (_, pe) =>
        //     ViewModel?.OnPointerMoved(pe.GetPosition(canvas.Parent as Visual));
        // canvas.PointerReleased += (_, pe) =>
        //     ViewModel?.OnPointerReleased(pe.GetPosition(canvas.Parent as Visual));
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var screenPoint = e.GetPosition(sender as Visual);
        
        ViewModel?.OnPointerPressed(screenPoint);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var screenPoint = e.GetPosition(sender as Visual);
        
        ViewModel?.OnPointerMoved(screenPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var screenPoint = e.GetPosition(sender as Visual);
        
        ViewModel?.OnPointerReleased(screenPoint);
    }
}