using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Tessera.App.Extensions;
using Tessera.App.ViewModels;
using Tessera.App.Views;

namespace Tessera.App;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        
        services.AddCommonServices();
        services.AddViewModels();
            
        _serviceProvider = services.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = ActivatorUtilities.CreateInstance<MainView>(_serviceProvider);
            desktop.MainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}