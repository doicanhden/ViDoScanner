namespace ViDoScanner.ViewModels
{
  using System.Xml.Serialization;
  using ViDoScanner.Core;

  public class ConfigViewModel : ViewModelBasic
  {
    #region Data Members
    private Configuration config = new Configuration();
    #endregion

    public ConfigViewModel()
    {
      BlankSelection = "-";
      MultiSelection = "*";
      RatioThreshold = 25;
      RatioDelta = 125;
    }

    public string ImagesDirectory
    {
      get { return (config.ImagesDirectory); }
      set
      {
        if (config.ImagesDirectory != value)
        {
          config.ImagesDirectory = value;
          RaisePropertyChanged("ImagesDirectory");
        }
      }
    }
    public string OutputDirectory
    {
      get { return (config.OutputDirectory); }
      set
      {
        if (config.OutputDirectory != value)
        {
          config.OutputDirectory = value;
          RaisePropertyChanged("OutputDirectory");
        }
      }
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
    public Configuration Model { get { return (config); } }

  }
}
