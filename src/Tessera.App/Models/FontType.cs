namespace Tessera.App.Models;

public record struct FontType
{
    public string Name { get; set; }
    public string Description { get; set; }
    public FontFamily FontFamily => new(Name);
}