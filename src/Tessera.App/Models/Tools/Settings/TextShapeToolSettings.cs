using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class TextShapeToolSettings : ObservableObject
{
    private readonly string[] _fonts = ["Arial", "Comic Sans MS", "Cascadia Mono", "Sans Serif Collection"];

    [ObservableProperty]
    private double _fontSize = 14;

    [ObservableProperty] 
    private IBrush _color = Brushes.Black;
    
    [ObservableProperty]
    private TextAlignment _textAlignment = TextAlignment.Left;
    
    [ObservableProperty] 
    private ObservableCollection<string> _availableFontFamilies = [];

    [ObservableProperty] 
    private string _selectedFontFamily = "Sans Serif Collection";
    
    public TextShapeToolSettings()
    {
        var fontNames = FontManager.Current.SystemFonts
            .Where(sf => _fonts.Contains(sf.Name))
            .Select(f => f.Name)
            .OrderBy(name => name);

        AvailableFontFamilies = new ObservableCollection<string>(fontNames);
    }
}