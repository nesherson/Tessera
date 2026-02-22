using System.Collections.ObjectModel;
using Avalonia.Input;
using Tessera.App.Models;

namespace Tessera.App.Interfaces;

public interface ICanvasContext
{
    ObservableCollection<ShapeBase> Shapes { get; }
    Cursor CurrentCursor { get; set; }
    RectangleShape EraserRect { get; set; }
    CanvasTransform Transform { get; }
}