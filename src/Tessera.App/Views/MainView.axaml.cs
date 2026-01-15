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

        var applicationPageName = (ApplicationPageNames)navItem.Tag!;
        
        ViewModel.GoToPage(applicationPageName);

    }

    private void ThemeButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Application.Current is null || Application.Current.RequestedThemeVariant is null)
        {
            return;
        }

        var appTheme = Application.Current.RequestedThemeVariant.ToString();

        if (ThemeVariant.Dark.Key.ToString() == appTheme)
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
            ThemeSymbol.Symbol = Symbol.WeatherSunny;
        }
        else if (ThemeVariant.Light.Key.ToString() == appTheme)
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
            ThemeSymbol.Symbol = Symbol.WeatherMoon;
        }
    }

    private void ExitButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}