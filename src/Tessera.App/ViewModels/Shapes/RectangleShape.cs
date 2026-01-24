using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class RectangleShape : ShapeBase
{
    public override bool Intersects(Rect rect)
    {
        return false;
    }
}