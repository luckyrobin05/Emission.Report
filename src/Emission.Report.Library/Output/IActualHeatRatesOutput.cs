
using System.Collections.Generic;

using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;

namespace Emission.Report.Library.Output
{
  public interface IActualHeatRatesOutput
  {
    IList<ActualHeatRates> Get(GenerationReport inputReport);
  }
}
