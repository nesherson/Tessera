using Tessera.App.Data;
using Tessera.App.ViewModels;

namespace Tessera.App.Factories;

public class DesignerPageFactory : IPageFactory
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
    {
        return new DummyPageViewModel();
    }
}