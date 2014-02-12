namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using System.Xml.Serialization;
  using ViDoScanner.Utilities;

  [XmlType(TypeName="Anchor")]
  public class AnchorViewModel:ViewModelBasic
  {
    #region Data Members
    private PageViewModel page;
    private Rect rect = new Rect();
    #endregion

    #region Constructors
    /// <summary>
    /// Initialize a new object of AnchorViewModel class.
    /// Note: Page should be Set after initialize Field.
    /// </summary>
    public AnchorViewModel()
    {
    }

    /// <summary>
    /// Initialize a new object of AnchorViewModel class.
    /// </summary>
    /// <param name="page">Page contain this Anchor</param>
    public AnchorViewModel(PageViewModel page)
    {
      this.page = page;
    }

    /// <summary>
    /// Initialize a new object of AnchorViewModel class.
    /// </summary>
    /// <param name="page">Page contain this Anchor</param>
    /// <param name="rect">Rect of Anchor</param>
    public AnchorViewModel(PageViewModel page, Rect r)
    {
      this.rect = r;
      this.page = page;
    }
    #endregion

    /// <summary>
    /// Gets or sets X location of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="X")]
    public int PixelX
    {
      get { return ((int)(X * page.ScaleX)); }
      set { X = value / page.ScaleX; }
    }

    /// <summary>
    /// Gets or sets Y location of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Y")]
    public int PixelY
    {
      get { return ((int)(Y * page.ScaleY)); }
      set { Y = value / page.ScaleY; }
    }

    /// <summary>
    /// Gets or sets Width of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Width")]
    public int PixelWidth
    {
      get { return ((int)(Width * page.ScaleX)); }
      set { Width = value / page.ScaleX; }
    }

    /// <summary>
    /// Gets or sets Height of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Height")]
    public int PixelHeight
    {
      get { return ((int)(Height * page.ScaleY)); }
      set { Height = value / page.ScaleY; }
    }

    [XmlIgnore]
    public double X
    {
      get { return (rect.X); }
      set
      {
        if (rect.X != value)
        {
          rect.X = value;
          RaisePropertyChanged("X");
        }
      }
    }
    [XmlIgnore]
    public double Y
    {
      get { return (rect.Y); }
      set
      {
        if (rect.Y != value)
        {
          rect.Y = value;
          RaisePropertyChanged("Y");
        }
      }
    }
    [XmlIgnore]
    public double Width
    {
      get { return (rect.Width); }
      set
      {
        if (rect.Width != value)
        {
          rect.Width = value;
          RaisePropertyChanged("Width");
        }
      }
    }
    [XmlIgnore]
    public double Height
    {
      get { return (rect.Height); }
      set
      {
        if (rect.Height != value)
        {
          rect.Height = value;
          RaisePropertyChanged("Height");
        }
      }
    }
  }
}
