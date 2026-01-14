using Tessera.App.Data;
using Tessera.App.ViewModels;

namespace Tessera.App.Factories;

public interface IPageFactory
{
    PageViewModel GetPageViewModel(ApplicationPageNames pageName);
}