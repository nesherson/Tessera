using Microsoft.Extensions.DependencyInjection;
using Tessera.App.Data;
using Tessera.App.Factories;
using Tessera.App.ViewModels;

namespace Tessera.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IPageFactory, PageFactory>();
        
        services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(
            sp => pageName => pageName switch
            {
                ApplicationPageNames.Drawing => sp.GetRequiredService<DrawingPageViewModel>(),
                _ => sp.GetRequiredService<DrawingPageViewModel>()
            });
    }
    
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<DrawingPageViewModel>();
    }
}