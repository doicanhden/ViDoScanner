namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using ViDoScanner.Utilities;
  class AnchorViewModel:NotificationObject
  {
    #region Data Members
    private double x = 0;
    private double y = 0;
    private double width = 0;
    private double height = 0;
    #endregion

    public AnchorViewModel()
    {
    }
    public AnchorViewModel(Rect r)
    {
      X = r.X;
      Y = r.Y;
      Width = r.Width;
      Height = r.Height;
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
  }
}
