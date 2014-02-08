namespace ViDoScanner.ViewModels
{
  using System.Windows;
  using ViDoScanner.Utilities;
  class AnchorViewModel:NotificationObject
  {
    #region Data Members
    private double x = 0;
    private double y = 0;
    #endregion

    public AnchorViewModel()
    {

    }
    public AnchorViewModel(Point p)
    {
      X = p.X;
      Y = p.Y;
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
  }
}
