namespace ViDoScanner.ViewModels
{
  public class ConfigViewModel : ViewModelBasic
  {
    #region Data Members
    private string imagesDirectory;
    private string outputDirectory;

    private string blankSelection;
    private string multiSelection;

    private double ratioThreshold;
    private double ratioTheta;
    #endregion

    public ConfigViewModel()
    {
      BlankSelection = "-";
      MultiSelection = "*";
      RatioThreshold = 25;
      RatioTheta = 125;
    }
    public string ImagesDirectory
    {
      get { return (imagesDirectory); }
      set
      {
        if (imagesDirectory != value)
        {
          imagesDirectory = value;
          RaisePropertyChanged("ImagesDirectory");
        }
      }
    }
    public string OutputDirectory
    {
      get { return (outputDirectory); }
      set
      {
        if (outputDirectory != value)
        {
          outputDirectory = value;
          RaisePropertyChanged("OutputDirectory");
        }
      }
    }

    public string BlankSelection
    {
      get { return (blankSelection); }
      set
      {
        if (blankSelection != value)
        {
          blankSelection = value;
          RaisePropertyChanged("BlankSelection");
        }
      }
    }
    public string MultiSelection
    {
      get { return (multiSelection); }
      set
      {
        if (multiSelection != value)
        {
          multiSelection = value;
          RaisePropertyChanged("MultiSelection");
        }
      }
    }

    public double RatioThreshold
    {
      get { return (ratioThreshold); }
      set
      {
        if (ratioThreshold != value)
        {
          ratioThreshold = value;
          RaisePropertyChanged("RatioThreshold");
        }
      }
    }
    public double RatioTheta
    {
      get { return (ratioTheta); }
      set
      {
        if (ratioTheta != value)
        {
          ratioTheta = value;
          RaisePropertyChanged("RatioTheta");
        }
      }
    }

  }
}
