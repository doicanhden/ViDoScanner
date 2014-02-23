namespace ViDoScanner.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.Configuration;
  using System.IO;
  using System.Windows;
  using System.Windows.Input;
  using System.Xml.Serialization;
  using ViDoScanner.Commands;
  using ViDoScanner.Utilities;

  public class MainWindowViewModel : ViewModelBasic
  {
    #region Data Members
    private Window mainWindow;
    private ConfigViewModel config = new ConfigViewModel();
    private TemplateViewModel template;
    private ObservableCollection<MenuItemCheckableViewModel> configFiles = new ObservableCollection<MenuItemCheckableViewModel>();

    private ICommand createTemplateCommand;
    private ICommand openTemplateCommand;
    private ICommand saveTemplateCommand;
    private ICommand saveTemplateAsCommand;
    private ICommand closeTemplateCommand;
    private ICommand changeConfigFileCommand;
    private ICommand createFieldCommand;
    private ICommand showConfigCommand;
    private ICommand showScanTestingCommand;
    private ICommand showScanCommand;
    #endregion

    private static string CurrentConfigName
    {
      get { return (Properties.Settings.Default["ConfigName"].ToString()); }
      set { Properties.Settings.Default["ConfigName"] = value; }
    }
    public static string ConfigsDirectory
    {
      get { return (Environment.CurrentDirectory + @"\Configs"); }
    }
    public static string GetConfigFileName(string configName)
    {
      return (string.Format("{0}{1}{2}.cfg", ConfigsDirectory, Path.DirectorySeparatorChar, configName));
    }
    public static void SaveConfigAsName(string configName, ConfigViewModel config)
    {
      Xml.Serialize(GetConfigFileName(configName), config);
    }

    #region Public Properties
    public ObservableCollection<MenuItemCheckableViewModel> ConfigNames
    {
      get { return (configFiles); }
    }
    public TemplateViewModel Template
    {
      get { return (template); }
      set
      {
        template = value;
        RaisePropertyChanged("Template");
      }
    }
    public ConfigViewModel Config
    {
      get { return (config); }
      set
      {
        config = value;
        RaisePropertyChanged("Config");
      }
    }
    #endregion

    public MainWindowViewModel(Window mainWindow)
    {
      this.mainWindow = mainWindow;

      LoadConfigNames();
      LoadCurrentConfig();
    }

    public Window MainWindow
    {
      get { return (mainWindow); }
    }

    #region Public Commands
    #region Create Template
    private void CreateTemplateExecute(object x)
    {
      this.Template = new TemplateViewModel();
      var dir = Browsers.ShowBrowserFolder("Chọn thư mục chứa chứ thư mục mẫu");
      SaveTemplateAs(dir);
    }
    public ICommand CreateTemplateCommand
    {
      get
      {
        return (createTemplateCommand ?? (createTemplateCommand = new RelayCommand<object>(
          CreateTemplateExecute)));
      }
    }
    #endregion

    #region Open Template
    private void OpenTemplateExecute(object x)
    {
      var dir = Browsers.ShowBrowserFolder("Chọn thư mục mẫu", false);
      if (!string.IsNullOrWhiteSpace(dir))
        OpenTemplate(dir);
    }
    public ICommand OpenTemplateCommand
    {
      get
      {
        return (openTemplateCommand ?? (openTemplateCommand = new RelayCommand<object>(
          OpenTemplateExecute)));
      }
    }
    #endregion

    #region Save Template
    private void SaveTemplateExecute(object x)
    {
      var dir = Browsers.ShowBrowserFolder("Chọn thư mục chứa chứ thư mục mẫu");
      SaveTemplate(dir);
    }
    private bool SaveTemplateCanExecute(object x)
    {
      return (Template != null && Template.IsValid);
    }
    public ICommand SaveTemplateCommand
    {
      get
      {
        return (saveTemplateCommand ?? (saveTemplateCommand = new RelayCommand<object>(
          SaveTemplateExecute, SaveTemplateCanExecute)));
      }
    }
    #endregion

    #region Save Template As
    private void SaveTemplateAsExecute(object x)
    {
      var dir = Browsers.ShowBrowserFolder("Chọn thư mục chứa chứ thư mục mẫu");
      SaveTemplateAs(dir);
    }
    private bool SaveTemplateAsCanExecute(object x)
    {
      return (Template != null && Template.IsValid);
    }
    public ICommand SaveTemplateAsCommand
    {
      get
      {
        return (saveTemplateAsCommand ?? (saveTemplateAsCommand = new RelayCommand<object>(
          SaveTemplateAsExecute, SaveTemplateAsCanExecute)));
      }
    }

    #endregion

    #region Close Template
    private void CloseTemplateExecute(object obj)
    {
      this.Template = null;
    }
    private bool CloseTemplateCanExecute(object obj)
    {
      return (Template != null);
    }
    public ICommand CloseTemplateCommand
    {
      get
      {
        return (closeTemplateCommand ?? (closeTemplateCommand = new RelayCommand<object>(
          CloseTemplateExecute, CloseTemplateCanExecute)));
      }
    }
    #endregion

    #region Show config dialog
    private void ShowConfigExecute(object x)
    {
      var currentConfigName = CurrentConfigName;

      var newConfig = new ConfigViewModel(this);
      var dialog = new Windows.ConfigScanner(newConfig);
      dialog.Owner = this.mainWindow;
      dialog.Title = string.Format("Cấu hình: \"{0}\"", currentConfigName);

      if (dialog.ShowDialog() == true)
      {
        SaveConfigAsName(currentConfigName, newConfig);
        Config = newConfig;
      }
    }
    private bool ShowConfigCanExecute(object x)
    {
      return (Config != null);
    }
    public ICommand ShowConfigCommand
    {
      get
      {
        return (showConfigCommand ?? (showConfigCommand = new RelayCommand<object>(
          ShowConfigExecute, ShowConfigCanExecute)));
      }
    }
    #endregion

    #region Show scan testing dialog
    private void ShowScanTestingExecute(object x)
    {
      var scanTestingViewModel = new ScanTestingViewModel(this);
      var scanTesting = new Windows.ScanTesting(scanTestingViewModel);
      
      scanTesting.Title = "Quét thử nghiệm: " + Properties.Settings.Default["ConfigName"];
      scanTesting.Owner = mainWindow;
      
      if (scanTesting.ShowDialog() == true)
      {
        this.Config = scanTestingViewModel.Config;
      }
    }
    private bool ShowScanTestingCanExecute(object x)
    {
      return (Template != null && Config != null);
    }
    public ICommand ShowScanTestingCommand
    {
      get
      {
        return (showScanTestingCommand ?? (showScanTestingCommand = new RelayCommand<object>(
          ShowScanTestingExecute, ShowScanTestingCanExecute)));
      }
    }
    #endregion


    public ICommand ChangeConfigFileCommand
    {
      get
      {
        return (changeConfigFileCommand ?? (changeConfigFileCommand = new RelayCommand<string>(
          (x) => SelectConfigByName(x),
          (x) => File.Exists(GetConfigFileName(x)))));
      }
    }
    public ICommand CreateFieldCommand
    {
      get
      {
        return (createFieldCommand ?? (createFieldCommand = new RelayCommand<object>(
          (x) =>
          {
            Template.SelectedPage.CreateField(new System.Windows.Rect(0, 0, 50, 50));
          },
          (x) => Template != null && Template.IsValid && Template.SelectedPage != null)));
      }
    }


    public ICommand ShowScanCommand
    {
      get
      {
        return (showScanCommand ?? (showScanCommand = new RelayCommand<object>(
          (x) =>
          {
            var scanViewModel = new ScanViewModel(this);
            var scan = new Windows.Scan(scanViewModel);
            scan.Owner = this.mainWindow;

            scan.ShowDialog();
          },
          (x) => this.IsValid)));
      }
    }
    #endregion


    public void OpenTemplate(string templateFolder)
    {
      if (Directory.Exists(templateFolder))
      {
        templateFolder += Path.DirectorySeparatorChar;

        var template = Xml.Deserialize<TemplateViewModel>(templateFolder + "template.xml");

        foreach (var page in template.Pages)
        {
          if (page.Fields != null)
          {
            foreach (var field in page.Fields)
            {
              field.Page = page;
            }
          }

          if (page.Anchors != null)
          {
            foreach (var anchor in page.Anchors)
            {
              anchor.Page = page;
            }
          }

          page.ImagePath = templateFolder + Path.GetFileName(page.ImagePath);
        }

        this.Template = template;
      }
    }
    public void SaveTemplate(string containFolder)
    {
      if (Directory.Exists(containFolder))
      {
        containFolder += Path.DirectorySeparatorChar;
        var templateFolder = containFolder + Template.Name;

        if (Directory.Exists(templateFolder))
        {
          var mes = string.Format("Mẫu \"{0}\" đã tồn tại, ghi đè?", Template.Name);
          if (MessageBox.Show(mes, "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
          {
            Directory.Delete(templateFolder, true);
          }
          else
          {
            return;
          }
        }

        Directory.CreateDirectory(templateFolder);
        templateFolder += Path.DirectorySeparatorChar;

        Xml.Serialize(templateFolder + "template.xml", Template);

        foreach (var page in Template.Pages)
        {
          var newImagePath = string.Format("{0}page.{1}{2}",
            templateFolder, page.Index, Path.GetExtension(page.ImagePath));

          try
          {
            File.Copy(page.ImagePath, newImagePath, true);
          }
          catch
          {
          }

          page.ImagePath = newImagePath;
        }
      }
    }
    public void SaveTemplateAs(string containDirectory)
    {
      var newName = Browsers.ShowPromptBox("Tạo mẫu mới", "Nhập vào tên mẫu");
      if (!string.IsNullOrWhiteSpace(newName))
      {
        var currentTemplateName = Template.Name;
        Template.Name = newName;
        SaveTemplate(containDirectory);
        Template.Name = currentTemplateName;
      }
    }


    public bool CreateConfig(string configName, ConfigViewModel config)
    {
      var configFileName = GetConfigFileName(configName);
      if (File.Exists(configFileName))
      {
        var mes = string.Format("Cấu hình \"{0}\" đã tồn tại, ghi đè?", configName);
        if (MessageBox.Show(mes, "Cảnh báo") == MessageBoxResult.OK)
        {
          File.Delete(configFileName);
        }
        else
        {
          return (false);
        }
      }

      SaveConfigAsName(configName, config);
      LoadConfigNames();
      SelectConfigByName(configName);

      return (true);
    }

    private void LoadConfigNames()
    {
      if (!Directory.Exists(ConfigsDirectory))
      {
        Directory.CreateDirectory(ConfigsDirectory);
      }

      ConfigNames.Clear();
      var configFiles = Directory.GetFiles(ConfigsDirectory, "*.cfg", SearchOption.TopDirectoryOnly);
      if (configFiles != null && configFiles.Length > 0)
      {
        foreach (var configFile in configFiles)
        {
          ConfigNames.Add(new MenuItemCheckableViewModel()
          {
            Header = Path.GetFileNameWithoutExtension(configFile)
          });
        }
      }
    }
    private void LoadCurrentConfig()
    {
      var currentConfigName = CurrentConfigName;

      if (string.IsNullOrWhiteSpace(currentConfigName))
      {
        CreateConfig("Default", new ConfigViewModel());
      }
      else
      {
        SelectConfigByName(currentConfigName);
      }
    }
    private void SelectConfigByName(string configName)
    {
      Config = Xml.Deserialize<ConfigViewModel>(GetConfigFileName(configName));
      CurrentConfigName = configName;

      foreach (var c in ConfigNames)
      {
        c.IsChecked = (c.Header == configName);
      }
    }


    #region Validation
    private static readonly string[] validatedProperties = {"Template", "Config"};
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      string error = null;

      switch (propertyName)
      {
        case "Template":
          if (Template == null)
            error = "Chưa chọn một mẫu";
          else
            error = DoAssert(!Template.IsValid, "Mẫu không hợp lệ");
          break;
        case "Config":
          if (Config == null)
            error = "Chưa chọn một cấu hình";
          else
            error = DoAssert(!Config.IsValid, "Cấu hình không hợp lệ");
          break;
        default:
          break;
      }

      return (error);
    }
    #endregion
  }
}
