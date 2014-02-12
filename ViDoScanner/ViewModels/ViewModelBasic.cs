namespace ViDoScanner.ViewModels
{
  using System;
  using System.ComponentModel;
  using ViDoScanner.Utilities;
  public class ViewModelBasic:NotificationObject, IDataErrorInfo
  {
    #region Constructors
    protected ViewModelBasic()
    {
    }
    #endregion

    #region Properties
    public virtual bool IsValid
    {
      get
      {
        if (ValidatedProperties != null)
        {
          foreach (string propertyName in ValidatedProperties)
          {
            if (GetValidationError(propertyName) != null)
              return (false);
          }
        }
        return (true);
      }
    }
    protected virtual string[] ValidatedProperties
    {
      get { return (null); }
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
