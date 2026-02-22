using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tessera.App.Data;
using Tessera.App.Factories;
using Tessera.App.Messages;

namespace Tessera.App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
     private readonly IPageFactory _pageFactory;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DrawingPageIsActive))]
    private PageViewModel _currentPage;
    
    [ObservableProperty]
    private bool _isPaneOpen;
    
    [ObservableProperty]
    private DialogViewModelBase? _dialog;
    
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
        
        WeakReferenceMessenger.Default.Register<ShowConfirmDialogMessage>(this, HandleShowConfirmDialogMessage);
        WeakReferenceMessenger.Default.Register<ShowCanvasSettingsDialogMessage>(this, HandleShowCanvasOptionsDialogMessage);
    }

    private void HandleShowConfirmDialogMessage(object r, ShowConfirmDialogMessage m)
    {
        Dialog = new ConfirmDialogViewModel
        {
            Title = m.Title,
            Message = m.Message,
            OnResult = result =>
            {
                m.Tcs.SetResult(result);
                Dialog?.Close();
                Dialog = null;
            }
        };
        
        Dialog.Show();
    }
    
    private void HandleShowCanvasOptionsDialogMessage(object r, ShowCanvasSettingsDialogMessage m)
    {
        Dialog = new CanvasSettingsViewModel
        {
            GridSpacing =  m.GridSpacing,
            SelectedGridType = m.GridType,
            SelectedGridColor = m.GridColor,
            OnResult = result =>
            {
                m.Tcs.SetResult(result);
                Dialog?.Close();
                Dialog = null;
            }
        };
        
        Dialog.Show();
    }

    public string Title { get; set; } 
    public bool DrawingPageIsActive => CurrentPage.PageName == ApplicationPageNames.Drawing;
    
    [RelayCommand]
    private void GoToPage(ApplicationPageNames applicationPage)
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