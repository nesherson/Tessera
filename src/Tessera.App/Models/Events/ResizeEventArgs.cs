using Avalonia.Input;
using Tessera.App.Enumerations;

namespace Tessera.App.Models;

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