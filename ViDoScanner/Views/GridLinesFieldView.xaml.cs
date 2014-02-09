
namespace ViDoScanner.Views
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  /// <summary>
  /// Interaction logic for GridLinesFieldView.xaml
  /// </summary>
  public partial class GridLinesFieldView : UserControl
  {
    private Visibility FocusVisibility
    {
      get { return (Visibility)GetValue(FocusVisibilityProperty); }
      set { SetValue(FocusVisibilityProperty, value); }
    }

    private static readonly DependencyProperty FocusVisibilityProperty =
        DependencyProperty.Register("FocusVisibility", typeof(Visibility), typeof(GridLinesFieldView), new PropertyMetadata(Visibility.Collapsed));

    
    public GridLinesFieldView()
    {
      InitializeComponent();
    }


    #region Command: Select
    public static readonly DependencyProperty SelectCommandProperty =
      DependencyProperty.Register("SelectCommand", typeof(ICommand),
      typeof(GridLinesFieldView), new PropertyMetadata(null, new PropertyChangedCallback(
        (d, e) => (d as GridLinesFieldView).SelectCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue))));
    public ICommand SelectCommand
    {
      get { return (ICommand)GetValue(SelectCommandProperty); }
      set { SetValue(SelectCommandProperty, (ICommand)value); }
    }

    public static readonly DependencyProperty SelectCommandTargetProperty =
      DependencyProperty.Register("SelectCommandTarget", typeof(IInputElement),
      typeof(GridLinesFieldView), new PropertyMetadata(null));
    public IInputElement SelectCommandTarget
    {
      get { return (IInputElement)GetValue(SelectCommandTargetProperty); }
      set { SetValue(SelectCommandTargetProperty, (IInputElement)value); }
    }

    public static readonly DependencyProperty SelectCommandParameterProperty =
      DependencyProperty.Register("SelectCommandParameter", typeof(object),
      typeof(GridLinesFieldView), new PropertyMetadata(null));
    public object SelectCommandParameter
    {
      get { return (object)GetValue(SelectCommandParameterProperty); }
      set { SetValue(SelectCommandParameterProperty, (object)value); }
    }
    private void SelectCommandChanged(ICommand oldCommand, ICommand newCommand)
    {
      EventHandler eventHandler = (s, e) =>
      {
        if (this.SelectCommand != null)
        {
          this.IsEnabled = CanExecuteCommand(SelectCommand, SelectCommandTarget, SelectCommandParameter);
        }
      };

      // If oldCommand is not null, then we need to remove the handlers.
      if (oldCommand != null)
      {
        oldCommand.CanExecuteChanged -= eventHandler;
      }
      if (newCommand != null)
      {
        newCommand.CanExecuteChanged += eventHandler;
      }
    }
    #endregion

    #region Command: Delete
    public static readonly DependencyProperty DeleteCommandProperty =
      DependencyProperty.Register("DeleteCommand", typeof(ICommand),
      typeof(GridLinesFieldView), new PropertyMetadata(null, new PropertyChangedCallback(
        (d, e) => (d as GridLinesFieldView).DeleteCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue))));
    public ICommand DeleteCommand
    {
      get { return (ICommand)GetValue(DeleteCommandProperty); }
      set { SetValue(DeleteCommandProperty, value); }
    }

    public static readonly DependencyProperty DeleteCommandTargetProperty =
      DependencyProperty.Register("DeleteCommandTarget", typeof(IInputElement),
      typeof(GridLinesFieldView), new PropertyMetadata(null));
    public IInputElement DeleteCommandTarget
    {
      get { return (IInputElement)GetValue(DeleteCommandTargetProperty); }
      set { SetValue(DeleteCommandTargetProperty, value); }
    }

    public static readonly DependencyProperty DeleteCommandParameterProperty =
      DependencyProperty.Register("DeleteCommandParameter", typeof(object),
      typeof(GridLinesFieldView), new PropertyMetadata(null));
    public object DeleteCommandParameter
    {
      get { return (object)GetValue(DeleteCommandParameterProperty); }
      set { SetValue(DeleteCommandParameterProperty, value); }
    }
    private void DeleteCommandChanged(ICommand oldCommand, ICommand newCommand)
    {
      EventHandler eventHandler = (s, e) =>
      {
        if (this.DeleteCommand != null)
        {
          this.DeleteButton.Visibility = CanExecuteCommand(DeleteCommand, DeleteCommandTarget, DeleteCommandParameter) ?
            Visibility.Visible : Visibility.Collapsed;
        }
      };

      // If oldCommand is not null, then we need to remove the handlers.
      if (oldCommand != null)
      {
        oldCommand.CanExecuteChanged -= eventHandler;
      }
      if (newCommand != null)
      {
        newCommand.CanExecuteChanged += eventHandler;
      }
    }
    #endregion

    #region Commands Helper
    private static bool CanExecuteCommand(ICommand command, IInputElement commandTarget, object commandParameter)
    {
      RoutedCommand routedCommand = command as RoutedCommand;
      if (routedCommand != null)
        return (routedCommand.CanExecute(commandParameter, commandTarget));

      return (command.CanExecute(commandParameter));
    }
    private static void ExecuteCommand(ICommand command, IInputElement commandTarget, object commandParameter)
    {

      if (command != null && CanExecuteCommand(command, commandTarget, commandParameter))
      {
        RoutedCommand routedCommand = command as RoutedCommand;
        if (routedCommand != null)
        {
          routedCommand.Execute(commandParameter, commandTarget);
        }
        else
        {
          command.Execute(commandParameter);
        }
      }
    }
    #endregion

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      this.Focus();
    }
    protected override void OnGotFocus(RoutedEventArgs e)
    {
      base.OnGotFocus(e);
      FocusVisibility = Visibility.Visible;
      ExecuteCommand(SelectCommand, SelectCommandTarget, SelectCommandParameter);
    }
    protected override void OnLostFocus(RoutedEventArgs e)
    {
      base.OnLostFocus(e);
      FocusVisibility = Visibility.Collapsed;
    }
    private void SelectionThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
      double l = (double)GetValue(Canvas.LeftProperty);
      double t = (double)GetValue(Canvas.TopProperty);

      if (double.IsNaN(l))
        l = 0;
      if (double.IsNaN(t))
        t = 0;

      SetValue(Canvas.LeftProperty, Math.Max(0, l + e.HorizontalChange));
      SetValue(Canvas.TopProperty, Math.Max(0, t + e.VerticalChange));
    }
    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
      var thumb = sender as Thumb;

      double newWidth, newHeight;
      switch (thumb.VerticalAlignment)
      {
        case VerticalAlignment.Bottom:
          newHeight = Height + e.VerticalChange;
          if (newHeight >= MinHeight && newHeight <= MaxHeight)
            Height = newHeight;
          break;
        case VerticalAlignment.Top:
          newHeight = Height - e.VerticalChange;
          if (newHeight >= MinHeight && newHeight <= MaxHeight)
          {
            Height = newHeight;
            SetValue(Canvas.TopProperty, (double)GetValue(Canvas.TopProperty) + e.VerticalChange);
          }
          break;
        default:
          break;
      }

      switch (thumb.HorizontalAlignment)
      {
        case HorizontalAlignment.Left:
          newWidth = Width - e.HorizontalChange;
          if (newWidth >= MinWidth && newWidth <= MaxWidth)
          {
            Width = newWidth;
            SetValue(Canvas.LeftProperty, (double)GetValue(Canvas.LeftProperty) + e.HorizontalChange);
          }
          break;
        case HorizontalAlignment.Right:
          newWidth = Width + e.HorizontalChange;
          if (newWidth >= MinWidth && newWidth <= MaxWidth)
            Width = newWidth;
          break;
        default:
          break;
      }
    }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      DeleteThisField();
    }
    private void DeleteThisField()
    {
      if (MessageBox.Show(
        string.Format("Bạn muốn xóa vùng '{0}'?", FieldName.Text), "Cảnh báo",
        MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
      {
        ExecuteCommand(DeleteCommand, DeleteCommandTarget, DeleteCommandParameter);
      }
    }
    private void UserControl_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.F2 && this.FieldName.IsEditable)
      {
        this.FieldName.IsInEditMode = true;
      }

      if (e.Key == Key.Delete)
      {
        DeleteThisField();
      }
    }
  }
}
