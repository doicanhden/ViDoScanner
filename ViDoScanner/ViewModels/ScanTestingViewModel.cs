﻿namespace ViDoScanner.ViewModels
{
  using Com.StellmanGreene.CSVReader;
  using System.Data;
  using System.IO;
  using System.Text;
  using System.Windows.Input;
  using ViDoScanner.Commands;
  using ViDoScanner.Processing;

  public class ScanTestingViewModel:ViewModelBasic
  {
    #region Data Members
    private MainWindowViewModel main;
    private ConfigViewModel config;
    private ICommand scanCommand;
    private DataTable data;
    private string imagePath;
    #endregion

    public ConfigViewModel Config
    {
      get { return (config); }
    }

    public string ImagePath
    {
      get { return (imagePath); }
      set
      {
        if (imagePath != value)
        {
          imagePath = value;
          RaisePropertyChanged("ImagePath");
        }
      }
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

    public ScanTestingViewModel(MainWindowViewModel main)
    {
      this.config = new ConfigViewModel(main);
      this.main = main;
    }

    public ICommand ScanCommand
    {
      get
      {
        return (scanCommand ?? (scanCommand = new RelayCommand<object>(
          (x)=>
          {
            var sb = new StringBuilder();
            sb.AppendLine(Scanner.GetPageCSVHeader(main.Template.Pages[0].Page));
            sb.Append(Scanner.Image(ImagePath, main.Template.Pages[0].Page, config.Config));

            var csv = new CSVReader(sb.ToString());
            Data = csv.CreateDataTable(true);
          },
          (x) => this.IsValid)));
      }
    }

    #region Validation
    private static readonly string[] validatedProperties = {"ImagePath"};
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    public override bool IsValid
    {
      get
      {
        return (base.IsValid && main.IsValid);
      }
    }
    protected override string GetValidationError(string propertyName)
    {
      if (propertyName == "ImagePath")
      {
        if (!File.Exists(ImagePath))
        {
          return ("Ảnh không tồn tại");
        }
      }
      return (base.GetValidationError(propertyName));
    }
    #endregion

  }
}
