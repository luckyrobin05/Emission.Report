
#region

using System;
using System.IO;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;
using Emission.Report.Library.FileOps;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;
using Emission.Report.Library.Output;

#endregion

namespace Emission.Report.Library.Process
{
  public class InputReportProcessor : IInputReportProcessor
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IConfigSettings _configSettings;
    private readonly IFileSerializerBuilder _fileSerializerBuilder;
    private readonly IOutputReportCreator _outputReport;
    private readonly IFileMover _fileMover;

    #endregion Fields

    #region Constructor

    public InputReportProcessor(
      ILoggerCreator loggerCreator,
      IConfigSettings configSettings,
      IFileSerializerBuilder fileSerializerBuilder,
      IOutputReportCreator outputReport,
      IFileMover fileMover)
    {
      _logger = loggerCreator.GetTypeLogger<InputReportProcessor>();
      _configSettings = configSettings;
      _fileSerializerBuilder = fileSerializerBuilder;
      _outputReport = outputReport;
      _fileMover = fileMover;
    }

    #endregion Constructor

    #region Methods

    public bool Process(string inputFilePath)
    {
      try
      {
        var inputFileSerializer = _fileSerializerBuilder.Get(inputFilePath);
        var inputData = inputFileSerializer.Read<GenerationReport>(inputFilePath);

        if(inputData == null)
        {
          _logger.Warn("Failed to process output File: {0}. Moving file to : {1}", inputFilePath, _configSettings.InputFailureFileFolder);
          _fileMover.Move(inputFilePath, _configSettings.InputFailureFileFolder);
          return false;
        }

        var inputFileName = Path.GetFileName(inputFilePath);
        var outPutData = _outputReport.Generate(inputData);
        var writeSuccess =
          inputFileSerializer.Write<GenerationOutput>(
            Path.Combine(_configSettings.OutputFileFolder, _configSettings.OutputFilePrefix + Constants.Underscore + inputFileName),
            outPutData);

        if (writeSuccess)
        {
          _logger.Info("Successfully created output File: {0}. Moving file to : {1}", inputFilePath, _configSettings.InputSuccessFileFolder);
          _fileMover.Move(inputFilePath, Path.Combine(_configSettings.InputFileFolder, _configSettings.InputSuccessFileFolder));
          return true;
        }
        else
        {
          _logger.Warn("Failed to create output File: {0}. Moving file to : {1}", inputFilePath, _configSettings.InputFailureFileFolder);
          _fileMover.Move(inputFilePath, Path.Combine(_configSettings.InputFileFolder, _configSettings.InputFailureFileFolder));
          return false;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(string.Format("Error while creating output File: {0}. Moving file to : {1}", inputFilePath, _configSettings.InputFailureFileFolder), ex);
        _fileMover.Move(inputFilePath, Path.Combine(_configSettings.InputFileFolder, _configSettings.InputFailureFileFolder));        
        return false;
      }
    }

    #endregion Methods

  }
}
