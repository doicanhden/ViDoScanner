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
  public class MainWindowViewModel:ViewModelBasic
  {
    private ConfigViewModel config = new ConfigViewModel();
    private TemplateViewModel template;

    private ICommand openTemplateCommand;
    private ICommand saveTemplateCommand;
    private ICommand changeConfigFileCommand;
    private ICommand createFieldCommand;
    private ICommand showConfigCommand;
    private ICommand showScanTestingCommand;
    private ICommand showScanCommand;
    private Window mainWindow;
    private ObservableCollection<string> configFiles;

    public static string ConfigsFolder
    {
      get
      {
        return (Environment.CurrentDirectory + @"\Configs");
      }
    }
    public static string GetConfigFileName(string configName)
    {
      return (string.Format("{0}{1}{2}.cfg", ConfigsFolder, Path.DirectorySeparatorChar, configName));
    }

    public MainWindowViewModel(Window mainWindow)
    {
      this.mainWindow = mainWindow;
      this.Template = new TemplateViewModel();
      this.ConfigNames = new ObservableCollection<string>();
      LoadConfigNames();

      LoadCurrentConfig();
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

    #region Commands
    public ICommand ShowConfigCommand
    {
      get
      {
        return (showConfigCommand ?? (showConfigCommand = new RelayCommand<object>(
          (x) => ShowConfig())));
      }
    }
    public ICommand ShowScanTestingCommand
    {
      get
      {
        return (showScanTestingCommand ?? (showScanTestingCommand = new RelayCommand<object>(
          (x) =>
          {
            var scanTestingViewModel = new ScanTestingViewModel(this);
            var scanTesting = new Windows.ScanTesting(scanTestingViewModel);

            scanTesting.Title = "Quét thử nghiệm: " + Properties.Settings.Default["ConfigName"];
            scanTesting.Owner = mainWindow;

            if (scanTesting.ShowDialog() == true)
            {
              this.Config = scanTestingViewModel.Config;
            }
          }, (x) => Template != null && Template.IsValid)));
      }
    }
    public ICommand OpenTemplateCommand
    {
      get
      {
        return (openTemplateCommand ?? (openTemplateCommand = new RelayCommand<object>(
          (x) => OpenTemplate(Browsers.ShowBrowserFolder("Chọn thư mục mẫu", false)))));
      }
    }
    public ICommand SaveTemplateCommand
    {
      get
      {
        return (saveTemplateCommand ?? (saveTemplateCommand = new RelayCommand<object>(
          (x) => SaveTemplate(Browsers.ShowBrowserFolder("Chọn thư mục chứa mẫu")),
          (x) => Template.IsValid)));
      }
    }

    public ICommand ChangeConfigFileCommand
    {
      get
      {
        return (changeConfigFileCommand ?? (changeConfigFileCommand = new RelayCommand<string>(
          (x) => ChangeConfig(x),
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
            scan.ShowDialog();
          },
          (x) => this.IsValid
          )));
      }
    }
    #endregion

    public void ShowConfig()
    {
      var newConfig = new ConfigViewModel(Config);
      var dg = new Windows.ConfigScanner(newConfig);
      dg.Title = "Cấu hình:" + Properties.Settings.Default["ConfigName"];

      if (dg.ShowDialog() == true)
      {
        Config = newConfig;
        LoadConfigNames();
      }
    }

    public void ChangeConfig(string name)
    {
      Properties.Settings.Default["ConfigName"] = name;
      Config = Xml.Deserialize<ConfigViewModel>(GetConfigFileName(name));
    }
    public void LoadCurrentConfig()
    {
      var curConfigName = Properties.Settings.Default["ConfigName"].ToString();
      if (string.IsNullOrWhiteSpace(curConfigName))
      {
        SaveConfigAsName("Default", new ConfigViewModel());
        curConfigName = "Default";
      }
      ChangeConfig(curConfigName);
    }
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
          var mesRes = MessageBox.Show("Mẫu này đã tồn tại!! \r\n Lưu đè?", "Cảnh báo", MessageBoxButton.YesNoCancel);
          if (mesRes == MessageBoxResult.Cancel)
          {
            return;
          }

          if (mesRes == MessageBoxResult.No)
          {

          }

          Directory.Delete(templateFolder, true);
        }

        Directory.CreateDirectory(templateFolder);
        templateFolder += Path.DirectorySeparatorChar;

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

        Xml.Serialize(templateFolder + "template.xml", Template);
      }
    }

    public void LoadConfigNames()
    {
      var configFolder = ConfigsFolder;

      if (!Directory.Exists(configFolder))
      {
        Directory.CreateDirectory(configFolder);
      }

      ConfigNames.Clear();

      var configFiles = Directory.GetFiles(configFolder, "*.cfg", SearchOption.TopDirectoryOnly);
      if (configFiles != null && configFiles.Length > 0)
      {
        foreach (var configFile in configFiles)
        {
          ConfigNames.Add(Path.GetFileNameWithoutExtension(configFile));
        }
      }
      else
      {
        SaveConfigAsName("Default", Config);
        ConfigNames.Add("Default");
      }
    }


    public bool Closing()
    {
      return (false);
    }

    public ObservableCollection<string> ConfigNames
    {
      get { return (configFiles); }
      private set
      {
        if (configFiles != value)
        {
          configFiles = value;
          RaisePropertyChanged("ConfigFiles");
        }
      }
    }

    public static void SaveConfigAsName(string configName, ConfigViewModel config)
    {
      Xml.Serialize(string.Format("{0}{1}{2}.cfg", ConfigsFolder, Path.DirectorySeparatorChar, configName), config);
    }
  }
}
