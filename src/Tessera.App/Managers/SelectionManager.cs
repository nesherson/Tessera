using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Avalonia.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public partial class SelectionManager : ObservableObject
{
    private readonly ObservableCollection<ShapeBase> _shapes;
    private readonly HashSet<ShapeBase> _selectedShapes = [];

    public SelectionManager(ObservableCollection<ShapeBase> shapes)
    {
        _shapes = shapes;
        _shapes.CollectionChanged += OnShapesCollectionChanged;
    }

    public IReadOnlyCollection<ShapeBase> SelectedShapes => _selectedShapes;

    [ObservableProperty]
    private Rect _selectionBounds;
    
    [ObservableProperty]
    private bool _hasSelection;

    public bool IsSelected(ShapeBase shape) => SelectedShapes.Contains(shape);

    public event EventHandler? SelectionChanged;
    
    public void Select(ShapeBase shape)
    {
        _selectedShapes.Add(shape);
        
        shape.PropertyChanged += OnShapePropertyChanged;
        HasSelection = _selectedShapes.Count > 0;
        
        OnSelectionChanged();
        UpdateBounds();
    }

    public void SelectRange(IEnumerable<ShapeBase> shapes)
    {
        foreach (var shape in shapes)
        {
            _selectedShapes.Add(shape);
            
            shape.PropertyChanged += OnShapePropertyChanged;
        }
        
        HasSelection = _selectedShapes.Count > 0;
        
        OnSelectionChanged();   
        UpdateBounds();
    }
    
    public void Deselect(ShapeBase shape)
    {
        shape.PropertyChanged -= OnShapePropertyChanged;

        _selectedShapes.Remove(shape);
        
        HasSelection = _selectedShapes.Count > 0;
        
        OnSelectionChanged();
        UpdateBounds();
    }
    
    public void Clear()
    {
        foreach (var shape in _selectedShapes)
            shape.PropertyChanged -= OnShapePropertyChanged;
        
        _selectedShapes.Clear();
        
        HasSelection = _selectedShapes.Count > 0;
        
        OnSelectionChanged();
        UpdateBounds();
    }
    
    public void Toggle(ShapeBase shape)
    {
        if (!_selectedShapes.Remove(shape))
        {
            _selectedShapes.Add(shape);
        
            shape.PropertyChanged += OnShapePropertyChanged;
        }
        else
        {
            shape.PropertyChanged -= OnShapePropertyChanged;
        }
        
        HasSelection = _selectedShapes.Count > 0;
        
        OnSelectionChanged();
        UpdateBounds();
    }
    
    public void MoveSelection(Vector delta)
    {
        SelectionBounds = new Rect(SelectionBounds.X + delta.X, 
            SelectionBounds.Y + delta.Y,
            SelectionBounds.Width,
            SelectionBounds.Height);
    }

    private void UpdateBounds()
    {
        if (!HasSelection)
        {
            SelectionBounds = new Rect(0, 0, 0, 0);
            
            return;
        }
        
        var rects = SelectedShapes.Select(s => s.GetBounds())
            .ToList();
        var union = rects.First();
        
        foreach (var r in rects.Skip(1))
            union = union.Union(r);

        SelectionBounds = union;
    }
    
    private void OnShapesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e is { Action: NotifyCollectionChangedAction.Remove, OldItems: not null })
        {
            foreach (ShapeBase shape in e.OldItems)
            {
                shape.PropertyChanged -= OnShapePropertyChanged;
                _selectedShapes.Remove(shape);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            foreach (var shape in _selectedShapes)
                shape.PropertyChanged -= OnShapePropertyChanged;
            
            _selectedShapes.Clear();
        }
        
        HasSelection = _selectedShapes.Count > 0;
        
        UpdateBounds();
    }
    
    private void OnShapePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is TextShape && e.PropertyName == nameof(TextShape.StrokeThickness))
        {
            UpdateBounds();
        }
    }
    
    private void OnSelectionChanged() => SelectionChanged?.Invoke(this, EventArgs.Empty);
}