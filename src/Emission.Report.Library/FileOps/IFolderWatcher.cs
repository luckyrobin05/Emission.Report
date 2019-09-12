
#region

using System.IO;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.FileOps
{
  public interface IFolderWatcher
  {
    void Watch(string inputFileFolder, FileSystemEventHandler FileSystemWatcher_Created, string fileFiler = Constants.AllFileFilter);
  }
}
