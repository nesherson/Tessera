using System.Threading.Tasks;

namespace Tessera.App.Messages;

public class ShowConfirmDialogMessage(string title, string message)
{
    public TaskCompletionSource<bool> Tcs { get; } = new();
    public string Title { get; } = title;
    public string Message { get; } = message;
}