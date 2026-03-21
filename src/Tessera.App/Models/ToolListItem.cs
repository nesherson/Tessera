namespace Tessera.App.Models;

public class ToolListItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public virtual object? TypeValue => null;
}

public class ToolListItem<T> : ToolListItem 
    where T : Enum
{
    public required T Type { get; set; }
    public override object TypeValue => Type;
}