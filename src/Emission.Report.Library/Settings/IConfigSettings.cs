
#region

using Emission.Report.Library.Types.Serializable.Reference;

#endregion

namespace Emission.Report.Library.Settings
{
  public interface IConfigSettings
  {

    string InputFileFolder { get; }
    string InputSuccessFileFolder { get; }
    string InputFailureFileFolder { get; }
    string OutputFileFolder { get; }
    string OutputFilePrefix { get; }

    ReferenceData CurrentReferenceData { get; }

  }
}
