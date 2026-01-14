using System;
using Tessera.App.Data;
using Tessera.App.ViewModels;

namespace Tessera.App.Factories;

public class PageFactory : IPageFactory
{
    private readonly Func<ApplicationPageNames, PageViewModel> _factory;
    
    public PageFactory(Func<ApplicationPageNames, PageViewModel> factory)
    {
        _factory = factory;
    }
    
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
    {
        return _factory(pageName);
    }
}