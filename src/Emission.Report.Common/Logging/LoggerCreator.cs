
#region

using System.Collections.Generic;
using log4net;

#endregion

namespace Emission.Report.Common.Logging
{
  public class LoggerCreator : ILoggerCreator
  {

    #region Fields

    private static readonly object LockObject = new object();
    private static readonly IDictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();

    #endregion Fields

    #region Methods

    public ILogger GetTypeLogger<T>() where T : class
    {
      return this.GetTypeLogger<T>(null);
    }
    
    public ILogger GetTypeLogger<T>(string componentName) where T : class
    {
      var loggerType = componentName ?? typeof(T).Name;
      if (Loggers.ContainsKey(loggerType))
      {
        return Loggers[loggerType];
      }

      lock (LockObject)
      {
        if (Loggers.ContainsKey(loggerType))
        {
          return Loggers[loggerType];
        }

        var defaultRepo = LogManager.GetRepository();
        var log = LogManager.GetLogger(defaultRepo.Name, loggerType);
        var logger = new Log4netLogger(log, loggerType);
        Loggers[loggerType] = logger;
        return logger;
      }
    }

    #endregion Methods

  }
}
