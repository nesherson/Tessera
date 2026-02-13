using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Input;
using Tessera.App.ViewModels;

namespace Tessera.App.Interfaces;

public interface ICanvasContext
{
    ObservableCollection<ShapeBase> Shapes { get; }
    double OffsetX { get; set; }
    double OffsetY { get; set; }
    Cursor CurrentCursor { get; set; }
    RectangleShape EraserRect { get; set; }
    Point ToWorld(Point screenPoint);
}