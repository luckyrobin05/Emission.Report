
#region

using System.Configuration;
using System.IO;

using Emission.Report.Common.Logging;
using Emission.Report.Library.FileOps;
using Emission.Report.Library.Types.Serializable.Reference;

#endregion

namespace Emission.Report.Library.Settings
{
  public class ConfigSettings : IConfigSettings
  {

    #region Fields

    public string InputFileFolder { get; private set; }
    public string InputSuccessFileFolder { get; private set; }
    public string InputFailureFileFolder { get; private set; }
    public string OutputFileFolder { get; private set; }
    public string OutputFilePrefix { get; private set; }

    public ReferenceData CurrentReferenceData { get; private set; }

    #endregion Fields

    #region Constructor

    public ConfigSettings(
      ILoggerCreator loggerCreator,
      IFileSerializerBuilder fileSerializerBuilder)
    {
      var logger = loggerCreator.GetTypeLogger<ConfigSettings>();

      this.InputFileFolder = ConfigurationManager.AppSettings.Get(Constants.InputFileFolder);
      this.InputSuccessFileFolder = ConfigurationManager.AppSettings.Get(Constants.InputSuccessFileFolder);
      this.InputFailureFileFolder = ConfigurationManager.AppSettings.Get(Constants.InputFailureFileFolder);
      this.OutputFileFolder = ConfigurationManager.AppSettings.Get(Constants.OutputFileFolder);
      this.OutputFilePrefix = ConfigurationManager.AppSettings.Get(Constants.OutputFilePrefix);

      var referenceFilePath = ConfigurationManager.AppSettings.Get(Constants.ReferenceFileFullPath);

      if (!File.Exists(referenceFilePath))
      {
        logger.Error("Cannot find ReferenceData file at: {0}. Cannot start process", referenceFilePath);
        throw new FileNotFoundException(string.Format("Cannot find ReferenceData file at: {0}. Cannot start process", referenceFilePath));
      }
      var fileSerializer = fileSerializerBuilder.Get(referenceFilePath);
      this.CurrentReferenceData = fileSerializer.Read<ReferenceData>(referenceFilePath);
    }

    #endregion Constructor

  }
}
