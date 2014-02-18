namespace ViDoScanner.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  /// <summary>
  /// Interaction logic for EditableTextBlock.xaml
  /// </summary>
  public partial class EditableTextBlockControl : UserControl
  {
    #region Data Members
    private string oldText;
    #endregion

    #region Constructors
    public EditableTextBlockControl()
    {
      InitializeComponent();
      base.Focusable = true;
      base.FocusVisualStyle = null;
    }
    #endregion

    #region Properties
    public string Text
    {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }
    public static readonly DependencyProperty TextProperty =
      DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlockControl), new PropertyMetadata(""));

    public bool IsEditable
    {
      get { return (bool)GetValue(IsEditableProperty); }
      set { SetValue(IsEditableProperty, value); }
    }
    public static readonly DependencyProperty IsEditableProperty =
      DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableTextBlockControl), new PropertyMetadata(true));

    public bool IsInEditMode
    {
      get { return (IsEditable ? (bool)GetValue(IsInEditModeProperty) : false); }
      set
      {
        if (IsEditable)
        {
          if (value)
            oldText = Text;
          SetValue(IsInEditModeProperty, value);
        }
      }
    }
    public static readonly DependencyProperty IsInEditModeProperty =
      DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableTextBlockControl), new PropertyMetadata(false));
    #endregion

    #region Event Handlers
    // Invoked when we enter edit mode.
    void TextBox_Loaded(object sender, RoutedEventArgs e)
    {
      TextBox txt = sender as TextBox;

      // Give the TextBox input focus
      txt.Focus();
      txt.SelectAll();
    }

    // Invoked when we exit edit mode.
    void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      IsInEditMode = false;
    }

    // Invoked when the user edits the annotation.
    void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        IsInEditMode = false;
        e.Handled = true;
      }
      else if (e.Key == Key.Escape)
      {
        IsInEditMode = false;
        Text = oldText;
        e.Handled = true;
      }
    }
    private void MainControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (IsEditable)
        IsInEditMode = true;
    }
    #endregion
  }
}