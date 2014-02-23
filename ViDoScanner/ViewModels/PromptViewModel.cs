namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using System.Windows.Input;
  using ViDoScanner.Commands;

  public class PromptViewModel:ViewModelBasic
  {
    #region Data Members
    private ICommand okCommand;
    private ICommand cancelCommand;
    private string title;
    private string text;
    private string detail;
    #endregion

    #region Public Properties
    public string Title
    {
      get { return (title); }
      set
      {
        if (title != value)
        {
          title = value;
          RaisePropertyChanged("Title");
        }
      }
    }
    public string Text
    {
      get { return (text); }
      set
      {
        if (text != value)
        {
          text = value;
          RaisePropertyChanged("Text");
        }
      }
    }
    public string Detail
    {
      get { return (detail); }
      set
      {
        if (detail != value)
        {
          detail = value;
          RaisePropertyChanged("Detail");
        }
      }
    }
    #endregion

    #region Public Commands
    public ICommand OkCommand
    {
      get
      {
        return (okCommand ?? (okCommand = new RelayCommand<Window>(
          (x) =>
          {
            if (x != null)
            {
              x.DialogResult = true;
              x.Close();
            }
          },
          (x) => this.IsValid)));
      }
    }
    public ICommand CancelCommand
    {
      get
      {
        return (cancelCommand ?? (cancelCommand = new RelayCommand<Window>(
          (x) =>
          {
            if (x != null)
            {
              x.Close();
            }
          })));
      }
    }
    #endregion

    #region Validations
    private static readonly string[] validatedProperties = {"Text"};
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      if (propertyName == "Text" && string.IsNullOrWhiteSpace(Text))
      {
        return ("Không được để trống.");
      }

      return (null);
    }
    #endregion
  }
}
