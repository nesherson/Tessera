using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Constants;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class MultiShapePropertyProxy : ObservableObject, IShapeProperties
{
    public MultiShapePropertyProxy(List<ShapeBase> shapes)
    {
        Shapes = shapes;
    }

    public List<ShapeBase> Shapes { get; }

    public IBrush StrokeColor
    {
        get => Shapes.FirstOrDefault()?.StrokeColor ?? Brushes.Black;
        set
        {
            foreach (var shape in Shapes)
            {
                shape.StrokeColor = value;
                
                if (shape is TextShape)
                    continue;
                
                switch (FillType)
                {
                    case FillType.None:
                        shape.Color = Brushes.Transparent;

                        break;
                    case FillType.Semi:
                        if (value is SolidColorBrush scb)
                            shape.Color = new SolidColorBrush(scb.Color, AppConstants.SemiFillOpacity);

                        break;
                    case FillType.Solid:
                        shape.Color = value;

                        break;
                }
            }
            
            OnPropertyChanged();
        }
    }

    public IBrush Color
    {
        get => Shapes.FirstOrDefault()?.Color ?? Brushes.Black;
        set
        {
            foreach (var shape in Shapes)
                shape.Color = value;
            OnPropertyChanged();
        }
    }

    public double StrokeThickness
    {
        get => Shapes.FirstOrDefault()?.StrokeThickness ?? 1;
        set
        {
            foreach (var shape in Shapes)
            {
                shape.StrokeThickness = value;

                if (shape is TextShape textShape)
                {
                    textShape.FontSize = textShape.StrokeThickness * 2;
                    textShape.UpdateBounds();
                }
                else if (shape is PointShape pointShape)
                {
                    pointShape.Width = pointShape.StrokeThickness;
                    pointShape.Height = pointShape.StrokeThickness;
                }
            }
            OnPropertyChanged();
        }
    }

    public double Opacity
    {
        get => Shapes.FirstOrDefault()?.Opacity ?? 1;
        set
        {
            foreach (var shape in Shapes)
                shape.Opacity = value;
            OnPropertyChanged();
        }
    }

    public AvaloniaList<double> StrokeDashArray =>
        Shapes.FirstOrDefault()?.StrokeDashArray ?? [0, 0];

    public StrokeType StrokeType
    {
        get => Shapes.FirstOrDefault()?.StrokeType ?? StrokeType.Solid;
        set
        {
            foreach (var shape in Shapes)
                shape.StrokeType = value;
            OnPropertyChanged();
        }
    }

    public FillType FillType
    {
        get => Shapes.FirstOrDefault()?.FillType ?? new FillType();
        set
        {
            foreach (var shape in Shapes)
            {
                shape.FillType = value;

                if (shape is TextShape)
                    continue;
                
                switch (FillType)
                {
                    case FillType.None:
                        shape.Color = Brushes.Transparent;

                        break;
                    case FillType.Semi:
                        if (shape.StrokeColor is SolidColorBrush scb)
                            shape.Color = new SolidColorBrush(scb.Color, AppConstants.SemiFillOpacity);
                        
                        break;
                    case FillType.Solid:
                        shape.Color = shape.StrokeColor;

                        break;
                }
            }
            OnPropertyChanged();
        }
    }

    public ShapeType ShapeType { get; set; }
}