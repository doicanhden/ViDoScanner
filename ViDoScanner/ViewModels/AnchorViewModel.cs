namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using System.Xml.Serialization;
  using ViDoScanner.Core;
  using ViDoScanner.Utilities;

  [XmlType(TypeName="Anchor")]
  public class AnchorViewModel:ViewModelBasic
  {
    #region Data Members
    private Anchor anchor = new Anchor();
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="AnchorViewModel"/> class.
    /// </summary>
    public AnchorViewModel()
    {
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets X location of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="X")]
    public int PixelX
    {
      get { return (anchor.X); }
      set
      {
        if (anchor.X != value)
        {
          anchor.X = value;
          RaisePropertyChanged("PixelX", "X");
        }
      }
    }

    /// <summary>
    /// Gets or sets Y location of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Y")]
    public int PixelY
    {
      get { return (anchor.Y); }
      set
      {
        if (anchor.Y != value)
        {
          anchor.Y = value;
          RaisePropertyChanged("PixelY", "Y");
        }
      }
    }

    /// <summary>
    /// Gets or sets Width of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Width")]
    public int PixelWidth
    {
      get { return (anchor.Width); }
      set
      {
        if (anchor.Width != value)
        {
          anchor.Width = value;
          RaisePropertyChanged("PixelWidth", "Width");
        }
      }
    }

    /// <summary>
    /// Gets or sets Height of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Height")]
    public int PixelHeight
    {
      get { return (anchor.Height); }
      set
      {
        if (anchor.Height != value)
        {
          anchor.Height = value;
          RaisePropertyChanged("PixelHeight", "Height");
        }
      }
    }

    [XmlIgnore]
    public Anchor Anchor { get { return (anchor); } }
    [XmlIgnore]
    public PageViewModel Page { get; set; }
    #endregion

    #region Binding Properties
    [XmlIgnore]
    public double X
    {
      get { return (PixelX / Page.ScaleX); }
      set { PixelX = (int)(value * Page.ScaleX); }
    }
    [XmlIgnore]
    public double Y
    {
      get { return (PixelY / Page.ScaleY); }
      set { PixelY = (int)(value * Page.ScaleY); }
    }
    [XmlIgnore]
    public double Width
    {
      get { return (PixelWidth / Page.ScaleX); }
      set { PixelWidth = (int)(value * Page.ScaleX); }
    }
    [XmlIgnore]
    public double Height
    {
      get { return (PixelHeight / Page.ScaleY); }
      set { PixelHeight = (int)(value * Page.ScaleY); }
    }
    #endregion
  }
}
