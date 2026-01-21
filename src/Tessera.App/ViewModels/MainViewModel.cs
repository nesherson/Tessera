using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Data;
using Tessera.App.Factories;

namespace Tessera.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
     private readonly IPageFactory _pageFactory;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DrawingPageIsActive))]
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
    public bool DrawingPageIsActive => CurrentPage.PageName == ApplicationPageNames.Drawing;
    
    [RelayCommand]
    public void GoToPage(ApplicationPageNames applicationPage)
    {
        CurrentPage = _pageFactory.GetPageViewModel(applicationPage);
    }
    
    [RelayCommand]
    private void ToggleSideMenu()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
    [RelayCommand]
    private void ExitApplication()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}