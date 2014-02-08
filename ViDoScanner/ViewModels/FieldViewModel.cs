namespace ViDoScanner.ViewModels
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using ViDoScanner.Utilities;
  public class FieldViewModel : NotificationObject, IDataErrorInfo
  {
    #region Data Members
    private string name;
    private double x;
    private double y;
    private double width;
    private double height;
    private int index;
    private int numberOfBlanks;
    private int numberOfRows;
    private int numberOfCols;
    #endregion

    #region Constructors
    public FieldViewModel(Rect rect, int index)
    {
      Index = index;
      X = rect.X;
      Y = rect.Y;
      Width = rect.Width;
      Height = rect.Height;
    }
    #endregion

    #region Public Properties
    public double X
    {
      get { return (x); }
      set
      {
        if (x != value)
        {
          x = value;
          RaisePropertyChanged("X");
        }
      }
    }
    public double Y
    {
      get { return (y); }
      set
      {
        if (y != value)
        {
          y = value;
          RaisePropertyChanged("Y");
        }
      }
    }
    public double Width
    {
      get { return (width); }
      set
      {
        if (width != value)
        {
          width = value;
          RaisePropertyChanged("Width", "Cell");
        }
      }
    }
    public double Height
    {
      get { return (height); }
      set
      {
        if (height != value)
        {
          height = value;
          RaisePropertyChanged("Height", "Cell");
        }
      }
    }
    public Rect Cell
    {
      get
      {
        return (new Rect(0, 0,
          Width  / NumberOfCols,
          Height / NumberOfRows));
      }
    }
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
    public int NumberOfRows
    {
      get { return (numberOfRows); }
      set
      {
        if (numberOfRows != value)
        {
          numberOfRows = value;
          RaisePropertyChanged("NumberOfRows", "Cell");
        }
      }
    }
    public int NumberOfCols
    {
      get { return (numberOfCols); }
      set
      {
        if (numberOfCols != value)
        {
          numberOfCols = value;
          RaisePropertyChanged("NumberOfCols", "Cell");
        }
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
      "NumberOfRows",
      "NumberOfCols"
    };
    public bool IsValid
    {
      get
      {
        foreach (string propertyName in validatedProperties)
        {
          if (GetValidationError(propertyName) != null)
            return (false);
        }
        return (true);
      }
    }
    private string GetValidationError(string propertyName)
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
        case "NumberOfRows":
          error = DoAssert(NumberOfRows <= 0, "Số lượng cột phải lớn hơn 0.");
          break;
        case "NumberOfCols":
          error = DoAssert(NumberOfCols <= 0, "Số lượng dòng phải lớn hơn 0.");
          break;
      }
      return (error);
    }

    private string DoAssert(bool isError, string error)
    {
      return (isError ? error : null);
    }
    #endregion

    #region Implementation of IDataErrorInfo
    string IDataErrorInfo.Error
    {
      get { return (null); }
    }
    string IDataErrorInfo.this[string propertyName]
    {
      get { return (GetValidationError(propertyName)); }
    }
    #endregion
  }
}
