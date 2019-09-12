
#region

using System;
using System.IO;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.FileOps
{
  public class FolderWatcher : IFolderWatcher
  {

    #region Fields

    private readonly ILogger _logger;

    #endregion Fields

    #region Constructor

    public FolderWatcher(
      ILoggerCreator loggerCreator)
    {
      _logger = loggerCreator.GetTypeLogger<FolderWatcher>();
    }

    #endregion Constructor

    #region Methods

    public void Watch(
      string inputFileFolder, FileSystemEventHandler FileSystemWatcher_Created, string fileFiler = Constants.AllFileFilter)
    {
      var fileSystemWatcher = new FileSystemWatcher(inputFileFolder);
      fileSystemWatcher.Created += FileSystemWatcher_Created;
      fileSystemWatcher.Filter = fileFiler;
      fileSystemWatcher.EnableRaisingEvents = true;
      fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
      fileSystemWatcher.IncludeSubdirectories = false;

      _logger.Info("Starting file watcher to listen for new files named like : '{0}' in the path {1}", fileFiler, inputFileFolder);

      _logger.Info("Press 'q' to quit.");

      while (Console.Read() != 'q') ;

      _logger.Info("User pressed q. Ending Monitoring");
    }
    
    #endregion Methods

  }
}
