
#region 

using System;
using System.Collections.Concurrent;
using System.IO;
using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.FileOps
{
  public class FileSerializerBuilder : IFileSerializerBuilder
  {

    #region

    private readonly ILoggerCreator _loggerCreator;
    private readonly ILogger _logger;
    private readonly IFileMover _fileMover;
    private readonly ConcurrentDictionary<string, IFileSerializer> _fileSerializers;

    #endregion

    #region

    public FileSerializerBuilder(
      ILoggerCreator loggerCreator,
      IFileMover fileMover)
    {
      _loggerCreator = loggerCreator;
      _logger = loggerCreator.GetTypeLogger<FileSerializerBuilder>();
      _fileMover = fileMover;
      _fileSerializers = new ConcurrentDictionary<string, IFileSerializer>();
    }

    #endregion

    #region

    public IFileSerializer Get(string inputFilePath)
    {
      if (string.IsNullOrWhiteSpace(inputFilePath))
      {
        throw new FileNotFoundException(string.Format("Invalid input file path:{0}", inputFilePath));
      }
      var fileExtension = Path.GetExtension(inputFilePath).ToUpper();
      IFileSerializer fileSerializer;
      _logger.Info("Getting FileSerializer for *{0} fileType", fileExtension);
      if (!_fileSerializers.TryGetValue(fileExtension, out fileSerializer))
      {
        switch (fileExtension)
        {
          //Unused. 
          //If this is to be used, then an extra converter will be needed to convert json/xml types to/from a common type
          //which can be used across the solution, rather than the xml serialized types
          case Constants.JsonType:
            fileSerializer = new JsonFileSerializer(_loggerCreator);
            break;
          case Constants.XmlType:
            fileSerializer = new XmlFileSerializer(_loggerCreator, _fileMover);
            break;
          default:
            throw new NotImplementedException(string.Format("File read logic for *{0} type not implemented", fileExtension));
        }

        _fileSerializers.TryAdd(fileExtension, fileSerializer);
      }
      return fileSerializer;
    }

    #endregion

  }
}
