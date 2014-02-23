
namespace ViDoScanner.ViewModels
{
  using ViDoScanner.Utilities;
  public class MenuItemCheckableViewModel : NotificationObject
  {
    private string header;
    private bool isChecked;

    public string Header
    {
      get { return (header); }
      set
      {
        if (header != value)
        {
          header = value;
          RaisePropertyChanged("Header");
        }
      }
    }
    public bool IsChecked
    {
      get { return (isChecked); }
      set
      {
        if (isChecked != value)
        {
          isChecked = value;
          RaisePropertyChanged("IsChecked");
        }
      }
    }
  }
}
