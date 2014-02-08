using System;
using System.Collections;
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

namespace ViDoScanner.Views
{
  /// <summary>
  /// Interaction logic for FormattedTab.xaml
  /// </summary>
  public partial class FormattedTab : UserControl
  {
    // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
      "ItemSource", typeof(IEnumerable), typeof(FormattedTab), new FrameworkPropertyMetadata(ItemSourceChangedCallback));

    public FormattedTab()
    {
      InitializeComponent();
    }

    public IEnumerable ItemSource
    {
      get { return this.TabControl.ItemsSource; }
      set
      {
        this.TabControl.ItemsSource = value;
        this.TabControl.SelectedIndex = 0;
      }
    }

    public void AddResource(object key, object value)
    {
      this.Resources.Add(key, value);
    }

    private static void ItemSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      FormattedTab tab = d as FormattedTab;
      if (tab != null)
      {
        IEnumerable value = (IEnumerable)e.NewValue;
        tab.ItemSource = value;
      }
    }

  }
}
