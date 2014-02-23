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
  using ViDoScanner.Core;
  using ViDoScanner.Utilities;

  [XmlRootAttribute("Template", IsNullable=false)]
  public class TemplateViewModel:ViewModelBasic
  {
    #region Data Members
    private Template template = new Template();
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
    private PageViewModel selectedPage;

    private ICommand createPageCommand;
    private ICommand deletePageCommand;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateViewModel"/> class.
    /// </summary>
    public TemplateViewModel()
    {
      Name = "Template";
    }
    public TemplateViewModel(Template template)
    {
      this.template = template;
    }
    #endregion

    #region Model of TemplateViewModel
    /// <summary>
    /// Gets or sets name of template.
    /// </summary>
    public string Name
    {
      get { return (template.Name); }
      set
      {
        if (template.Name != value)
        {
          template.Name = value;
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
    [XmlIgnore]
    public Template Template
    {
      get
      {
        template.Pages = new Page[Pages.Count];
        for (int i = 0; i < Pages.Count; ++i)
        {
          template.Pages[i] = Pages[i].Page;
        }

        return (template);
      }
    }

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
    public ICommand CreatePageCommand
    {
      get
      {
        return (createPageCommand ?? (createPageCommand = new RelayCommand<string>(
          (x) => CreatePage(),
          (x) => Pages.Count < 1)));
      }
    }

    public void CreatePage()
    {
      var p = Browsers.ShowOpenFile("Chọn ảnh nền cho trang", "Picture (*.jpg)|*.jpg");
      if (!string.IsNullOrWhiteSpace(p))
      {
        int pageIndex = GenerateIndex;
        var page = new PageViewModel()
        {
          Index = pageIndex,
          Name = "Trang " + pageIndex,
          ImagePath = p
        };

        Pages.Add(page);

        SelectedPage = page;
      }
    }

    public ICommand DeletePageCommand
    {
      get
      {
        return (deletePageCommand ?? (deletePageCommand = new RelayCommand<PageViewModel>(
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
