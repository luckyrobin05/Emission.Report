
#region

using System.Linq;
using System.Collections.Generic;
using Emission.Report.Library.Enums;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using Emission.Report.Library.Types.Serializable.Output;
using EmissionDay = Emission.Report.Library.Types.Serializable.Output.Day;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Calculate.Emissions;

#endregion

namespace Emission.Report.Library.Output
{
  public class MaxEmissionGeneratorsOutput : IMaxEmissionGeneratorsOutput
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IDailyEmissionValueCalculator _dailyEmissionCalculator;
    private readonly IEmissionFactorRetriever _emissionfactorRetriever;

    #endregion Fields

    #region Constructor

    public MaxEmissionGeneratorsOutput(
      ILoggerCreator loggerCreator,
      IDailyEmissionValueCalculator dailyEmissionCalculator,
      IEmissionFactorRetriever emissionfactorRetriever
      )
    {
      _logger = loggerCreator.GetTypeLogger<MaxEmissionGeneratorsOutput>();
      _dailyEmissionCalculator = dailyEmissionCalculator;
      _emissionfactorRetriever = emissionfactorRetriever;
    }

    #endregion Constructor

    #region Methods

    public MaxEmissionGenerators Get(GenerationReport inputReport)
    {
      if (inputReport == null)
      {
        _logger.Warn("No report to retrieve MaxEmissionGenerator");
        return null;
      }

      _logger.Info("Getting MaxEmissionGenerators"); 

      var dailyEmissions = new List<EmissionDay>();

      if (inputReport.Coal != null
        && inputReport.Coal.Generators != null)
      {
        var emissionFactor = _emissionfactorRetriever.Get(GeneratorType.Coal);
        for (var index = 0; index < inputReport.Coal.Generators.Count; index++)
        {
          var generator = inputReport.Coal.Generators[index];
          var coalDailyEmissions = Calculate(generator, emissionFactor);
          dailyEmissions.AddRange(coalDailyEmissions);
        }
      }

      if (inputReport.Gas != null
        && inputReport.Gas.Generators != null)
      {
        var emissionFactor = _emissionfactorRetriever.Get(GeneratorType.Gas);
        for (var index = 0; index < inputReport.Gas.Generators.Count; index++)
        {
          var generator = inputReport.Gas.Generators[index];
          var gasDailyEmissions = Calculate(generator, emissionFactor);
          dailyEmissions.AddRange(gasDailyEmissions);
        }
      }

      if (!dailyEmissions.Any())
      {
        _logger.Warn("No MaxEmissionGenerators for the given input report");
        return null;
      }

      var maxEmissions = new MaxEmissionGenerators
      {
        Day = new List<EmissionDay>(),
      };
      var distinctDates = dailyEmissions.Select(x => x.Date).Distinct().ToList();
      for (var index = 0; index < distinctDates.Count(); index++)
      {
        var date = distinctDates[index];
        var maxEmission = dailyEmissions.Where(x => x.Date == date).OrderBy(x => x.Emission).Last();

        maxEmissions.Day.Add(maxEmission);
      }

      return maxEmissions;
    }

    private IList<EmissionDay> Calculate(IEmissionGenerator generator, double emissionFactor)
    {
      var dailyEmissions = new List<EmissionDay>();
      if (generator.Generation == null)
      {
        _logger.Warn("No Generator passed to calculate Daily Emission");
        return dailyEmissions;
      }

      for (var index = 0; index < generator.Generation.Days.Count; index++)
      {
        var day = generator.Generation.Days[index];
        var dailyEmission = _dailyEmissionCalculator.Calculate(day.Energy, generator.EmissionsRating, emissionFactor);

        var emission = new EmissionDay
        {
          Date = day.Date,
          Name = generator.Name,
          Emission = dailyEmission,
        };

        dailyEmissions.Add(emission);
      }

      return dailyEmissions;
    }

    #endregion Methods

  }
}
