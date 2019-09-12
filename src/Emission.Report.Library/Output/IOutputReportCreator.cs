
#region 

using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;

#endregion

namespace Emission.Report.Library.Output
{
  public interface IOutputReportCreator
  {
    GenerationOutput Generate(GenerationReport inputReport);
  }
}
