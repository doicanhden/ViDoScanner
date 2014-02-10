namespace ViDoScanner.ViewModels
{
  using System.Collections.ObjectModel;
  using System.Windows.Input;
  using ViDoScanner.Commands;
  using ViDoScanner.Utilities;
  class TemplateViewModel:ViewModelBasic
  {
    #region Data Members
    private ICommand createPage;
    private ICommand selectPage;
    private ICommand deletePage;
    private PageViewModel selectedPage;
    #endregion

    #region Constructors
    public TemplateViewModel()
    {
      Pages = new ObservableCollection<PageViewModel>();
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets list of pages.
    /// </summary>
    public ObservableCollection<PageViewModel> Pages { get; private set; }
    /// <summary>
    /// Gets or sets selected page.
    /// </summary>
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
    public ICommand CreatePage
    {
      get
      {
        return (createPage ?? (createPage = new RelayCommand<string>(
          (x) =>
          {
            var page = new PageViewModel(x);
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
  }
}
