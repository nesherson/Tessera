using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Data;

namespace Tessera.App.ViewModels;

public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    private ApplicationPageNames _pageName;
}