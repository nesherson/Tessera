using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Tessera.App.ViewModels;
using Tessera.App.Models;

namespace Tessera.App.Views;

public partial class DrawingPageView : UserControl
{
    private DrawingPageViewModel? ViewModel => DataContext as DrawingPageViewModel;

    public DrawingPageView()
    {
        InitializeComponent();
    }

    private ToolItem? _previouslySelectedToolItem;

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        
        if (ViewModel == null) return;
        
        ViewModel.ViewportWidth = CanvasContainer.Bounds.Width;
        ViewModel.ViewportHeight = CanvasContainer.Bounds.Height;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var pointer = e.GetPosition(CanvasContainer);
        
        if (ViewModel?.IsToolSettingsOpen == true)
        {
            ViewModel.IsToolSettingsOpen = false;
        }
        
        ViewModel?.OnPointerPressed(pointer);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        
        ViewModel?.OnPointerMoved(currentPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        
        ViewModel?.OnPointerReleased(currentPoint);
    }
    
    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        var delta =  e.Delta.Y;
        
        ViewModel?.OnPointerWheelChanged(currentPoint, delta);
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

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox { DataContext: TextShape shape }) 
            return;

        switch (e.Key)
        {
            case Key.Enter:
                shape.IsEditing = false;
                e.Handled = true;
                break;
            case Key.Escape:
                ViewModel?.Shapes.Remove(shape); 
                e.Handled = true;
                break;
        }
    }

    private void OnToolListBoxTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not ListBox || DataContext is not DrawingPageViewModel vm) 
            return;
        
        var clickedItem = (e.Source as Visual)?.FindAncestorOfType<ListBoxItem>();
        
        if (clickedItem?.DataContext is not ToolItem tappedTool) 
            return;

        if (_previouslySelectedToolItem == tappedTool)
        {
            vm.IsToolSettingsOpen = true;
            _previouslySelectedToolItem = null;
        }
        else
        {
            _previouslySelectedToolItem = tappedTool;
            vm.IsToolSettingsOpen = false;
        }
    }
}