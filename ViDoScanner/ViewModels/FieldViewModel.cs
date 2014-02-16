namespace ViDoScanner.ViewModels
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Xml.Serialization;
  using ViDoScanner.Utilities;
  using ViDoScanner.Enums;
  [XmlType(TypeName="Field")]
  public class FieldViewModel : ViewModelBasic
  {
    #region Data Members
    private int pixelX;
    private int pixelY;
    private int pixelWidth;
    private int pixelHeight;

    private string name;
    private int index = 0;
    private int numberOfBlanks = 0;
    private int numberOfRows = 1;
    private int numberOfCols = 1;
    private DataTypes type = DataTypes.Alpha;
    private Directions direction = Directions.Vertical;
    private string defaultValue = "X";
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldViewModel"/> class.
    /// Note: Page should be Set after initialize FieldViewModel.
    /// </summary>
    public FieldViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldViewModel"/> class.
    /// </summary>
    /// <param name="page">Page contain this Field</param>
    public FieldViewModel(PageViewModel page)
    {
      this.Page = page;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldViewModel"/> class.
    /// </summary>
    /// <param name="page">Page contain this Field</param>
    /// <param name="rect">Rect of Field</param>
    /// <param name="index">Index of Field</param>
    public FieldViewModel(PageViewModel page, Rect rect, int index)
    {
      Name = "Vùng " + index;
      Index = index;

      this.Page = page;
      X = rect.X;
      Y = rect.Y;
      Width = rect.Width;
      Height = rect.Height;
    }
    #endregion

    #region Model of FieldViewModel
    /// <summary>
    /// Gets or sets index of field.
    /// </summary>
    [XmlAttribute]
    public int Index
    {
      get { return (index); }
      set
      {
        if (index != value)
        {
          index = value;
          RaisePropertyChanged("Index");
        }
      }
    }

    /// <summary>
    /// Gets or sets x location of Field (pixel unit).
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
    /// Gets or sets y location of Field (pixel unit).
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
          RaisePropertyChanged("PixelWidth", "Width", "Cell");
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
          RaisePropertyChanged("PixelHeight", "Height", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets name of field.
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
    /// Gets or sets type of data.
    /// </summary>
    public DataTypes Type
    {
      get { return (type); }
      set
      {
        if (type != value)
        {
          type = value;
          RaisePropertyChanged("Type");
        }
      }
    }

    /// <summary>
    /// Gets or sets direction of data.
    /// </summary>
    public Directions Direction
    {
      get { return (direction); }
      set
      {
        if (direction != value)
        {
          direction = value;
          RaisePropertyChanged("Direction", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of records
    /// </summary>
    public int NumberOfRecords
    {
      get { return (numberOfRows); }
      set
      {
        if (numberOfRows != value)
        {
          numberOfRows = value;
          RaisePropertyChanged("NumberOfRecords", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of selection
    /// </summary>
    public int NumberOfSelection
    {
      get { return (numberOfCols); }
      set
      {
        if (numberOfCols != value)
        {
          numberOfCols = value;
          RaisePropertyChanged("NumberOfSelection", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of blanks
    /// </summary>
    public int NumberOfBlanks
    {
      get { return (numberOfBlanks); }
      set
      {
        if (numberOfBlanks != value)
        {
          numberOfBlanks = value;
          RaisePropertyChanged("NumberOfBlanks");
        }
      }
    }

    /// <summary>
    /// Gets or sets default value.
    /// </summary>
    public string DefaultValue
    {
      get { return (defaultValue); }
      set
      {
        if (defaultValue != value)
        {
          defaultValue = value;
          RaisePropertyChanged("DefaultValue");
        }
      }
    }
    #endregion

    #region Public Properties
    [XmlIgnore]
    public PageViewModel Page { get; set; }

    [XmlIgnore]
    public double X
    {
      get { return (PixelX / this.Page.ScaleX); }
      set { PixelX = (int)(value * this.Page.ScaleX); }
    }

    [XmlIgnore]
    public double Y
    {
      get { return (PixelY / this.Page.ScaleY); }
      set { PixelY = (int)(value * this.Page.ScaleY); }
    }

    [XmlIgnore]
    public double Width
    {
      get { return (PixelWidth / this.Page.ScaleX); }
      set { PixelWidth = (int)(value * this.Page.ScaleX); }
    }

    [XmlIgnore]
    public double Height
    {
      get { return (PixelHeight / this.Page.ScaleY); }
      set { PixelHeight = (int)(value * this.Page.ScaleY); }
    }

    [XmlIgnore]
    public Rect Cell
    {
      get
      {
        return ((Direction == Directions.Vertical) ? 
          new Rect(0, 0, Width / NumberOfSelection, Height / NumberOfRecords) :
          new Rect(0, 0, Width / NumberOfRecords, Height / NumberOfSelection));
      }
    }
    #endregion

    #region Validation
    private static readonly string[] validatedProperties =
    {
      "X",
      "Y",
      "Width",
      "Height",
      "NumberOfBlank",
      "NumberOfRecords",
      "NumberOfSelection"
    };
    protected override string[] ValidatedProperties
    {
      get { return (validatedProperties); }
    }
    protected override string GetValidationError(string propertyName)
    {
      string error = null;
      switch (propertyName)
      {
        case "X":
          error = DoAssert(X < 0, "Giá trị X không hợp lệ.");
          break;
        case "Y":
          error = DoAssert(Y < 0, "Giá trị Y không hợp lệ.");
          break;
        case "Width":
          error = DoAssert(Width <= 0, "Giá trị Width không hợp lệ.");
          break;
        case "Height":
          error = DoAssert(Height <= 0, "Giá trị Height không hợp lệ.");
          break;
        case "NumberOfBlanks":
          error = DoAssert(NumberOfBlanks < 0, "Số lượng khoảng trắng phải lớn hơn hoặc bằng 0.");
          break;
        case "NumberOfRecords":
          error = DoAssert(NumberOfRecords <= 0, "Số lượng bản ghi phải lớn hơn 0.");
          break;
        case "NumberOfSelection":
          error = DoAssert(NumberOfSelection <= 0, "Số lượng lựa chọn phải lớn hơn 0.");
          break;
      }
      return (error);
    }
    #endregion
  }
}
