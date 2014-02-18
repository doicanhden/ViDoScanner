using LumenWorks.Framework.IO.Csv;
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
using System.Windows.Shapes;
using ViDoScanner.Processing.Scanner;
using ViDoScanner.ViewModels;

namespace ViDoScanner.Windows
{
  /// <summary>
  /// Interaction logic for ScanTesting.xaml
  /// </summary>
  public partial class ScanTesting : Window
  {
    public ConfigViewModel Config
    {
      get
      {
        return (ConfigViewModel)(this.DataContext ?? (
          this.DataContext = new ConfigViewModel()));
      }
      set
      {
        this.DataContext = value;
      }
    }

    public string ImagePath { get; set; }

    public ScanTesting()
    {
      InitializeComponent();
    }

    private void Button0_Click(object sender, RoutedEventArgs e)
    {
      Scanner scanner = new Scanner();
      scanner.Config = Config.Model;
      scanner.LoadTemplate(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner\Template.xml");

      scanner.SinglePage(scanner.Template.Pages[0], new string[] { ImagePath }, null);
    }

    private void Button1_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
