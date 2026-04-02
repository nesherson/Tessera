using System.Collections.Generic;
using Tessera.App.Enumerations;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public class TransformManager
{
    public void Scale(IList<ShapeBase> shapes, ResizePoint resizePoint, Vector delta)
    {
        foreach (var shape in shapes)
        {
            shape.Scale(resizePoint, delta);
        }
    }
}