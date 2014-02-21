namespace ViDoScanner.ViewModels
{
  using System;
  using System.IO;
  using System.Windows.Input;
  using System.Windows.Threading;
  using ViDoScanner.Commands;
  using ViDoScanner.Processing;

  public class ScanViewModel:ViewModelBasic
  {
    private Scanner scanner = new Scanner();
    private DispatcherTimer dispatcherTimer = new DispatcherTimer();
    private MainWindowViewModel main;
    private ICommand scanCommand;
    private ICommand stopCommand;
    private ICommand pauseCommand;
    private int maximumImages = 1;
    private int scannedImages = 0;

    private string imagesDirectoryName;
    private string outputDirectoryName;

    public int MaximumImages
    {
      get { return (maximumImages); }
      set
      {
        if (maximumImages != value)
        {
          maximumImages = value;
          RaisePropertyChanged("MaximumImages");
        }
      }
    }
    public int ScannedImages
    {
      get { return (scannedImages); }
      set
      {
        if (scannedImages != value)
        {
          scannedImages = value;
          RaisePropertyChanged("ScannedImages");
        }
      }
    }

    public string ImagesDirectoryName
    {
      get { return (imagesDirectoryName); }
      set
      {
        if (imagesDirectoryName != value)
        {
          imagesDirectoryName = value;
          RaisePropertyChanged("ImagesDirectoryName");
        }
      }
    }
    public string OutputDirectoryName
    {
      get { return (outputDirectoryName); }
      set
      {
        if (outputDirectoryName != value)
        {
          outputDirectoryName = value;
          RaisePropertyChanged("OutputDirectoryName");
        }
      }
    }

    public bool IsNotScanning
    {
      get { return (!scanner.IsScanning); }
    }
    public ICommand ScanCommand
    {
      get
      {
        return (scanCommand ?? (scanCommand = new RelayCommand<object>(
          ScanExecute, ScanCanExecute)));
      }
    }
    public ICommand StopCommand
    {
      get
      {
        return (stopCommand ?? (stopCommand = new RelayCommand<object>(
          StopExecute, StopCanExecute)));
      }
    }
    public ICommand PauseCommand
    {
      get
      {
        return (pauseCommand ?? (pauseCommand = new RelayCommand<object>(
          PauseExecute, PauseCanExecute)));
      }
    }

    public ScanViewModel(MainWindowViewModel main)
    {
      this.main = main;

      dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
      dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
    }

    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      MaximumImages = scanner.MaximumImages;
      ScannedImages = scanner.ScannedImages;

      if (MaximumImages == ScannedImages)
      {
        StopExecute(null);
      }
    }

    private void ScanExecute(object x)
    {
      scanner.Config = main.Config.Config;
      scanner.IsScanning = true;
      scanner.ScanAllDirectories(ImagesDirectoryName, "*.jpg", OutputDirectoryName, main.Template.Pages[0].Page);
      dispatcherTimer.Start();
    }
    private bool ScanCanExecute(object x)
    {
      return (this.IsValid);
    }

    private void StopExecute(object x)
    {
      dispatcherTimer.Stop();
      scanner.IsScanning = false;
    }
    private bool StopCanExecute(object x)
    {
      return (scanner.IsScanning);
    }

    private void PauseExecute(object x)
    {
      scanner.IsPausing = !scanner.IsPausing;
    }
    private bool PauseCanExecute(object x)
    {
      return (scanner.IsScanning);
    }
    
    #region Validation
    private static readonly string[] validatedProperties = {"ImagesDirectoryName", "OutputDirectoryName"};
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      string result = null;
      switch (propertyName)
      {
        case "ImagesDirectoryName":
          result = DoAssert(!Directory.Exists(ImagesDirectoryName), "Thư mục chứa ảnh không tồn tại");
          break;
        case "OutputDirectoryName":
          result = DoAssert(!Directory.Exists(OutputDirectoryName), "Thư mục chứa ảnh không tồn tại");
          break;
        default:
          break;
      }

      return (result);
    }
    #endregion
  }
}
