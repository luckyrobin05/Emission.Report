
#region

using System.Linq;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;

#endregion

namespace Emission.Report.Library.Output
{
  public class OutputReportCreator : IOutputReportCreator
  {

    #region Fields

    private readonly ILogger _logger;

    private readonly ITotalsOutput _totalsOutput;
    private readonly IMaxEmissionGeneratorsOutput _maxEmissionGeneratorsOutput;
    private readonly IActualHeatRatesOutput _actualHeatRatesOutput;

    #endregion Fields

    #region Constructor

    public OutputReportCreator(
      ILoggerCreator loggerCreator,
      ITotalsOutput totalsOutput,
      IMaxEmissionGeneratorsOutput maxEmissionGeneratorsOutput,
      IActualHeatRatesOutput actualHeatRatesOutput
      )
    {
      _logger = loggerCreator.GetTypeLogger<OutputReportCreator>();
      _totalsOutput = totalsOutput;
      _maxEmissionGeneratorsOutput = maxEmissionGeneratorsOutput;
      _actualHeatRatesOutput = actualHeatRatesOutput;
    }

    #endregion Constructor

    #region Methods

    public GenerationOutput Generate(GenerationReport inputReport)
    {
      var totals = _totalsOutput.Get(inputReport);
      var maxEmissionGenerators = _maxEmissionGeneratorsOutput.Get(inputReport);
      var actualHeatRates = _actualHeatRatesOutput.Get(inputReport);

      var output = new GenerationOutput
      {
        Totals = totals,
        MaxEmissionGenerators = maxEmissionGenerators,
        ActualHeatRates = actualHeatRates == null ? null : actualHeatRates.ToList(),
      };

      return output;
    }

    #endregion Methods

  }
}
