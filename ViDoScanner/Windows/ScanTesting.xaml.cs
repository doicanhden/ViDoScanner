using Com.StellmanGreene.CSVReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViDoScanner.Processing;
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

  }
}
