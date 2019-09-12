
#region

using System.Linq;
using System.Collections.Generic;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using Emission.Report.Library.Types.Serializable.Output;
using Emission.Report.Library.Types.Serializable.Output.Interfaces;

using Emission.Report.Library.Calculate.GenerationValue;
using Emission.Report.Library.Enums;

#endregion

namespace Emission.Report.Library.Output
{
  public class TotalsOutput : ITotalsOutput
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly ITotalGenerationValueCalculator _totalGenerationValueCalculator;
    private readonly IValueFactorRetriever _valueFactorRetriever;

    #endregion Fields

    #region Constructor

    public TotalsOutput(
      ILoggerCreator loggerCreator,
      ITotalGenerationValueCalculator totalGenerationValueCalculator,
      IValueFactorRetriever valueFactorRetriever
      )
    {
      _logger = loggerCreator.GetTypeLogger<TotalsOutput>();
      _totalGenerationValueCalculator = totalGenerationValueCalculator;
      _valueFactorRetriever = valueFactorRetriever;
    }

    #endregion Constructor

    #region Methods

    public Totals Get(GenerationReport inputReport)
    {
      if (inputReport == null)
      {
        _logger.Warn("No report to retrieve Totals");
        return null;
      }

      var outputGenerators = new List<Generator>();

      if (inputReport.Coal != null
        && inputReport.Coal.Generators != null)
      {
        for (var index = 0; index < inputReport.Coal.Generators.Count; index++)
        {
          var inputGenerator = inputReport.Coal.Generators[index];
          var total = GetOutputGenerator(GeneratorType.Coal, inputGenerator, string.Empty);
          if (total != null)
            outputGenerators.Add(total);
        }
      }
      if (inputReport.Gas != null
        && inputReport.Gas.Generators != null)
      {
        for (var index = 0; index < inputReport.Gas.Generators.Count; index++)
        {
          var inputGenerator = inputReport.Gas.Generators[index];
          var total = GetOutputGenerator(GeneratorType.Gas, inputGenerator, string.Empty);
          if (total != null)
            outputGenerators.Add(total);
        }
      }
      if (inputReport.Wind != null
        && inputReport.Wind.Generators != null)
      {
        for (var index = 0; index < inputReport.Wind.Generators.Count; index++)
        {
          var inputGenerator = inputReport.Wind.Generators[index];
          var total = GetOutputGenerator(GeneratorType.Wind, inputGenerator, inputGenerator.Location);
          if (total != null)
            outputGenerators.Add(total);
        }
      }

      if (!outputGenerators.Any())
      {
        return null;
      }

      return new Totals
      {
        Generator = outputGenerators,
      };
    }

    private Generator GetOutputGenerator(GeneratorType generatorType, IInputGenerator inputGenerator, string location)
    {
      if (inputGenerator == null
        || inputGenerator.Generation == null
        || inputGenerator.Generation.Days == null)
      {
        return null;
      }

      var valueFactor = _valueFactorRetriever.Get(location);

      var totalGenerator = _totalGenerationValueCalculator.GetTotalGenerationValue(inputGenerator, valueFactor);

      return totalGenerator as Generator;
    }

    #endregion Methods

  }
}
