namespace Tessera.App.Models;

public record struct ShapeSize
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Thickness { get; set; }
}