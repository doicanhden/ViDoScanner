namespace ViDoScanner.ViewModels
{
  using System.Windows.Input;
  using System.Xml.Serialization;
  using ViDoScanner.Core;
  using ViDoScanner.Commands;
  using System.Windows;
  using System.IO;
  using ViDoScanner.Utilities;
  public class ConfigViewModel : ViewModelBasic
  {
    #region Data Members
    private Configuration config = new Configuration();
    private ICommand saveCommand;
    private RelayCommand<Window> cancelCommand;
    #endregion
    public ConfigViewModel()
    {
    }

    public ConfigViewModel(ConfigViewModel config)
    {
      this.config.GrayscaleThreshold = config.GrayscaleThreshold;
      this.config.RatioDelta = config.RatioDelta;
      this.config.RatioThreshold = config.RatioThreshold;
      this.config.BlankSelection = config.BlankSelection;
      this.config.MultiSelection = config.MultiSelection;
    }
    public string BlankSelection
    {
      get { return (config.BlankSelection); }
      set
      {
        if (config.BlankSelection != value)
        {
          config.BlankSelection = value;
          RaisePropertyChanged("BlankSelection");
        }
      }
    }
    public string MultiSelection
    {
      get { return (config.MultiSelection); }
      set
      {
        if (config.MultiSelection != value)
        {
          config.MultiSelection = value;
          RaisePropertyChanged("MultiSelection");
        }
      }
    }

    public double RatioThreshold
    {
      get { return (config.RatioThreshold); }
      set
      {
        if (config.RatioThreshold != value)
        {
          config.RatioThreshold = value;
          RaisePropertyChanged("RatioThreshold");
        }
      }
    }
    public double RatioDelta
    {
      get { return (config.RatioDelta); }
      set
      {
        if (config.RatioDelta != value)
        {
          config.RatioDelta = value;
          RaisePropertyChanged("RatioDelta");
        }
      }
    }

    public int GrayscaleThreshold
    {
      get { return (config.GrayscaleThreshold); }
      set
      {
        if (config.GrayscaleThreshold != value)
        {
          config.GrayscaleThreshold = value;
          RaisePropertyChanged("GrayscaleThreshold");
        }
      }
    }

    [XmlIgnore]
    public Configuration Config { get { return (config); } }

    public ICommand SaveCommand
    {
      get
      {
        return (saveCommand ?? (saveCommand = new RelayCommand<Window>(
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
    public ICommand CreateCommand
    {
      get
      {
        return (createCommand ?? (createCommand = new RelayCommand<Window>(
          (x) =>
          {
            var configName = Browsers.ShowPromptBox("Tạo thông số mới", "Nhập vào tên thông số");

            if (!string.IsNullOrWhiteSpace(configName))
            {
              MainWindowViewModel.SaveConfigAsName(configName, this);

              if (x != null)
              {
                x.DialogResult = true;
                x.Close();
              }
            }
          }, 
          (x) => this.IsValid)));
      }
    }


    private static readonly string[] validatedProperties = {
                                                             "BlankSelection", "MultiSelection"
                                                           };
    private ICommand createCommand;
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      string result = string.Empty;
      switch (propertyName)
      {
        case "BlankSelection":
          result = DoAssert(string.IsNullOrWhiteSpace(BlankSelection), "Không được để trống.");
          break;
        case "MultiSelection":
          result = DoAssert(string.IsNullOrWhiteSpace(MultiSelection), "Không được để trống.");
          break;
        default:
          break;
      }

      return (result);
    }
  }
}
