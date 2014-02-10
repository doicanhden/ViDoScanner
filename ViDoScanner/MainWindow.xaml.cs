﻿using System;
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

      template.CreatePage.Execute("F:\\Khanh\\SkyDrive\\Development\\Github\\ViDoScanner\\Template\\demo.xml.0.jpg");
      page = template.SelectedPage;
      page.Name = "Page1";
      page.Width = 1653;
      page.Height = 2338;
      
      template.CreatePage.Execute("F:\\Khanh\\SkyDrive\\Development\\Github\\ViDoScanner\\Template\\demo3.xml.0.jpg");
      page = template.SelectedPage;
      page.Name = "Trang 2";
      page.Width = 1653;
      page.Height = 2338;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      template.SelectedPage.CreateField.Execute(r);

      template.SelectedPage.SelectedField.NumberOfCols = 6;
      template.SelectedPage.SelectedField.NumberOfRows = 10;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      var r = new Rect(10, 10, 150, 150);
      template.SelectedPage.CreateField.Execute(r);
    }
  }
}
