namespace ViDoScanner.ViewModels
{
  using System;
  using System.IO;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Threading;
  using ViDoScanner.Commands;
  using ViDoScanner.Processing;

  public class ScanViewModel:ViewModelBasic
  {
    private Scanner scanner = new Scanner();
    private DispatcherTimer dispatcherTimer = new DispatcherTimer();
    private MainWindowViewModel main;
    private ICommand scanStopCommand;
    private ICommand pauseResumeCommand;
    private int scannedImages = 0;
    private int maximumImages = 1;

    private string imagesDirectoryName;
    private string outputDirectoryName;
    private string statusString;

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

    public string StatusString
    {
      get { return (statusString); }
      set
      {
        statusString = value;
        RaisePropertyChanged("StatusString");
      }
    }

    public ICommand ScanStopCommand
    {
      get
      {
        return (scanStopCommand ?? (scanStopCommand = new RelayCommand<object>(
          ScanStopExecute, ScanStopCanExecute)));
      }
    }
    public ICommand PauseResumeCommand
    {
      get
      {
        return (pauseResumeCommand ?? (pauseResumeCommand = new RelayCommand<object>(
          PauseResumeExecute, PauseResumeCanExecute)));
      }
    }

    public ScanViewModel(MainWindowViewModel main)
    {
      this.main = main;

      dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
      dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
    }

    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      MaximumImages = scanner.MaximumImages;
      ScannedImages = scanner.ScannedImages;
      StatusString = string.Format("Đang quét {0} trên {1} ảnh", ScannedImages, MaximumImages);

      if (MaximumImages != 0 && ScannedImages == MaximumImages)
      {
        StatusString = "Xong.";
        dispatcherTimer.Stop();

        if (MessageBox.Show("Đã quét xong, xem kết quả?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          var dialog = new Windows.ViewResults(new ViewResultsViewModel()
          {
            OutputDirectoryName = this.OutputDirectoryName
          });

          dialog.Owner = this.main.MainWindow;
          dialog.ShowDialog();
        }
      }
    }

    private void ScanStopExecute(object obj)
    {
      if (scanner.IsScanning)
      {
        dispatcherTimer.Stop();
        scanner.IsScanning = false;
        scanner.IsPausing = false;
        scanner.MaximumImages = 0;
        scanner.ScannedImages = 0;
      }
      else
      {
        scanner.Config = main.Config.Config;
        scanner.IsPausing = false;
        scanner.IsScanning = true;
        scanner.ScanAllDirectories(ImagesDirectoryName, "*.jpg", OutputDirectoryName, main.Template.Pages[0].Page);
        dispatcherTimer.Start();
      }
    }
    private bool ScanStopCanExecute(object obj)
    {
      if (scanner.IsScanning)
        return (true);

      return (this.IsValid);
    }

    private void PauseResumeExecute(object obj)
    {
      scanner.IsPausing = !scanner.IsPausing;
    }
    private bool PauseResumeCanExecute(object obj)
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
