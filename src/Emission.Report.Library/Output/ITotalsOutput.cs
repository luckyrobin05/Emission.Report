
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;

namespace Emission.Report.Library.Output
{
  public interface ITotalsOutput
  {
    Totals Get(GenerationReport inputReport);
  }
}
