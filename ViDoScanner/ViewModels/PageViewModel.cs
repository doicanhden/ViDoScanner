namespace ViDoScanner.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media.Imaging;
  using System.Xml.Serialization;
  using ViDoScanner.Commands;
  using ViDoScanner.Core;
  using ViDoScanner.Processing;

  [XmlType(TypeName="Page")]
  public class PageViewModel:ViewModelBasic
  {
    #region Data Members
    private Page page = new Page();

    private ICommand createFieldCommand;
    private ICommand selectFieldCommand;
    private ICommand deleteFieldCommand;
    private FieldViewModel selectedField;

    private bool isInCreationMode;
    private BitmapImage image;
    private double scaleX;
    private double scaleY;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="PageViewModel"/> class.
    /// </summary>
    public PageViewModel()
    {
      this.Fields = new ObservableCollection<FieldViewModel>();
      this.Anchors = new ObservableCollection<AnchorViewModel>();
    }
    #endregion

    public void CreateField(System.Windows.Rect rect)
    {
      var field = new FieldViewModel(this, rect, GenerateIndex);
      Fields.Add(field);
      SelectedField = field;
    }

    #region Model of PageViewModel
    /// <summary>
    /// Gets or sets index of page.
    /// </summary>
    [XmlAttribute]
    public int Index
    {
      get { return (page.Index); }
      set
      {
        if (page.Index != value)
        {
          page.Index = value;
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
      get { return (page.Width); }
      set
      {
        if (page.Width != value)
        {
          page.Width = value;
          RaisePropertyChanged("PixelWidth", "Width");
        }
      }
    }

    /// <summary>
    /// Gets or sets Height (pixel unit).
    /// </summary>
    [XmlAttribute(AttributeName="Height")]
    public int PixelHeight
    {
      get { return (page.Height); }
      set
      {
        if (page.Height != value)
        {
          page.Height = value;
          RaisePropertyChanged("PixelHeight", "Height");
        }
      }
    }

    /// <summary>
    /// Gets or sets image path of page.
    /// </summary>
    public string ImagePath
    {
      get { return (page.ImagePath); }
      set
      {
        if (page.ImagePath != value)
        {
          page.ImagePath = value;
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
    public Resolution Resolution
    {
      get { return (page.Resolution); }
      set
      {
        if (page.Resolution != value)
        {
          page.Resolution = value;
          RaisePropertyChanged("Resolution");
        }
      }
    }

    /// <summary>
    /// Gets or sets name of page.
    /// </summary>
    public string Name
    {
      get { return (page.Name); }
      set
      {
        if (page.Name != value)
        {
          page.Name = value;
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
    [XmlIgnore]
    public Page Page
    {
      get
      {
        if (Fields != null)
        {
          page.Fields = new Field[Fields.Count];
          for (int i = 0; i < Fields.Count; ++i)
          {
            page.Fields[i] = Fields[i].Field;
          }
        }

        if (Anchors != null)
        {
          page.Anchors = new Anchor[Anchors.Count];
          for (int i = 0; i < Anchors.Count; ++i)
          {
            page.Anchors[i] = Anchors[i].Anchor;
          }
        }

        return (page);
      }
    }

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
      get { return (PixelWidth / ScaleX); }
      set { PixelWidth = (int)(value * ScaleX); }
    }

    /// <summary>
    /// Gets or sets Height (WPF Unit).
    /// </summary>
    [XmlIgnore]
    public double Height
    {
      get { return (PixelHeight / ScaleY); }
      set { PixelHeight = (int)(value * ScaleY); }
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
    public ICommand CreateFieldCommand
    {
      get
      {
        return (createFieldCommand ?? (createFieldCommand = new RelayCommand<System.Windows.Rect>(
          (x) => CreateField(x),
          (x) => !(x.IsEmpty) && x.Width > 10 && x.Height > 10)));
      }
    }

    /// <summary>
    /// Select a field. Parameter: Reference to Field.
    /// </summary>
    public ICommand SelectFieldCommand
    {
      get
      {
        return (selectFieldCommand ?? (selectFieldCommand = new RelayCommand<FieldViewModel>(
          (x) => SelectedField = x,
          (x) =>(SelectedField == null || SelectedField.IsValid ||
            (x != null && !x.IsValid)) && !IsInCreationMode)));
      }
    }

    /// <summary>
    /// Delete a field.
    /// </summary>
    public ICommand DeleteFieldCommand
    {
      get
      {
        return (deleteFieldCommand ?? (deleteFieldCommand = new RelayCommand<FieldViewModel>(
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
