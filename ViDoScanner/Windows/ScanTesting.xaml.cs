using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViDoScanner.Utilities;
using ViDoScanner.ViewModels;
namespace ViDoScanner.Windows
{
  /// <summary>
  /// Interaction logic for ScanTesting.xaml
  /// </summary>
  public partial class ScanTesting : Window
  {
    private ScanTestingViewModel model;

    public ScanTesting(ScanTestingViewModel model)
    {
      InitializeComponent();
      this.model = model;
      this.DataContext = model;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var imagePath = Browsers.ShowOpenFile("Chọn ảnh quét", "Pictures (*.jpg)|*.jpg");
      if (!string.IsNullOrWhiteSpace(imagePath))
      {
        this.model.ImagePath = imagePath;
      }
    }

    private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (sender != null)
      {
        DataGrid grid = sender as DataGrid;

        if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
        {
          var row = grid.SelectedItem as DataRowView;
          

          DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
          
          
        }
      }
    }

  }
}
