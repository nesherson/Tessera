using System.Collections.Generic;
using System.Diagnostics;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public class TransformManager
{
    public void Scale(IList<ShapeBase> shapes, Vector delta)
    {
        foreach (var shape in shapes)
        {
            shape.Width += delta.X;
            shape.Height += delta.Y;
        }
    }
}