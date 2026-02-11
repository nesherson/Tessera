using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
        var pointer = e.GetCurrentPoint(CanvasContainer);
        
        ViewModel?.OnPointerPressed(pointer.Position);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var currentPoint = e.GetCurrentPoint(CanvasContainer).Position;
        
        ViewModel?.OnPointerMoved(currentPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var currentPoint = e.GetCurrentPoint(CanvasContainer).Position;
        
        ViewModel?.OnPointerReleased(currentPoint);
    }

    private void OnTextBoxLoaded(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox tb) return;
        
        tb.Focus();
        tb.SelectAll();
    }

    private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox { DataContext: TextShape shape }) return;
        
        if (string.IsNullOrWhiteSpace(shape.Text))
        {
            ViewModel?.Shapes.Remove(shape);
        }

        shape.IsEditing = false;
    }
}