namespace ViDoScanner.Views
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using ViDoScanner.ViewModels;

  /// <summary>
  /// Interaction logic for PageView.xaml
  /// </summary>
  public partial class PageView : UserControl
  {
    #region Public Properties
    private Point Point1
    {
      get { return (Point)GetValue(Point1Property); }
      set { SetValue(Point1Property, value); }
    }
    private static readonly DependencyProperty Point1Property =
        DependencyProperty.Register("Point1", typeof(Point), typeof(PageView), new PropertyMetadata(null));

    private Point Point2
    {
      get { return (Point)GetValue(Point2Property); }
      set { SetValue(Point2Property, value); }
    }
    private static readonly DependencyProperty Point2Property =
        DependencyProperty.Register("Point2", typeof(Point), typeof(PageView), new PropertyMetadata(null));

    private Visibility CreationVisibility
    {
      get { return (Visibility)GetValue(CreationVisibilityProperty); }
      set { SetValue(CreationVisibilityProperty, value); }
    }
    private static readonly DependencyProperty CreationVisibilityProperty =
        DependencyProperty.Register("CreationVisibility", typeof(Visibility), typeof(PageView), new PropertyMetadata(Visibility.Collapsed));

    public bool IsInCreationMode
    {
      get { return (bool)GetValue(IsInCreationModeProperty); }
      set { SetValue(IsInCreationModeProperty, value); }
    }
    // Using a DependencyProperty as the backing store for CreationMode.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsInCreationModeProperty =
        DependencyProperty.Register("IsInCreationMode", typeof(bool), typeof(PageView), new PropertyMetadata(false));
    #endregion

    #region Constructors
    public PageView()
    {
      InitializeComponent();
    }
    #endregion

    #region Event Handlers
    private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (IsInCreationMode)
      {
        CreationVisibility = Visibility.Visible;
        Point1 = Mouse.GetPosition(sender as IInputElement);
      }
    }
    private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (CreationVisibility == Visibility.Visible)
      {
        Point2 = Mouse.GetPosition(sender as IInputElement);
      }
    }
    private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      if (IsInCreationMode && CreationVisibility == Visibility.Visible)
      {
        CreationVisibility = Visibility.Collapsed;

        Rect r = new Rect(Point1, Point2);
        r.X = (int)r.X;
        r.Y = (int)r.Y;
        r.Width = (int)r.Width;
        r.Height = (int)r.Height;

        PageViewModel page = this.DataContext as PageViewModel;
        if (page.CreateField.CanExecute(r))
        {
          page.CreateField.Execute(r);
          IsInCreationMode = false;
        }
      }
    }
    #endregion
  }
}
