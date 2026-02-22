using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class TextShapeToolSettings : ToolSettingsBase
{
    private readonly string[] _fonts = ["Arial", "Comic Sans MS", "Cascadia Mono", "Sans Serif Collection"];
    
    [ObservableProperty]
    private TextAlignment _textAlignment = TextAlignment.Left;
    
    [ObservableProperty] 
    private List<FontType> _availableFontFamilies = [];

    [ObservableProperty]
    private FontType _fontFamily;
    
    public TextShapeToolSettings()
    {
        AvailableFontFamilies = FontManager.Current.SystemFonts
            .Where(sf => _fonts.Contains(sf.Name))
            .Select(f => new FontType {  Name = f.Name, Description = f.Name })
            .OrderBy(f => f.Name)
            .ToList();
        FontFamily = AvailableFontFamilies.First();
    }
}