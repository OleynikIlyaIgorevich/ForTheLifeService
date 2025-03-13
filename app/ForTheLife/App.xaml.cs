using ForTheLife.Views;
using System.Windows;

namespace ForTheLife;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        var startUpWindow = new CatalogWindow();
        startUpWindow.Show();
    }

}


