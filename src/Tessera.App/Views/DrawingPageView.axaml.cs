using System.Diagnostics;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
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
        if (e.Properties.IsMiddleButtonPressed) return;

        var pointer = e.GetPosition(CanvasContainer);

        if (ViewModel?.IsToolSettingsOpen == true)
        {
            ViewModel.IsToolSettingsOpen = false;
        }

        ViewModel?.OnPointerPressed(pointer);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (e.Properties.IsMiddleButtonPressed) return;

        var currentPoint = e.GetPosition(CanvasContainer);

        ViewModel?.OnPointerMoved(currentPoint);
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (e.Properties.IsMiddleButtonPressed) return;

        var currentPoint = e.GetPosition(CanvasContainer);

        ViewModel?.OnPointerReleased(currentPoint);
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var currentPoint = e.GetPosition(CanvasContainer);
        var delta = e.Delta.Y;

        ViewModel?.OnPointerWheelChanged(currentPoint, delta);
    }

    private void OnTextBoxLoaded(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox tb) return;
        
        if (tb.DataContext is TextShape { IsEditing: true })
        {
            tb.Focus();
            return;
        }
        
        if (tb.DataContext is TextShape shape)
        {
            shape.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(TextShape.IsEditing) && shape.IsEditing)
                {
                    Dispatcher.UIThread.Post(() => tb.Focus());
                }
            };
        }
    }

    private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox { DataContext: TextShape shape }) return;
        
        FinalizeTextShape(shape);
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape && sender is TextBox { DataContext: TextShape shape })
        {
            FinalizeTextShape(shape);
            e.Handled = true;
        }
    }
    
    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox tb) 
            return;
        
        if (tb.DataContext is not TextShape shape)
            return;
        
        Dispatcher.UIThread.Post(() =>
        {
            var lineCount = tb.Text?.Split('\n').Length ?? 1;
            var lineHeight = shape.FontSize * 1.4;
            var requiredHeight = lineCount * lineHeight;

            shape.Height = Math.Max(requiredHeight, 16);
        });
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
    
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Delete:
                ViewModel?.RemoveSelectedShapesCommand.Execute(null);
                break;
        }
    }
    
    private void FinalizeTextShape(TextShape shape)
    {
        shape.IsEditing = false;

        if (string.IsNullOrWhiteSpace(shape.Text))
        {
            ViewModel?.Shapes.Remove(shape);
        }
    }
}