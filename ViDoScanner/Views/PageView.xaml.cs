using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ViDoScanner.Views
{
  /// <summary>
  /// Interaction logic for PageView.xaml
  /// </summary>
  public partial class PageView : UserControl
  {
    private static readonly DependencyProperty CreationVisibilityProperty =
        DependencyProperty.Register("CreationVisibility", typeof(Visibility), typeof(PageView), new PropertyMetadata(Visibility.Collapsed));
    private static readonly DependencyProperty Point1Property =
        DependencyProperty.Register("Point1", typeof(Point), typeof(PageView), new PropertyMetadata(null));
    private static readonly DependencyProperty Point2Property =
        DependencyProperty.Register("Point2", typeof(Point), typeof(PageView), new PropertyMetadata(null));

    private Point Point1
    {
      get { return (Point)GetValue(Point1Property); }
      set { SetValue(Point1Property, value); }
    }
    private Point Point2
    {
      get { return (Point)GetValue(Point2Property); }
      set { SetValue(Point2Property, value); }
    }
    private Visibility CreationVisibility
    {
      get { return (Visibility)GetValue(CreationVisibilityProperty); }
      set { SetValue(CreationVisibilityProperty, value); }
    }

    public PageView()
    {
      InitializeComponent();
      Point1 = new Point(50, 50);
    }

    private void ScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      CreationVisibility = Visibility.Visible;
      Point1 = Mouse.GetPosition(sender as IInputElement);
    }

    private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (CreationVisibility == Visibility.Visible)
      {
        Point2 = Mouse.GetPosition(sender as IInputElement);
      }
    }

    private void ScrollViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      CreationVisibility = Visibility.Collapsed;
    }
  }
}
