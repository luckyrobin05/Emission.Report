
#region

using System;
using log4net;

#endregion

namespace Emission.Report.Common.Logging
{
  public class Log4netLogger : ILogger
  {

    #region Fields

    private readonly ILog _log;
    private readonly string _componentName;
    private readonly int _minComponentLength;

    #endregion Fields

    #region Constructor

    public Log4netLogger(ILog log, string componentName = null, int minComponentLength = 20)
    {
      if (ReferenceEquals(log, null))
      {
        throw new ArgumentNullException();
      }

      _componentName = componentName;
      _minComponentLength = minComponentLength;
      _log = log;
    }

    #endregion Constructor

    #region Methods

    private string GenerateMessage(string message, object[] args)
    {
      if (!string.IsNullOrWhiteSpace(_componentName))
      {
        var component = string.Format("{{0, -{0}}}: {{1}}", _minComponentLength);
        message = string.Format(component, _componentName, message);
      }

      return string.Format(message, args);
    }

    public void Debug(string message, params object[] args)
    {
      if (!ReferenceEquals(args, null))
      {
        message = GenerateMessage(message, args);
      }

      _log.Debug(message);
    }

    public void Debug(string message, Exception exception)
    {
      _log.Debug(message, exception);
    }

    public void Error(string message, params object[] args)
    {
      if (!ReferenceEquals(args, null))
      {
        message = GenerateMessage(message, args);
      }

      _log.Error(message);
    }

    public void Error(string message, Exception exception)
    {
      _log.Error(message, exception);
    }

    public void Info(string message, params object[] args)
    {
      if (!ReferenceEquals(args, null))
      {
        message = GenerateMessage(message, args);
      }

      _log.Info(message);
    }

    public void Info(string message, Exception exception)
    {
      _log.Info(message, exception);
    }

    public void Warn(string message, params object[] args)
    {
      if (!ReferenceEquals(args, null))
      {
        message = GenerateMessage(message, args);
      }

      _log.Warn(message);
    }

    public void Warn(string message, Exception exception)
    {
      _log.Warn(message, exception);
    }

    #endregion Methods

  }
}
