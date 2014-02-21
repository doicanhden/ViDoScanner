using Com.StellmanGreene.CSVReader;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace ViDoScanner.Windows
{
  /// <summary>
  /// Interaction logic for ViewResults.xaml
  /// </summary>
  public partial class ViewResults : Window
  {
    private string fileName;
    public string FileName
    {
      get { return (fileName); }
      set
      {
        if (File.Exists(value))
        {
          fileName = value;
          UpdateDataGrid(value);
        }
      }
    }

    private void UpdateDataGrid(string value)
    {
      var csv = new CSVReader(new StringReader(File.ReadAllText(value)));
      this.ResultsDataGrid.ItemsSource = csv.CreateDataTable(true).DefaultView;
      this.ResultsDataGrid.AutoGenerateColumns = true;
    }
 
    public ViewResults()
    {
      InitializeComponent();
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
  }
}
