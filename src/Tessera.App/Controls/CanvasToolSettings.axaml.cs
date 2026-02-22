using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Models;

namespace Tessera.App.Controls;

public partial class CanvasToolSettings : UserControl
{
    public static readonly StyledProperty<ObservableObject> ToolSettingsProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, ObservableObject>(nameof(ToolSettings));

    public ObservableObject ToolSettings
    {
        get => GetValue(ToolSettingsProperty);
        set => SetValue(ToolSettingsProperty, value);
    }
    
    public CanvasToolSettings()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            ToolSettings = new TextShapeToolSettings();
        }
    }
}