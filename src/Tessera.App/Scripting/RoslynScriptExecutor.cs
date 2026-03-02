using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Tessera.App.Interfaces;
using Tessera.App.Models;
using Tessera.App.ViewModels;

namespace Tessera.App.Scripting;

public class RoslynScriptExecutor
{
    private readonly ScriptingApi _api;
    private string _resultText;

    // public RoslynScriptExecutor(ICanvasContext context)
    // {
    //     _api = new ScriptingApi(context);
    // }
    //
    public RoslynScriptExecutor(TestDocument testDocument)
    {
        _api = new ScriptingApi(testDocument);
    }

    public async Task ExecuteAsync(string code)
    {
        var options = ScriptOptions.Default
            .AddReferences(typeof(ScriptingApi).Assembly)
            .AddImports("System", "System.Math");

        await CSharpScript.RunAsync(code, options, globals: _api);
    }
}

// This is the API surface exposed to scripts
public class ScriptingApi
{
    private readonly ICanvasContext _context;
    private IBrush _stroke = Brushes.Black;
    private IBrush _fill;
    private double _thickness = 1;

    private TestDocument _testDocument; 

    // public ScriptingApi(ICanvasContext context)
    // {
    //     _context = context;
    // }

    public ScriptingApi(TestDocument testDocument)
    {
        _testDocument = testDocument;
    }

    public void setText(string text)
    {
        _testDocument.ResultText = text;
    }

    public void drawCircle(double x, double y, double radius)
    {
        _context.Shapes.Add(new EllipseShape
        {
            X = x - radius, Y = y - radius,
            Width = radius * 2, Height = radius * 2,
            StrokeColor = _stroke, Color = _fill,
            StrokeThickness = _thickness
        });
    }

    public void drawLine(double x1, double y1, double x2, double y2)
    {
        _context.Shapes.Add(new LineShape
        {
            StartPoint = new Point(x1, y1),
            EndPoint = new Point(x2, y2),
            StrokeColor = _stroke,
            StrokeThickness = _thickness
        });
    }

    public void drawRectangle(double x, double y, double w, double h)
    {
        _context.Shapes.Add(new RectangleShape
        {
            X = x, Y = y, Width = w, Height = h,
            StrokeColor = _stroke, Color = _fill,
            StrokeThickness = _thickness
        });
    }

    public void setColor(string color) =>
        _stroke = new SolidColorBrush(Color.Parse(color));

    public void fill(string color) =>
        _fill = new SolidColorBrush(Color.Parse(color));

    public void setStrokeThickness(double t) => _thickness = t;

    public void clear() => _context.Shapes.Clear();
}


public class ScriptException : Exception
{
    public ScriptException(string message) : base(message) { }
}