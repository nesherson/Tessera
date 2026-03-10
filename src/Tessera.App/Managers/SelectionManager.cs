using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Models;

namespace Tessera.App.Managers;

public partial class SelectionManager : ObservableObject
{
    private readonly HashSet<ShapeBase> _selectedShapes = [];
    
    public IReadOnlyCollection<ShapeBase> SelectedShapes => _selectedShapes;

    [ObservableProperty]
    private Rect _selectionBounds;
    
    [ObservableProperty]
    private bool _hasSelection;

    public bool IsSelected(ShapeBase shape) => _selectedShapes.Contains(shape);

    public void Select(ShapeBase shape)
    {
        _selectedShapes.Add(shape);
        UpdateBounds();
    }
    
    public void Deselect(ShapeBase shape)
    {
        _selectedShapes.Remove(shape);
        UpdateBounds();
    }
    
    public void Clear()
    {
        _selectedShapes.Clear();
        UpdateBounds();
    }
    
    public void Toggle(ShapeBase shape)
    {
        if (!_selectedShapes.Remove(shape))
            _selectedShapes.Add(shape);
        
        UpdateBounds();
    }

    private void UpdateBounds()
    {
        HasSelection = _selectedShapes.Count > 0;

        if (!HasSelection) return;
        
        var rects = _selectedShapes.Select(s => s.GetBounds())
            .ToList();
        var union = rects.First();
        
        foreach (var r in rects.Skip(1))
            union = union.Union(r);

        SelectionBounds = union;
    }
}