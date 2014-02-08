using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViDoScanner.Commands;
using ViDoScanner.ViewModels;

namespace ViDoScanner
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    TemplateViewModel template = new TemplateViewModel();
    PageViewModel page;
    public MainWindow()
    {
      InitializeComponent();
      this.TemplateView.DataContext = template;

      template.CreatePage.Execute("F:\\Khanh\\SkyDrive\\Development\\Github\\ViDoScanner\\Template\\demo.xml.0.jpg");

      page = template.SelectedPage;
      page.Name = "Page1";
      page.Width = 1653;
      page.Height = 2338;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      page.CreateField.Execute(r);

      page.SelectedField.Name = "Field";
      page.SelectedField.NumberOfCols = 6;
      page.SelectedField.NumberOfRows = 10;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      page.CreateField.Execute(r);

      page.SelectedField.Name = "Field";
      page.SelectedField.NumberOfCols = 0;
      page.SelectedField.NumberOfRows = 0;
    }
  }
}
