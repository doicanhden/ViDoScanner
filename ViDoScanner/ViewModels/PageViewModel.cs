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
  class PageViewModel:NotificationObject
  {
    #region Data Members
    private ICommand createField;
    private ICommand selectField;
    private ICommand deleteField;
    private FieldViewModel selectedField;

    private int index;
    private string name;
    private string imagePath;
    private double x;
    private double y;
    private double width;
    private double height;
    #endregion

    #region Constructors
    public PageViewModel(string imagePath)
    {
      ImagePath = imagePath;
      Fields = new ObservableCollection<FieldViewModel>();
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
            var field = new FieldViewModel(x, GenerateIndex);

            Fields.Add(field);

            SelectedField = field;
          },
          (x) => !(x.IsEmpty))));
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
          (x) => SelectedField == null || SelectedField == x || SelectedField.IsValid)));
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

    #region Public Properties
    /// <summary>
    /// Gets or sets list of fields.
    /// </summary>
    public ObservableCollection<FieldViewModel> Fields { get; private set; }
    /// <summary>
    /// Gets or sets list of anchors.
    /// </summary>
    public ObservableCollection<AnchorViewModel> Anchors { get; private set; }
    /// <summary>
    /// Gets or sets selected field.
    /// </summary>
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
    /// Gets or sets index of page.
    /// </summary>
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
    /// Gets or sets image path of page.
    /// </summary>
    public string ImagePath
    {
      get { return (imagePath); }
      set
      {
        if (imagePath != value)
        {
          imagePath = value;
          RaisePropertyChanged("ImagePath");
        }
      }
    }
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
          RaisePropertyChanged("Width");
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
          RaisePropertyChanged("Height");
        }
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
