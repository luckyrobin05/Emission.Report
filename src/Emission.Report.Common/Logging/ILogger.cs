
#region

using System;

#endregion

namespace Emission.Report.Common.Logging
{
  public interface ILogger
  {
    void Debug(string message, params object[] args);
    void Debug(string message, Exception exception);
    void Error(string message, params object[] args);
    void Error(string message, Exception exception);
    void Info(string message, params object[] args);
    void Info(string message, Exception exception);
    void Warn(string message, params object[] args);
    void Warn(string message, Exception exception);
  }
}
