using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class DialogViewModelBase : ViewModelBase
{
    [ObservableProperty]
    private bool _isOpen;
    [ObservableProperty]
    private double _dialogWidth = double.NaN;
    
    public void Show()
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }
}