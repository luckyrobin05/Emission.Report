
#region 

using Emission.Report.Common.Logging;

#endregion

namespace Emission.Report.Library.FileOps
{
  public class JsonFileSerializer : IFileSerializer
  {

    #region Fields

    private readonly ILogger _logger;

    #endregion Fields

    #region Constructor

    public JsonFileSerializer(
      ILoggerCreator loggerCreator)
    {
      _logger = loggerCreator.GetTypeLogger<XmlFileSerializer>();
    }

    #endregion Constructor

    #region Methods

    #region Read

    /// <summary>
    /// Unused. 
    /// If this is to be used, then an extra converter will be needed to convert json/xml types to/from a common type
    /// which can be used across the solution, rather than the xml serialized types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullFilePath"></param>
    /// <returns></returns>
    T IFileSerializer.Read<T>(string fullFilePath)
    {
      _logger.Info("Json file read logic not implemented");
      return default(T);
    }

    #endregion Read

    #region Write

    /// <summary>
    /// Currently not implemented
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullFilePath"></param>
    /// <param name="outputdata"></param>
    /// <returns></returns>
    bool IFileSerializer.Write<T>(string fullFilePath, T outputdata)
    {
      _logger.Info("Json Write logic not implemented");
      return false;
    }

    #endregion Write

    #endregion Methods

  }
}
