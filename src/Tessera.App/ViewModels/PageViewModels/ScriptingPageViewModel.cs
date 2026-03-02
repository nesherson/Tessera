using System.Threading.Tasks;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Scripting;

namespace Tessera.App.ViewModels;

public partial class ScriptingPageViewModel : PageViewModel
{
    [ObservableProperty]
    private string _scriptText = "";

    [ObservableProperty] 
    private string _scriptError = "";
    
    [ObservableProperty]
    private TestDocument _testDocument = new();

    [ObservableProperty]
    private string _code = "";

    [RelayCommand]
    private async Task RunScript()
    {
        try
        {
            ScriptError = "";
            var executor = new RoslynScriptExecutor(TestDocument);
            
            await executor.ExecuteAsync(Code);
        }
        catch (ScriptException ex)
        {
            ScriptError = ex.Message;
        }
    }
}

public partial class TestDocument : ObservableObject
{
    [ObservableProperty] 
    private string _resultText = "";
}