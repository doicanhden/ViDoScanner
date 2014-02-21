using System.Windows;
namespace ViDoScanner
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Application_Exit(object sender, ExitEventArgs e)
    {
      ViDoScanner.Properties.Settings.Default.Save();
    }
  }
}
