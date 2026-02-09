using System.Diagnostics;
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
    
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var pointer = e.GetPosition(CanvasContainer);
        
        Debug.WriteLine($"SCREEN CLICK: {pointer.X}, {pointer.Y}");
        
        ViewModel?.OnPointerPressed(pointer);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        
        ViewModel?.OnPointerMoved(currentPoint);
        ViewModel?.UpdateDebugInfo(currentPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        
        ViewModel?.OnPointerReleased(currentPoint);
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        
        ViewModel?.Zoom(currentPoint, e.Delta.Y);
    }
}