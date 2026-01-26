using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Data;

namespace Tessera.App.ViewModels;

public partial class DummyPageViewModel : PageViewModel
{
    [ObservableProperty]
    private string _welcomeText = "This is dummy text!";

    public DummyPageViewModel()
    {
        PageName = ApplicationPageNames.Dummy;
    }
}