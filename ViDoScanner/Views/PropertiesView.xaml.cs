namespace ViDoScanner.Views
{
  using System.IO;
  using System.Windows.Controls;
  using ViDoScanner.Utilities;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for SelectedView.xaml
  /// </summary>
  public partial class PropertiesView : UserControl
  {
    public PropertiesView()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (this.DataContext != null)
      {
        var page = this.DataContext as PageViewModel;
        if (page != null)
        {
          var path = Browsers.ShowOpenFile("Chọn ảnh", "Pictures (*.jpg)|*.jpg");
          if (File.Exists(path))
          {
            page.ImagePath = path;
          }
        }
      }
    }
  }
}
