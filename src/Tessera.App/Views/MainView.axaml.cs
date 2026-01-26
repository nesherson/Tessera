using Avalonia;
using Avalonia.Controls;

namespace Tessera.App.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        
        this.AttachDevTools();
    }
}