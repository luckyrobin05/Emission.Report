
#region

using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

using System.ServiceModel.Web;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;
using Emission.Report.Library.Process;
using Emission.Report.Library.Enums;

using System.Collections.Generic;

#endregion

namespace Emission.Report.Library.Service
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
  public class ReportService : IReportService
  {

    #region Fields

    private ServiceHost _listenerHost;
    private readonly ILogger _logger;
    private readonly IConfigSettings _configSettings;
    private readonly IInputReportProcessor _reportProcessor;

    #endregion Fields

    #region Constructor

    public ReportService(
      ILoggerCreator loggerCreator,
      IConfigSettings configSettings,
      IInputReportProcessor reportProcessor)
    {
      _logger = loggerCreator.GetTypeLogger<ReportService>();
      _configSettings = configSettings;
      _reportProcessor = reportProcessor;
    }

    #endregion Constructor

    #region Methods

    public bool Start()
    {
      _logger.Info("Starting Service");
      try
      {
        var baseUrl = string.Format("http://{0}:8737/reportservice/", Environment.MachineName);
        _logger.Info("Service Url: {0}", baseUrl);
        var baseAddress = new Uri(baseUrl);
        _listenerHost = new ServiceHost(this, baseAddress);
        var binding = new WebHttpBinding();
        var endpoint =
          _listenerHost.AddServiceEndpoint(typeof(IReportService), binding, string.Empty);
        endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
        _listenerHost.Open();
        _logger.Info("Started Service");
        return true;
      }
      catch(Exception ex)
      {
        _logger.Error("Error while starting Service", ex);
        return false;
      }
    }

    public bool Stop()
    {
      _logger.Info("Stopping Service");
      try
      {
        if (_listenerHost == null) return true;
        _listenerHost.Close();
        _listenerHost = null;
        _logger.Info("Stopped Service");
        return true;
      }
      catch (Exception ex)
      {
        _logger.Error("Error while stopping Service", ex);
        return false;
      }
    }

    public string Ping()
    {
      return "Service alive and running!";
    }

    public string Process(string format, string path)
    {
      path = string.IsNullOrWhiteSpace(path) ? _configSettings.InputFileFolder : path;

      if (!Directory.Exists(path))
      {
        return string.Format("Invalid path : {0}", path);
      }

      var files =
        string.IsNullOrWhiteSpace(format)
        ? Directory.GetFiles(path).ToList()
        : Directory.GetFiles(path).Where(x => x.ToUpper().Contains(string.Format(".{0}", format.ToUpper()))).ToList();

      if (!files.Any())
      {
        return string.Format("No files to process at : {0}", path);
      }

      for (var index = 0; index < files.Count; index++)
      {
        _reportProcessor.Process(files[index]);
      }

      return string.Format("Processed files at : {0}", path);
    }

    #endregion Methods

  }
}
