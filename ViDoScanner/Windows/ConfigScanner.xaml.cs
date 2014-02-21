namespace ViDoScanner.Windows
{
  using System.Windows;
  using System.Windows.Forms;
  using ViDoScanner.Core;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for ConfigScanner.xaml
  /// </summary>
  public partial class ConfigScanner : Window
  {
    public ConfigScanner(ConfigViewModel config)
    {
      InitializeComponent();
      this.DataContext = config;
    }
  }
}
