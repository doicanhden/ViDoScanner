using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
namespace ViDoScanner.Controls
{
  /// <summary>
  /// Interaction logic for FileBrowserControl.xaml
  /// </summary>
  public partial class FileBrowserControl : UserControl
  {
    public FileBrowserControl()
    {
      InitializeComponent();
    }

    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(FileBrowserControl), new PropertyMetadata("Open file"));

    public string Filter
    {
      get { return (string)GetValue(FilterProperty); }
      set { SetValue(FilterProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register("Filter", typeof(string), typeof(FileBrowserControl), new PropertyMetadata("All Files (.*)|*.*"));

    public string FileName
    {
      get { return (string)GetValue(FileNameProperty); }
      set { SetValue(FileNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FileNameProperty =
        DependencyProperty.Register("FileName", typeof(string), typeof(FileBrowserControl), new PropertyMetadata(""));

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDlg = new OpenFileDialog();
      openFileDlg.Title = Title;
      openFileDlg.Filter = Filter;

      if (openFileDlg.ShowDialog() == true)
        this.FileName = openFileDlg.FileName;
    }
  }
}
