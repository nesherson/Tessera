using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class DialogViewModelBase : ViewModelBase
{
    [ObservableProperty]
    private bool _isOpen;
    [ObservableProperty]
    private double _dialogWidth = double.NaN;

    protected TaskCompletionSource CloseTask = new();
    
    public async Task WaitAsync()
    {
        await CloseTask.Task;
    }

    public void Show()
    {
        if (CloseTask.Task.IsCompleted)
            CloseTask = new TaskCompletionSource();
        
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
        
        CloseTask.TrySetResult();
    }
    
    // public void ShowWindowNotifcation(string title, string message, NotificationType type, Action? onClick = null)
    // {
    //     WeakReferenceMessenger.Default
    //         .Send(new ShowWindowNotificationMessage(
    //             new Notification(title, message, type, null, onClick)));
    // }
}