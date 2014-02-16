using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Xml.Serialization;
using ViDoScanner.Commands;
using ViDoScanner.Processing.Scanner;
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
    public MainWindow()
    {
      InitializeComponent();
      this.TemplateView.DataContext = template;
    }

    private void MenuItemOpenTemplate_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.FileName = "Document"; // Default file name
      dlg.DefaultExt = ".xml"; // Default file extension
      dlg.Filter = "Template (.xml)|*.xml"; // Filter files by extension 
      
      // Show open file dialog box
      Nullable<bool> result = dlg.ShowDialog();
      
      // Process open file dialog box results 
      if (result == true)
      {
        template.LoadTemplate.Execute(dlg.FileName);
      }
    }

    private void MenuItemSaveTemplate_Click(object sender, RoutedEventArgs e)
    {
      template.SaveTemplate.Execute(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner");
    }

    private void MenuItemTestScanner_Click(object sender, RoutedEventArgs e)
    {
      Scanner scanner = new Scanner();
      scanner.LoadTemplate(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner\Template.xml");
      scanner.Scan(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner\hoacd.xml.0.jpg");
      scanner.SaveLog(@"F:\Khanh\SkyDrive\Development\Github\ViDoScanner\Template.log.xml");
    }
  }
}
