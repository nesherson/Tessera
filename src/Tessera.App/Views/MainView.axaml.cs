using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using Tessera.App.Data;
using Tessera.App.ViewModels;

namespace Tessera.App.Views;

public partial class MainView : Window
{
    private MainViewModel ViewModel => DataContext as MainViewModel;
    
    public MainView()
    {
        InitializeComponent();
    }

    private void NavigationViewItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not NavigationViewItem navItem)
            return;

        ViewModel.GoToPage((ApplicationPageNames)navItem.Tag!);
    }

    private void ExitButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}