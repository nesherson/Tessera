using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class TriangleShape : ShapeBase
{
    [ObservableProperty]
    private ObservableCollection<Point> _points = [];
    public override bool Intersects(Rect rect)
    {
        throw new NotImplementedException();
    }

    public override bool HitTest(Point worldPoint, double tolerance)
    {
        throw new NotImplementedException();
    }

    public override void Move(Vector delta)
    {
        throw new NotImplementedException();
    }

    public override Rect GetBounds()
    {
        throw new NotImplementedException();
    }
}