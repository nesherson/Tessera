using System.Collections.Generic;
using System.Linq;
using Tessera.App.Enumerations;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public class TransformManager
{
    private readonly HashSet<ShapeBase> _shapes = [];
    private double _minWidth;
    private double _minHeight;

    public void ScaleStart(IList<ShapeBase> shapes)
    {
        foreach (var shape in shapes)
            _shapes.Add(shape);
        
        _minWidth = _shapes.MaxBy(s => s.MinWidth)?.MinWidth ?? 10;
        _minHeight = _shapes.MaxBy(s => s.MinHeight)?.MinHeight ?? 10;
    }

    public void Scale(Rect selectionBounds, ResizePoint resizePoint, Vector delta)
    {
        foreach (var shape in _shapes)
        {
            shape.Scale(selectionBounds, resizePoint, delta, _minWidth, _minHeight);
        }
    }

    public void ScaleCompleted()
    {
        _shapes.Clear();
        _minWidth = 0;
        _minHeight = 0;
    }
}