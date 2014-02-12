using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using ViDoScanner.Commands;
using ViDoScanner.ViewModels;
using ViDoScanner.Views;

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


    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      template.SelectedPage.CreateField.Execute(r);

      template.SelectedPage.SelectedField.NumberOfSelection = 6;
      template.SelectedPage.SelectedField.NumberOfRecords = 10;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      template.SelectedPage.CreateField.Execute(r);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      template.SaveTemplate.Execute(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner");
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      template.CreatePage.Execute("F:\\Khanh\\SkyDrive\\Development\\Github\\ViDoScanner\\Template\\demo.xml.0.jpg");
      page = template.SelectedPage;
      page.Name = "Page1";

      template.CreatePage.Execute("F:\\Khanh\\SkyDrive\\Development\\Github\\ViDoScanner\\Template\\demo3.xml.0.jpg");
      page = template.SelectedPage;
      page.Name = "Trang 2";
    }
  }
}
