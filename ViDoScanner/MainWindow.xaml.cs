namespace ViDoScanner
{
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      this.DataContext = new MainWindowViewModel(this);
    }
  }
}
