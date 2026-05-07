using System.Collections.Generic;
using Tessera.App.Enumerations;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public class TransformManager
{
    private readonly HashSet<ShapeBase> _shapes = [];

    public void ScaleStart(IList<ShapeBase> shapes)
    {
        foreach (var shape in shapes)
            _shapes.Add(shape);
    }

    public void Scale(ResizePoint resizePoint, Vector delta)
    {
        foreach (var shape in _shapes)
        {
            shape.Scale(resizePoint, delta);
        }
    }

    public void ScaleCompleted()
    {
        _shapes.Clear();
    }
}