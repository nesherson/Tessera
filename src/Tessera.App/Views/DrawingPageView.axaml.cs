using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Tessera.App.ViewModels;

namespace Tessera.App.Views;

public partial class DrawingPageView : UserControl
{
    private DrawingPageViewModel? ViewModel => DataContext as DrawingPageViewModel;
    
    private bool _isPressed;
    private bool _isDragging;
    private Point _startPoint;
    
    public DrawingPageView()
    {
        InitializeComponent();
    }
    
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // var screenPoint = e.GetPosition(sender as Visual);
        //
        // ViewModel?.OnPointerPressed(screenPoint);
        
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
        {
            _isPressed = true;
            _isDragging = false; // Reset drag flag
            _startPoint = point.Position;
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isPressed) return;

        var currentPoint = e.GetCurrentPoint(this).Position;
        var delta = currentPoint - _startPoint;

        // 2. Check if the user has moved the mouse enough to consider it a "Pan" operation
        // (This prevents accidental pans when trying to click)
        if (!_isDragging && (System.Math.Abs(delta.X) > 3 || System.Math.Abs(delta.Y) > 3))
        {
            _isDragging = true;
        }

        if (_isDragging)
        {
            // 3. Pan the canvas via ViewModel
            if (DataContext is DrawingPageViewModel vm)
            {
                vm.Pan(delta.X, delta.Y);
            }
            
            // Update start point for the next move event to get a continuous delta
            _startPoint = currentPoint;
        }
        // var screenPoint = e.GetPosition(sender as Visual);
        //
        // ViewModel?.OnPointerMoved(screenPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isPressed) return;

        // 4. If we didn't drag, treat it as a Click -> Add Point
        if (!_isDragging)
        {
            var point = e.GetCurrentPoint(this).Position;
            if (DataContext is DrawingPageViewModel vm)
            {
                vm.AddPoint(point);
            }
        }

        _isPressed = false;
        _isDragging = false;
        // var screenPoint = e.GetPosition(sender as Visual);
        //
        // ViewModel?.OnPointerReleased(screenPoint);
    }
}