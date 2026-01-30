using Avalonia.Controls;
using Avalonia.Input;
using Tessera.App.ViewModels;

namespace Tessera.App.Views;

public partial class DrawingPageView : UserControl
{
    private readonly DrawingPageViewModel _vm;

    public DrawingPageView()
    {
        InitializeComponent();
        
        _vm = new DrawingPageViewModel();
    }
    
    public DrawingPageView(DrawingPageViewModel vm)
    {
        InitializeComponent();
        
        _vm = vm;
    }
    
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var pointer = e.GetCurrentPoint(CanvasContainer);
        
        _vm.OnPointerPressed(pointer.Position);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var currentPoint = e.GetCurrentPoint(CanvasContainer).Position;
        
        _vm.OnPointerMoved(currentPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var currentPoint = e.GetCurrentPoint(CanvasContainer).Position;
        
        _vm.OnPointerReleased(currentPoint);
    }
}