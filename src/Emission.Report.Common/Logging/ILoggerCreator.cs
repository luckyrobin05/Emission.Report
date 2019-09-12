
#region

#endregion

namespace Emission.Report.Common.Logging
{
  public interface ILoggerCreator
  {
    ILogger GetTypeLogger<T>() where T : class;
    ILogger GetTypeLogger<T>(string componentName) where T : class;
  }
}
