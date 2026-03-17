using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class MultiShapePropertyProxy : ObservableObject, IShapeProperties
{
    private readonly List<ShapeBase> _shapes;

    public MultiShapePropertyProxy(List<ShapeBase> shapes)
    {
        _shapes = shapes;
    }

    public IBrush StrokeColor
    {
        get => _shapes.FirstOrDefault()?.StrokeColor ?? Brushes.Black;
        set
        {
            foreach (var shape in _shapes)
                shape.StrokeColor = value;
            OnPropertyChanged();
        }
    }

    public double StrokeThickness
    {
        get => _shapes.FirstOrDefault()?.StrokeThickness ?? 1;
        set
        {
            foreach (var shape in _shapes)
                shape.StrokeThickness = value;
            OnPropertyChanged();
        }
    }

    public double Opacity
    {
        get => _shapes.FirstOrDefault()?.Opacity ?? 1;
        set
        {
            foreach (var shape in _shapes)
                shape.Opacity = value;
            OnPropertyChanged();
        }
    }
}