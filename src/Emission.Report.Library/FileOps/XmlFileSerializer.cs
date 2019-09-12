
#region 

using System;
using System.IO;
using System.Xml.Serialization;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.FileOps
{
  public class XmlFileSerializer : IFileSerializer
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IFileMover _fileMove;

    #endregion Fields

    #region Constructor

    public XmlFileSerializer(
      ILoggerCreator loggerCreator,
      IFileMover fileMove)
    {
      _logger = loggerCreator.GetTypeLogger<XmlFileSerializer>();
      _fileMove = fileMove;
    }

    #endregion Constructor

    #region Methods

    #region Read

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullFilePath"></param>
    /// <returns></returns>
    T IFileSerializer.Read<T>(string fullFilePath)
    {
      var serializer = new XmlSerializer(typeof(T));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      _logger.Info("Reading input File: {0}", fullFilePath);
      try
      {
        using (var stream = new FileStream(fullFilePath, FileMode.Open))
        {
          var data = (T)serializer.Deserialize(stream);
          return data;
        }
      }
      catch (Exception ex)
      {
        _logger.Error(string.Format("Error while reading input File: {0}", fullFilePath), ex);
        return default(T);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
    {
      _logger.Warn("Unknown Node:" + e.Name + "\t" + e.Text);
    }

    private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {
      var attr = e.Attr;
      _logger.Warn("Unknown Attribute: " + attr.Name + "='" + attr.Value + "'");
    }

    #endregion Read

    #region Write

    bool IFileSerializer.Write<T>(string fullFilePath, T outputdata)
    {
      var serializer = new XmlSerializer(typeof(T));

      try
      {
        var targetFolder = Path.GetDirectoryName(fullFilePath);
        if (!Directory.Exists(targetFolder))
        {
          Directory.CreateDirectory(targetFolder);
        }

        _fileMove.Move(fullFilePath, targetFolder);

        _logger.Info("Writing output File: {0}", fullFilePath);
        using (var stream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
        {
          serializer.Serialize(stream, outputdata);
          stream.Close();
        }

        return true;
      }
      catch (Exception ex)
      {
        _logger.Error(string.Format("Error while writing output File: {0}", fullFilePath), ex);
        return false;
      }
    }

    #endregion Write

    #endregion Methods

  }
}
