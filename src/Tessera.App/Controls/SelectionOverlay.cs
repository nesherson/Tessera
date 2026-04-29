using System.Diagnostics;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Styling;
using Tessera.App.Enumerations;

namespace Tessera.App.Controls;

public class SelectionOverlay : Canvas
{
    public static readonly StyledProperty<Rect> SelectionBoundsProperty =
        AvaloniaProperty.Register<SelectionOverlay, Rect>(nameof(SelectionBounds));

    public static readonly StyledProperty<bool> HasSelectionProperty =
        AvaloniaProperty.Register<SelectionOverlay, bool>(nameof(HasSelection));
    
    private readonly Rectangle _border;
    private readonly Thumb[] _cornerHandles = new Thumb[4];
    private readonly Thumb[] _edgeHandles = new Thumb[4];

    private static readonly ResizePoint[] CornerPoints =
        [ResizePoint.TopLeft, ResizePoint.TopRight, ResizePoint.BottomLeft, ResizePoint.BottomRight];

    private static readonly ResizePoint[] EdgePoints =
        [ResizePoint.Top, ResizePoint.Bottom, ResizePoint.Left, ResizePoint.Right];

    private static readonly StandardCursorType[] CornerCursors =
    [StandardCursorType.TopLeftCorner, StandardCursorType.TopRightCorner,
        StandardCursorType.BottomLeftCorner, StandardCursorType.BottomRightCorner];

    private static readonly StandardCursorType[] EdgeCursors =
    [StandardCursorType.TopSide, StandardCursorType.BottomSide,
        StandardCursorType.LeftSide, StandardCursorType.RightSide];

    public SelectionOverlay()
    {
        IsVisible = false;
        
        _border = new Rectangle
        {
            Stroke = Brushes.DodgerBlue,
            StrokeThickness = 2,
            IsHitTestVisible = false
        };
        
        Children.Add(_border);
        
        for (var i = 0; i < 4; i++)
        {
            _cornerHandles[i] = CreateCornerThumb(CornerPoints[i], CornerCursors[i]);
            
            Children.Add(_cornerHandles[i]);
        }

        for (var i = 0; i < 4; i++)
        {
            _edgeHandles[i] = CreateEdgeThumb(EdgePoints[i], EdgeCursors[i]);
            
            Children.Add(_edgeHandles[i]);
        }
    }
    
    public Rect SelectionBounds
    {
        get => GetValue(SelectionBoundsProperty);
        set => SetValue(SelectionBoundsProperty, value);
    }

    public bool HasSelection
    {
        get => GetValue(HasSelectionProperty);
        set => SetValue(HasSelectionProperty, value);
    }
    
    // Events for resize operations
    public event EventHandler<ResizeEventArgs>? ResizeStarted;
    public event EventHandler<ResizeEventArgs>? ResizeDelta;
    public event EventHandler<ResizeEventArgs>? ResizeCompleted;
    
    private Thumb CreateCornerThumb(ResizePoint point, StandardCursorType cursor)
    {
        var thumb = new Thumb
        {
            Tag = point,
            Width = 8,
            Height = 8,
            Cursor = new Cursor(cursor),
            Theme = Application.Current?.FindResource("ThumbTheme") as ControlTheme
        };

        thumb.DragStarted += OnThumbDragStarted;
        thumb.DragDelta += OnThumbDragDelta;
        thumb.DragCompleted += OnThumbDragCompleted;

        return thumb;
    }

    private Thumb CreateEdgeThumb(ResizePoint point, StandardCursorType cursor)
    {
        var thumb = new Thumb
        {
            Tag = point,
            Cursor = new Cursor(cursor),
            IsHitTestVisible = true,
            Theme = Application.Current?.FindResource("EdgeThumbTheme") as ControlTheme
        };

        thumb.DragStarted += OnThumbDragStarted;
        thumb.DragDelta += OnThumbDragDelta;
        thumb.DragCompleted += OnThumbDragCompleted;

        return thumb;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectionBoundsProperty || change.Property == HasSelectionProperty)
            UpdateLayout();
    }
    
    private new void UpdateLayout()
    {
        if (!HasSelection)
        {
            IsVisible = false;
            
            return;
        }
        
        IsVisible = true;

        var b = SelectionBounds;
        
        // Border
        SetLeft(_border, b.X);
        SetTop(_border, b.Y);
        
        _border.Width = b.Width;
        _border.Height = b.Height;

        // Corners
        PositionCorner(_cornerHandles[0], b.Left, b.Top);
        PositionCorner(_cornerHandles[1], b.Right, b.Top);
        PositionCorner(_cornerHandles[2], b.Left, b.Bottom);
        PositionCorner(_cornerHandles[3], b.Right, b.Bottom);

        // Edges
        PositionEdge(_edgeHandles[0], b.Left, b.Top, b.Width, 6);      // top
        PositionEdge(_edgeHandles[1], b.Left, b.Bottom - 3, b.Width, 6); // bottom
        PositionEdge(_edgeHandles[2], b.Left, b.Top, 6, b.Height);      // left
        PositionEdge(_edgeHandles[3], b.Right - 3, b.Top, 6, b.Height); // right
    }

    private static void PositionCorner(Thumb thumb, double x, double y)
    {
        SetLeft(thumb, x - 4);
        SetTop(thumb, y - 4);
    }

    private static void PositionEdge(Thumb thumb, double x, double y, double w, double h)
    {
        Debug.WriteLine($"PositionEdge -> x: {x}, y: {y}, w: {w}, h: {h}");
        SetLeft(thumb, x);
        SetTop(thumb, y);
        thumb.Width = w;
        thumb.Height = h;
    }

    private void OnThumbDragStarted(object? sender, VectorEventArgs e)
        => ResizeStarted?.Invoke(sender, new ResizeEventArgs((ResizePoint)((Thumb)sender!).Tag!, e));

    private void OnThumbDragDelta(object? sender, VectorEventArgs e)
        => ResizeDelta?.Invoke(sender, new ResizeEventArgs((ResizePoint)((Thumb)sender!).Tag!, e));

    private void OnThumbDragCompleted(object? sender, VectorEventArgs e)
        => ResizeCompleted?.Invoke(sender, new ResizeEventArgs((ResizePoint)((Thumb)sender!).Tag!, e));
}

public class ResizeEventArgs
{
    public ResizePoint Handle { get; }
    public VectorEventArgs DragArgs { get; }

    public ResizeEventArgs(ResizePoint handle, VectorEventArgs dragArgs)
    {
        Handle = handle;
        DragArgs = dragArgs;
    }
}