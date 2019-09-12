
namespace Emission.Report.Library.FileOps
{
  public interface IFileSerializerBuilder
  {
    IFileSerializer Get(string inputFilePath);
  }
}
