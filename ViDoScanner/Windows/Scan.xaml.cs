namespace ViDoScanner.Windows
{
  using System.Windows;
  using ViDoScanner.Utilities;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for Scan.xaml
  /// </summary>
  public partial class Scan : Window
  {
    private ScanViewModel scan;
    public Scan(ScanViewModel scan)
    {
      InitializeComponent();
      this.scan = scan;
      this.DataContext = scan;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var directoryName = Browsers.ShowBrowserFolder("Chọn thư mục chứa ảnh", false);
      if (!string.IsNullOrWhiteSpace(directoryName))
      {
        this.scan.ImagesDirectoryName = directoryName;
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      var directoryName = Browsers.ShowBrowserFolder("Chọn thư mục chứa kết quả");
      if (!string.IsNullOrWhiteSpace(directoryName))
      {
        this.scan.OutputDirectoryName = directoryName;
      }
    }
  }
}
