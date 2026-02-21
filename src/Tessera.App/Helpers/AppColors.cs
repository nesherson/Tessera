using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace Tessera.App.Helpers;

public static class AppColors
{
    // Colors
    public static Color Black => GetColor("Color.Black");
    public static Color Silver => GetColor("Color.Silver");
    public static Color LightPurple => GetColor("Color.LightPurple");
    public static Color Purple => GetColor("Color.Purple");
    public static Color IndigoBlue => GetColor("Color.IndigoBlue");
    public static Color SkyBlue    => GetColor("Color.SkyBlue");
    public static Color Orange     => GetColor("Color.Orange");
    public static Color Rust       => GetColor("Color.Rust");
    public static Color Green      => GetColor("Color.Green");
    public static Color Emerald    => GetColor("Color.Emerald");
    public static Color Red        => GetColor("Color.Red");
    public static Color Salmon     => GetColor("Color.Salmon");

    // Brushes
    public static IBrush BlackBrush => GetBrush("Brush.Black");
    public static IBrush SilverBrush => GetBrush("Brush.Silver");
    public static IBrush LightPurpleBrush => GetBrush("Brush.LightPurple");
    public static IBrush PurpleBrush => GetBrush("Brush.Purple");
    public static IBrush IndigoBlueBrush => GetBrush("Brush.IndigoBlue");
    public static IBrush SkyBlueBrush    => GetBrush("Brush.SkyBlue");
    public static IBrush OrangeBrush     => GetBrush("Brush.Orange");
    public static IBrush RustBrush       => GetBrush("Brush.Rust");
    public static IBrush GreenBrush      => GetBrush("Brush.Green");
    public static IBrush EmeraldBrush    => GetBrush("Brush.Emerald");
    public static IBrush RedBrush        => GetBrush("Brush.Red");
    public static IBrush SalmonBrush     => GetBrush("Brush.Salmon");

    public static IReadOnlyList<IBrush> DefaultPalette { get; } =
    [
        BlackBrush, SilverBrush, LightPurpleBrush, PurpleBrush,
        IndigoBlueBrush, SkyBlueBrush, OrangeBrush, RustBrush,
        GreenBrush,      EmeraldBrush, RedBrush,    SalmonBrush,
    ];

    private static Color GetColor(string key) =>
        Application.Current!.Resources.TryGetResource(key, null, out var res) && res is Color c
            ? c
            : throw new KeyNotFoundException($"Color resource '{key}' not found.");

    private static IBrush GetBrush(string key) =>
        Application.Current!.Resources.TryGetResource(key, null, out var res) && res is IBrush b
            ? b
            : throw new KeyNotFoundException($"Brush resource '{key}' not found.");
}