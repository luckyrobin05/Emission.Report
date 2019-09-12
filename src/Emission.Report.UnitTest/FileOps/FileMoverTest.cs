
#region

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emission.Report.Library.FileOps;

#endregion

namespace Emission.Report.UnitTest.FileOps
{
  [TestClass]
  public class FileMoverTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow(@"Report.xml", @"C:\Tests\Input", @"C:\Tests\Output")]
    public void Test_Move_ExistingFile(string fileName, string inputFolder, string outputFolder)
    {
      //Setup
      var filePath = CreateFile(inputFolder, fileName);

      //Test
      var status = FileMover.Move(filePath, outputFolder);
      Assert.IsTrue(status);

      //Destroy
      DeleteFolder(inputFolder);
      DeleteFolder(outputFolder);
    }

    private string CreateFile(string fileFolder, string fileName)
    {
      if (!Directory.Exists(fileFolder))
      {
        Directory.CreateDirectory(fileFolder);
      }
      var filePath = Path.Combine(fileFolder, fileName);
      if (!File.Exists(filePath))
      {
        using (var stream = File.Create(filePath))
        { }
      }

      return filePath;
    }

    private void DeleteFolder(string fileFolder)
    {
      if (Directory.Exists(fileFolder))
      {
        Directory.Delete(fileFolder, true);
      }
    }

    [TestMethod]
    [DataRow(@"C:\Report.xml", @"C:\Output")]
    public void Test_Move_NonExistingFile(string filePath, string targetFolder)
    {
      var status = FileMover.Move(filePath, targetFolder);

      Assert.IsFalse(status);
    }

    [TestMethod]
    [DataRow(null, @"C:\Tests\Output")]
    public void Test_Move_InvalidFileName(string filePath, string outputFolder)
    {
      //Setup

      //Test
      var status = FileMover.Move(filePath, outputFolder);
      Assert.IsFalse(status);
    }

    #endregion Tests

  }
}
