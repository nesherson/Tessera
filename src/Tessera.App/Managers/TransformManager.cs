using System.Collections.Generic;
using Tessera.App.Enumerations;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public class TransformManager
{
    private readonly HashSet<ShapeBase> _shapes = [];
    private readonly Dictionary<ShapeBase, Rect> _shapeBounds = [];

    public void ScaleStart(IList<ShapeBase> shapes)
    {
        foreach (var shape in shapes)
        {
            _shapes.Add(shape);
            _shapeBounds.Add(shape, shape.GetBounds());
        }
    }

    public void Scale(ResizePoint resizePoint, Vector delta)
    {
        foreach (var shape in _shapes)
        {
            var shapeBounds = _shapeBounds[shape];

            _shapeBounds[shape] = shape.Scale(shapeBounds, resizePoint, delta);
        }
    }

    public void ScaleCompleted()
    {
        _shapes.Clear();
        _shapeBounds.Clear();
    }
}