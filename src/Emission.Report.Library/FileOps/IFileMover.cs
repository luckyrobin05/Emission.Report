
namespace Emission.Report.Library.FileOps
{
  public interface IFileMover
  {
    bool Move(string fullFilePath, string targetFolder);
  }
}
