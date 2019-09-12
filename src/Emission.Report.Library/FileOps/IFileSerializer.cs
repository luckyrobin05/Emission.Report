
#region

using Emission.Report.Library.Types;

#endregion

namespace Emission.Report.Library.FileOps
{
  public interface IFileSerializer
  {
    T Read<T>(string fullFilePath) where T : IInputData;
    bool Write<T>(string fullFilePath, T outputdata) where T : IOutputData;
  }
}
