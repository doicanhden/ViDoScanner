namespace ViDoScanner.ViewModels
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Xml.Serialization;
  using ViDoScanner.Core;
  using ViDoScanner.Utilities;
  [XmlType(TypeName="Field")]
  public class FieldViewModel : ViewModelBasic
  {
    #region Data Members
    private Field field = new Field();
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
    public FieldViewModel(PageViewModel page, System.Windows.Rect rect, int index)
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
      get { return (field.Index); }
      set
      {
        if (field.Index != value)
        {
          field.Index = value;
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
      get { return (field.X); }
      set
      {
        if (field.X != value)
        {
          field.X = value;
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
      get { return (field.Y); }
      set
      {
        if (field.Y != value)
        {
          field.Y = value;
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
      get { return (field.Width); }
      set
      {
        if (field.Width != value)
        {
          field.Width = value;
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
      get { return (field.Height); }
      set
      {
        if (field.Height != value)
        {
          field.Height = value;
          RaisePropertyChanged("PixelHeight", "Height", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets name of field.
    /// </summary>
    public string Name
    {
      get { return (field.Name); }
      set
      {
        if (field.Name != value)
        {
          field.Name = value;
          RaisePropertyChanged("Name");
        }
      }
    }

    /// <summary>
    /// Gets or sets type of data.
    /// </summary>
    public ViDoScanner.Enums.DataTypes Type
    {
      get { return ((ViDoScanner.Enums.DataTypes)field.Type); }
      set
      {
        if (field.Type != (DataTypes)value)
        {
          field.Type = (DataTypes)value;
          RaisePropertyChanged("Type");
        }
      }
    }

    /// <summary>
    /// Gets or sets direction of data.
    /// </summary>
    public ViDoScanner.Enums.Directions Direction
    {
      get { return ((ViDoScanner.Enums.Directions)field.Direction); }
      set
      {
        if (field.Direction != (Directions)value)
        {
          field.Direction = (Directions)value;
          RaisePropertyChanged("Direction", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of records
    /// </summary>
    public int NumberOfRecords
    {
      get { return (field.NumberOfRecords); }
      set
      {
        if (field.NumberOfRecords != value)
        {
          field.NumberOfRecords = value;
          RaisePropertyChanged("NumberOfRecords", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of selection
    /// </summary>
    public int NumberOfSelection
    {
      get { return (field.NumberOfSelection); }
      set
      {
        if (field.NumberOfSelection != value)
        {
          field.NumberOfSelection = value;
          RaisePropertyChanged("NumberOfSelection", "Cell");
        }
      }
    }

    /// <summary>
    /// Gets or sets number of blanks
    /// </summary>
    public int NumberOfBlanks
    {
      get { return (field.NumberOfBlanks); }
      set
      {
        if (field.NumberOfBlanks != value)
        {
          field.NumberOfBlanks = value;
          RaisePropertyChanged("NumberOfBlanks");
        }
      }
    }

    /// <summary>
    /// Gets or sets default value.
    /// </summary>
    public string DefaultValue
    {
      get { return (field.DefaultValue); }
      set
      {
        if (field.DefaultValue != value)
        {
          field.DefaultValue = value;
          RaisePropertyChanged("DefaultValue");
        }
      }
    }
    #endregion

    #region Public Properties
    [XmlIgnore]
    public PageViewModel Page { get; set; }

    [XmlIgnore]
    public Field Field { get { return (field); } }

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
    public System.Windows.Rect Cell
    {
      get
      {
        return ((field.Direction == Directions.Vertical) ? 
          new System.Windows.Rect(0, 0, Width / NumberOfSelection, Height / NumberOfRecords) :
          new System.Windows.Rect(0, 0, Width / NumberOfRecords, Height / NumberOfSelection));
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
