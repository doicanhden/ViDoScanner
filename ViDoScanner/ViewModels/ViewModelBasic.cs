namespace ViDoScanner.ViewModels
{
  using System.ComponentModel;
  using ViDoScanner.Utilities;
  public class ViewModelBasic:NotificationObject, IDataErrorInfo
  {
    #region Constructors
    protected ViewModelBasic()
    {
    }
    protected ViewModelBasic(string[] validatedProperties)
    {
      ValidatedProperties = validatedProperties;
    }
    #endregion

    #region Properties
    protected virtual string[] ValidatedProperties { get; set; }
    public virtual bool IsValid
    {
      get
      {
        foreach (string propertyName in ValidatedProperties)
        {
          if (GetValidationError(propertyName) != null)
            return (false);
        }
        return (true);
      }
    }
    #endregion

    protected virtual string GetValidationError(string propertyName)
    {
      return (null);
    }
    protected static string DoAssert(bool isError, string error)
    {
      return (isError ? error : null);
    }

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
