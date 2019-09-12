
#region

using System;
using System.IO;
using System.Collections.Generic;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Enums;
using Emission.Report.Library.FileOps;
using Emission.Report.Library.Process;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.Job
{
  public class ReportJob : IReportJob
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IConfigSettings _configSettings;
    private readonly IInputReportProcessor _reportProcessor;
    private readonly IFolderWatcher _folderWatcher;

    #endregion Fields

    #region Constructor

    public ReportJob(
      ILoggerCreator loggerCreator,
      IConfigSettings configSettings,
      IInputReportProcessor reportProcessor,
      IFolderWatcher folderWatcher
      )
    {
      _logger = loggerCreator.GetTypeLogger<ReportJob>();
      _configSettings = configSettings;
      _reportProcessor = reportProcessor;
      _folderWatcher = folderWatcher;
    }

    #endregion Constructor

    #region Methods

    public void Run()
    {
      _logger.Info("Beginning monitoring folder for GenerationReport files");

      if (!Directory.Exists(_configSettings.InputFileFolder))
      {
        _logger.Error("Invalid directory {0}.Aborting", _configSettings.InputFileFolder);
        return;
      }

      var nonProcessedFilesAtStart = Directory.GetFiles(_configSettings.InputFileFolder);
      for (var index = 0; index < nonProcessedFilesAtStart.Length; index++)
      {
        _reportProcessor.Process(nonProcessedFilesAtStart[index]);
      }

      _folderWatcher.Watch(_configSettings.InputFileFolder, FileSystemWatcher_Created);

      _logger.Info("Completed monitoring folder for GenerationReport files");
    }

    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs eventArgs)
    {
      _reportProcessor.Process(eventArgs.FullPath);
    }

    #endregion Methods

  }
}
