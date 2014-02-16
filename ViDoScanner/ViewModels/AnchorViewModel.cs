namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using System.Xml.Serialization;
  using ViDoScanner.Utilities;

  [XmlType(TypeName="Anchor")]
  public class AnchorViewModel:ViewModelBasic
  {
    #region Data Members
    private int pixelX;
    private int pixelY;
    private int pixelWidth;
    private int pixelHeight;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="AnchorViewModel"/> class.
    /// </summary>
    public AnchorViewModel()
    {
    }
    #endregion

    /// <summary>
    /// Gets or sets X location of Field (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="X")]
    public int PixelX
    {
      get { return (pixelX); }
      set
      {
        if (pixelX != value)
        {
          pixelX = value;
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
      get { return (pixelY); }
      set
      {
        if (pixelY != value)
        {
          pixelY = value;
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
      get { return (pixelWidth); }
      set
      {
        if (pixelWidth != value)
        {
          pixelWidth = value;
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
      get { return (pixelHeight); }
      set
      {
        if (pixelHeight != value)
        {
          pixelHeight = value;
          RaisePropertyChanged("PixelHeight", "Height");
        }
      }
    }

    [XmlIgnore]
    public PageViewModel Page { get; set; }
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
  }
}
