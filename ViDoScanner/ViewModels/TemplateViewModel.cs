namespace ViDoScanner.ViewModels
{
  using System.Collections.ObjectModel;
  using System.IO;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media;
  using System.Xml.Serialization;
  using ViDoScanner.Commands;
  using ViDoScanner.Utilities;

  [XmlRootAttribute("Template", IsNullable=false)]
  public class TemplateViewModel:ViewModelBasic
  {
    #region Data Members
    private string name;
    private ObservableCollection<PageViewModel> pages;
    private ICommand createPage;
    private ICommand selectPage;
    private ICommand deletePage;
    private ICommand saveTemplate;
    private ICommand loadTemplate;
    private PageViewModel selectedPage;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateViewModel"/> class.
    /// </summary>
    public TemplateViewModel()
    {
      Pages = new ObservableCollection<PageViewModel>();
      Name = "Template";
    }
    #endregion

    #region Model of TemplateViewModel
    /// <summary>
    /// Gets or sets name of template.
    /// </summary>
    public string Name
    {
      get { return (name); }
      set
      {
        if (name != value)
        {
          name = value;
          RaisePropertyChanged("Name");
        }
      }
    }

    /// <summary>
    /// Gets or sets list of pages.
    /// </summary>
    public ObservableCollection<PageViewModel> Pages
    {
      get { return (pages); }
      set
      {
        if (pages != value)
        {
          pages = value;
          RaisePropertyChanged("Pages");
        }
      }
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets selected page.
    /// </summary>
    [XmlIgnore]
    public PageViewModel SelectedPage
    {
      get { return (selectedPage); }
      set
      {
        if (selectedPage != value)
        {
          selectedPage = value;
          RaisePropertyChanged("SelectedPage");
        }
      }
    }
    #endregion

    #region Public Commands
    public ICommand LoadTemplate
    {
      get
      {
        return (loadTemplate ?? (loadTemplate = new RelayCommand<string>(
          DoLoadTemplate, (p) => !File.Exists(p))));
      }
    }
    private void DoLoadTemplate(string pathTemplate)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(TemplateViewModel));

      FileStream stream = new FileStream(pathTemplate, FileMode.Open);
      var template = (TemplateViewModel)serializer.Deserialize(stream);
      stream.Close();

      var pathFolder = Path.GetDirectoryName(pathTemplate) + Path.DirectorySeparatorChar;
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

        page.ImagePath = pathFolder + Path.GetFileName(page.ImagePath);
      }

      this.Name = template.Name;
      this.Pages = template.Pages;
    }

    public ICommand SaveTemplate
    {
      get
      {
        return (saveTemplate ?? (saveTemplate = new RelayCommand<string>(
          DoSaveTemplate, (p) => Directory.Exists(p) && IsValid)));
      }
    }
    private void DoSaveTemplate(string pathFolder)
    {
      if (Directory.Exists(pathFolder))
      {
        var templatePath = pathFolder + Path.DirectorySeparatorChar + Name;

        foreach (var page in Pages)
        {
          var newImagePath = templatePath + '.' + page.Index + Path.GetExtension(page.ImagePath);
          try
          {
            File.Copy(page.ImagePath, newImagePath, true);
          }
          catch { }

          page.ImagePath = newImagePath;
        }

        StreamWriter stream = new StreamWriter(templatePath + ".xml");

        XmlSerializer serializer = new XmlSerializer(typeof(TemplateViewModel));
        serializer.Serialize(stream, this);

        stream.Close();
      }

    }

    public ICommand CreatePage
    {
      get
      {
        return (createPage ?? (createPage = new RelayCommand<string>(
          (x) =>
          {
            var page = new PageViewModel(x) { Index = GenerateIndex };
            Pages.Add(page);
            SelectedPage = page;
          },
          (x) => !string.IsNullOrEmpty(x))));
      }
    }

    public ICommand SelectPage
    {
      get
      {
        return (selectPage ?? (selectPage = new RelayCommand<PageViewModel>(
          (x) => SelectedPage = x,
          (x) => x != null)));
      }
    }

    public ICommand DeletePage
    {
      get
      {
        return (deletePage ?? (deletePage = new RelayCommand<PageViewModel>(
          (x) =>
          {
            if (Pages.Contains(x))
            {
              if (SelectedPage == x)
                SelectedPage = null;

              Pages.Remove(x);
            }
          },
          (x) => x != null)));
      }
    }
    #endregion

    #region Private Properties
    /// <summary>
    /// Gets the index for new page.
    /// </summary>
    [XmlIgnore]
    private int GenerateIndex
    {
      get
      {
        int[] indices = Pages.Select(x => x.Index).ToArray();
        System.Array.Sort(indices);

        int i = 0;
        foreach (var index in indices)
        {
          if (i != index)
            break;
          ++i;
        }

        return (i);
      }
    }
    #endregion

    #region Validation
    private static readonly string[] validatedProperties = {"Pages"};
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      if (propertyName == "Pages")
      {
        foreach (var page in Pages)
        {
          if (!page.IsValid)
            return (string.Format("Trang '{0}' không hợp lệ.", page.Name));
        }
      }
      return (base.GetValidationError(propertyName));
    }
    #endregion
  }
}
