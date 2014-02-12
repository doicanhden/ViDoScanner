namespace ViDoScanner.ViewModels
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Windows;
  using System.Linq;
  using System;
  using System.Windows.Input;
  using ViDoScanner.Commands;
  using ViDoScanner.Utilities;
  using System.Xml.Serialization;
  using System.Windows.Media.Imaging;
  using ViDoScanner.Processing.Models;
  

  [XmlType(TypeName="Page")]
  public class PageViewModel:ViewModelBasic
  {
    #region Data Members
    private int index = 0;
    private string name;
    private string imagePath;

    private ICommand createField;
    private ICommand selectField;
    private ICommand deleteField;
    private FieldViewModel selectedField;

    private bool isInCreationMode;
    private BitmapImage image;
    private double scaleX;
    private double scaleY;
    private double width;
    private double height;
    #endregion

    #region Constructors
    /// <summary>
    /// Initialize a new object of PageViewModel class.
    /// </summary>
    public PageViewModel()
    {
      this.Fields = new ObservableCollection<FieldViewModel>();
      this.Anchors = new ObservableCollection<AnchorViewModel>();
    }

    /// <summary>
    /// Initialize a new object of PageViewModel class.
    /// </summary>
    /// <param name="imagePath">Path of image background.</param>
    public PageViewModel(string imagePath)
    {

      this.Fields = new ObservableCollection<FieldViewModel>();
      this.Anchors = new ObservableCollection<AnchorViewModel>();

      this.ImagePath = imagePath;
    }
    #endregion

    #region Model of PageViewModel
    /// <summary>
    /// Gets or sets index of page.
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
    /// Gets or sets Width (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Width")]
    public int PixelWidth
    {
      get { return ((int)(ScaleX * Width)); }
      set { Width = value / ScaleX; }
    }

    /// <summary>
    /// Gets or sets Height (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Height")]
    public int PixelHeight
    {
      get { return ((int)(ScaleY * Height)); }
      set { Height = value / ScaleY; }
    }

    /// <summary>
    /// Gets or sets image path of page.
    /// </summary>
    [XmlAttribute]
    public string ImagePath
    {
      get { return (imagePath); }
      set
      {
        if (imagePath != value)
        {
          imagePath = value;
          RaisePropertyChanged("ImagePath");

          try
          {
            var image = new BitmapImage(new Uri(value));
            Image = image;
          }
          catch { }
        }
      }
    }

    /// <summary>
    /// Gets or sets resolution of image background.
    /// </summary>
    public Resolution Resolution { get; set; }

    /// <summary>
    /// Gets or sets name of page.
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
    /// Gets or sets list of fields.
    /// </summary>
    public ObservableCollection<FieldViewModel> Fields { get; private set; }

    /// <summary>
    /// Gets or sets list of anchors.
    /// </summary>
    public ObservableCollection<AnchorViewModel> Anchors { get; private set; }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets image background.
    /// </summary>
    [XmlIgnore]
    public BitmapImage Image
    {
      get { return (image); }
      set
      {
        image = value;
        RaisePropertyChanged("Image");

        Resolution = new Resolution(value.DpiX, value.DpiY);

        ScaleX = Resolution.X / 96;
        ScaleY = Resolution.Y / 96;

        Width  = image.Width;
        Height = image.Height;
      }
    }

    /// <summary>
    /// Gets or sets Width (WPF Unit).
    /// </summary>
    [XmlIgnore]
    public double Width
    {
      get { return (width); }
      set
      {
        if (width != value)
        {
          width = value;
          RaisePropertyChanged("Width");
        }
      }
    }

    /// <summary>
    /// Gets or sets Height (WPF Unit).
    /// </summary>
    [XmlIgnore]
    public double Height
    {
      get { return (height); }
      set
      {
        if (height != value)
        {
          height = value;
          RaisePropertyChanged("Height");
        }
      }
    }

    /// <summary>
    /// Gets or sets scale X.
    /// </summary>
    [XmlIgnore]
    public double ScaleX
    {
      get { return (scaleX); }
      set
      {
        if (scaleX != value)
        {
          scaleX = value;
          RaisePropertyChanged("ScaleX");
        }
      }
    }

    /// <summary>
    /// Gets or sets scale Y.
    /// </summary>
    [XmlIgnore]
    public double ScaleY
    {
      get { return (scaleY); }
      set
      {
        if (scaleY != value)
        {
          scaleY = value;
          RaisePropertyChanged("ScaleY");
        }
      }
    }

    /// <summary>
    /// Gets or sets selected field.
    /// </summary>
    [XmlIgnore]
    public FieldViewModel SelectedField
    {
      get { return (selectedField); }
      private set
      {
        if (selectedField != value)
        {
          selectedField = value;
          RaisePropertyChanged("SelectedField");
        }
      }
    }

    /// <summary>
    /// Gets or sets IsInCreationMode for creation field.
    /// </summary>
    [XmlIgnore]
    public bool IsInCreationMode
    {
      get { return (isInCreationMode); }
      private set
      {
        if (isInCreationMode != value)
        {
          isInCreationMode = value;
          RaisePropertyChanged("IsInCreationMode");
        }
      }
    }
    #endregion

    #region Public Commands
    /// <summary>
    /// Create a new field. Parameter: Position and size, Rect type.
    /// </summary>
    public ICommand CreateField
    {
      get
      {
        return (createField ?? (createField = new RelayCommand<Rect>(
          (x) =>
          {
            var field = new FieldViewModel(this, x, GenerateIndex);

            Fields.Add(field);

            SelectedField = field;
          },
          (x) => !(x.IsEmpty) && x.Width > 10 && x.Height > 10)));
      }
    }

    /// <summary>
    /// Select a field. Parameter: Reference to Field.
    /// </summary>
    public ICommand SelectField
    {
      get
      {
        return (selectField ?? (selectField = new RelayCommand<FieldViewModel>(
          (x) => SelectedField = x,
          (x) =>(SelectedField == null || SelectedField.IsValid ||
            (x != null && !x.IsValid)) && !IsInCreationMode)));
      }
    }

    /// <summary>
    /// Delete a field.
    /// </summary>
    public ICommand DeleteField
    {
      get
      {
        return (deleteField ?? (deleteField = new RelayCommand<FieldViewModel>(
          (x) =>
          {
            if (Fields.Contains(x))
            {
              if (SelectedField == x)
                SelectedField = null;

              Fields.Remove(x);
            }
          },
          (x) => x != null)));
      }
    }
    #endregion

    #region Private Properties
    /// <summary>
    /// Gets the index for new field.
    /// </summary>
    private int GenerateIndex
    {
      get
      {
        int[] indices = Fields.Select(x => x.Index).ToArray();
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
  }
}
