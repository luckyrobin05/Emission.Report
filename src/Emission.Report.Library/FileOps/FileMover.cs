
#region

using System;
using System.IO;
using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.FileOps
{
  public class FileMover : IFileMover
  {

    #region

    private readonly ILogger _logger;

    #endregion

    #region

    public FileMover(ILoggerCreator loggerCreator)
    {
      _logger = loggerCreator.GetTypeLogger<FileMover>();
    }

    #endregion

    #region

    public bool Move(string fullFilePath, string targetFolder)
    {
      if (!File.Exists(fullFilePath))
      {
        _logger.Info("File: '{0}' does not exist", fullFilePath);
        return false;
      }

      if (!Directory.Exists(targetFolder))
      {
        Directory.CreateDirectory(targetFolder);
      }

      var fileName = Path.GetFileNameWithoutExtension(fullFilePath);
      var extension = Path.GetExtension(fullFilePath);

      var fileSuffix = 0;

      try
      {
        _logger.Info("Moving file: {0} to folder {1}", fileName, targetFolder);

        var targetPath = Path.Combine(targetFolder, fileName + extension);

        while (File.Exists(targetPath))
        {
          fileSuffix++;

          targetPath = Path.Combine(targetFolder, fileName + Constants.Underscore + fileSuffix + extension);
          continue;
        }

        File.Move(fullFilePath, targetPath);

        _logger.Info("Moved file: {0} to {1}", fileName, targetPath);

        return true;
      }
      catch (Exception ex)
      {
        _logger.Warn(string.Format("Error while moving file: {0} to folder {1}", fileName, targetFolder), ex);
        return false;
      }
    }

    #endregion

  }
}
