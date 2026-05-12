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
    
    public static readonly StyledProperty<Matrix> TransformMatrixProperty =
        AvaloniaProperty.Register<SelectionOverlay, Matrix>(nameof(TransformMatrix));

    public SelectionOverlay()
    {
        IsVisible = false;
        
        _border = new Rectangle
        {
            Stroke = Brushes.DodgerBlue,
            StrokeThickness = 2,
            IsHitTestVisible = false,
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
    
    public Matrix TransformMatrix
    {
        get => GetValue(TransformMatrixProperty);
        set => SetValue(TransformMatrixProperty, value);
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
            Width = 10,
            Height = 10,
            Cursor = new Cursor(cursor),
            Theme = Application.Current?.FindResource("ThumbTheme") as ControlTheme,
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

        if (change.Property == SelectionBoundsProperty 
            || change.Property == HasSelectionProperty 
            || change.Property == TransformMatrixProperty)
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

        var worldBounds = SelectionBounds;
        
        var topLeft = worldBounds.TopLeft.Transform(TransformMatrix);
        var bottomRight = worldBounds.BottomRight.Transform(TransformMatrix);
        var screenBounds = new Rect(topLeft, bottomRight);
        
        // Border
        SetLeft(_border, screenBounds.X);
        SetTop(_border, screenBounds.Y);
        
        _border.Width = screenBounds.Width;
        _border.Height = screenBounds.Height;

        // Corners
        PositionCorner(_cornerHandles[0], screenBounds.Left, screenBounds.Top);
        PositionCorner(_cornerHandles[1], screenBounds.Right, screenBounds.Top);
        PositionCorner(_cornerHandles[2], screenBounds.Left, screenBounds.Bottom);
        PositionCorner(_cornerHandles[3], screenBounds.Right, screenBounds.Bottom);

        // Edges
        PositionEdge(_edgeHandles[0], screenBounds.Left, screenBounds.Top, screenBounds.Width, 6);      // top
        PositionEdge(_edgeHandles[1], screenBounds.Left, screenBounds.Bottom - 3, screenBounds.Width, 6); // bottom
        PositionEdge(_edgeHandles[2], screenBounds.Left, screenBounds.Top, 6, screenBounds.Height);      // left
        PositionEdge(_edgeHandles[3], screenBounds.Right - 3, screenBounds.Top, 6, screenBounds.Height); // right
    }

    private static void PositionCorner(Thumb thumb, double x, double y)
    {
        SetLeft(thumb, x - thumb.Width / 2);
        SetTop(thumb, y - thumb.Height / 2);
    }

    private static void PositionEdge(Thumb thumb, double x, double y, double w, double h)
    {
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