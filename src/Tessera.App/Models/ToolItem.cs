using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class ToolItem
{
    public required string Name { get; set; }
    public string? IconPath { get; set; } 
    public required ICanvasTool Tool { get; set; }
    public ObservableObject? ToolSettings { get; set; }
}