using Com.StellmanGreene.CSVReader;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ViDoScanner.Utilities;
using ViDoScanner.ViewModels;
namespace ViDoScanner.Windows
{
  /// <summary>
  /// Interaction logic for ViewResults.xaml
  /// </summary>
  public partial class ViewResults : Window
  {
    private ViewResultsViewModel model;
    public ViewResults(ViewResultsViewModel model)
    {
      InitializeComponent();
      this.model = model;
      this.DataContext = model;
    }

    private void ResultsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (sender != null)
      {
        DataGrid grid = sender as DataGrid;

        if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
        {
          DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
          
        }
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var dir = Browsers.ShowBrowserFolder("Chọn thư mục chứ kết quả", false);
      if (!string.IsNullOrWhiteSpace(dir))
      {
        model.OutputDirectoryName = dir;
      }
    }
  }
}
