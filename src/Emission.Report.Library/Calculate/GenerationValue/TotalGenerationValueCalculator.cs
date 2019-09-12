
#region

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;
using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using Emission.Report.Library.Types.Serializable.Output;
using Emission.Report.Library.Types.Serializable.Output.Interfaces;

#endregion

namespace Emission.Report.Library.Calculate.GenerationValue
{
  public class TotalGenerationValueCalculator : ITotalGenerationValueCalculator
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IDailyGenerationValueCalculator _calculator;

    #endregion Fields

    #region Constructor

    public TotalGenerationValueCalculator(
      ILoggerCreator loggerCreator,
      IDailyGenerationValueCalculator calculator
      )
    {
      _logger = loggerCreator.GetTypeLogger<TotalGenerationValueCalculator>();
      _calculator = calculator;
    }

    #endregion Constructor

    #region Methods

    public IOutputGenerator GetTotalGenerationValue(IInputGenerator generator, double valueFactor)
    {
      var totalGenerationValue = 0d;

      for (var index = 0; index < generator.Generation.Days.Count; index++)
      {
        var dailyGeneration = generator.Generation.Days[index];
        var dailyGenerationValue = _calculator.Calculate(dailyGeneration.Energy, dailyGeneration.Price, valueFactor);

        totalGenerationValue += dailyGenerationValue;
      }

      return new Generator
      {
        Name = generator.Name,
        Total = totalGenerationValue,
      };
    }

    #endregion Methods

  }
}
