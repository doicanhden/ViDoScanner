namespace ViDoScanner.Enums
{
  using System.ComponentModel;
  using ViDoScanner.Resources;
  public enum Directions
  {
    [LocalizableDescription(@"Vertical", typeof(Localization))]
    Vertical,

    [LocalizableDescription(@"Horizontal", typeof(Localization))]
    Horizontal,
  }
}
