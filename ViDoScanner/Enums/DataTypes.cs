namespace ViDoScanner.Enums
{
  using System.ComponentModel;
  using ViDoScanner.Resources;
  public enum DataTypes
  {
    [LocalizableDescription(@"Alpha", typeof(Localization))]
    Alpha,
    [LocalizableDescription(@"Number1", typeof(Localization))]
    Number1,
    [LocalizableDescription(@"Number2", typeof(Localization))]
    Number2,
    [LocalizableDescription(@"Boolean", typeof(Localization))]
    Boolean
  }

}
