
#region

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emission.Report.Library.FileOps;

#endregion

namespace Emission.Report.UnitTest.FileOps
{
  [TestClass]
  public class FileSerializerBuilderTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests
    
    [TestMethod]
    [DataRow(@"C:\TestFile.xml", typeof(XmlFileSerializer))]
    [DataRow(@"C:\TestFile.json", typeof(JsonFileSerializer))]
    public void Test_FileSerializer_Type(string fileType, Type exptectedType)
    {
      var fileSerializer = FileSerializerBuilder.Get(fileType);

      Assert.AreSame(fileSerializer.GetType(), exptectedType);
    }

    [TestMethod]
    [DataRow(@"C:\TestFile.random")]
    public void Test_FileSerializer_InvalidFileType_Exception(string fileType)
    {
      Assert.ThrowsException<NotImplementedException>(() => { var fileSerializer = FileSerializerBuilder.Get(fileType); });
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("     ")]
    [DataRow(null)]
    public void Test_FileSerializer_FileNotFound_Exception(string fileType)
    {
      Assert.ThrowsException<FileNotFoundException>(() => { var fileSerializer = FileSerializerBuilder.Get(fileType); });
    }

    #endregion Tests

  }
}
