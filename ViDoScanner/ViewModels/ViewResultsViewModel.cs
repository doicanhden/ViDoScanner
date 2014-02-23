namespace ViDoScanner.ViewModels
{
  using Com.StellmanGreene.CSVReader;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Data;
  using System.IO;
  using System.Threading.Tasks;

  public class ViewResultsViewModel : ViewModelBasic
  {
    private string outputDirectoryName;
    private string outputFileName;
    private DataTable data;
    private ObservableCollection<string> outputFileNames;

    public ViewResultsViewModel()
    {
      this.PropertyChanged += ViewResultsViewModel_PropertyChanged;
    }

    public DataTable Data
    {
      get { return (data); }
      set
      {
        data = value;
        RaisePropertyChanged("Data");
      }
    }
    public string OutputFileName
    {
      get { return (outputFileName); }
      set
      {
        if (outputFileName != value)
        {
          outputFileName = value;
          RaisePropertyChanged("OutputFileName");
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
    public ObservableCollection<string> OutputFileNames
    {
      get
      {
        return (outputFileNames ?? (
          outputFileNames = new ObservableCollection<string>()));
      }
      set
      {
        outputFileNames = value;
        RaisePropertyChanged("OutputFileNames");
      }
    }

    void ViewResultsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "OutputDirectoryName":
          if (Directory.Exists(OutputDirectoryName))
          {
            Task task = new Task(() =>
            {
              this.OutputFileNames = new ObservableCollection<string>(
                Directory.GetFiles(OutputDirectoryName, "*.txt", SearchOption.TopDirectoryOnly));
            });

            task.Start();
          }
          break;
        case "OutputFileName":
          if (File.Exists(OutputFileName))
          {
            Task task = new Task(() =>
            {
              try
              {
                var csv = new CSVReader(File.ReadAllText(OutputFileName));
                Data = csv.CreateDataTable(true);
              }
              catch
              {

              }
            });

            task.Start();
          }
          break;
        default:
          break;
      }
    }
  }
}
