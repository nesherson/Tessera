using System.Collections.Generic;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Data;
using Tessera.App.Factories;

namespace Tessera.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
     private readonly IPageFactory _pageFactory;

    [ObservableProperty]
    private PageViewModel _currentPage;
    
    [ObservableProperty]
    private bool _isPaneOpen;
    
    public MainViewModel()
    {
        if (!Design.IsDesignMode) 
            return;
        
        _pageFactory = new DesignerPageFactory();
        CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Dummy);
        Title = "Tessera";
        
    }
    
    public MainViewModel(IPageFactory pageFactory)
    {
        _pageFactory = pageFactory;
        CurrentPage = pageFactory.GetPageViewModel(ApplicationPageNames.Drawing);
        Title = "Tessera";
    }

    public string Title { get; set; } 
    
    public void GoToPage(ApplicationPageNames pageName)
    {
        CurrentPage = _pageFactory.GetPageViewModel(pageName);
    }
}