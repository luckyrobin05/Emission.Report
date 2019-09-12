
#region

using System;
using System.Linq;
using System.Collections.Generic;

using Emission.Report.Common.Logging;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;

using Emission.Report.Library.Calculate.ActualHeatRate;

#endregion

namespace Emission.Report.Library.Output
{
  public class ActualHeatRatesOutput : IActualHeatRatesOutput
  {

    #region Fields
    
    private readonly ILogger _logger;
    private readonly IActualHeatRateCalculator _actualHeatRateCalculator;
    
    #endregion Fields

    #region Constructor

    public ActualHeatRatesOutput(
      ILoggerCreator loggerCreator,
      IActualHeatRateCalculator actualHeatRateCalculator
      )
    {
      _logger = loggerCreator.GetTypeLogger<ActualHeatRatesOutput>();
      _actualHeatRateCalculator = actualHeatRateCalculator;
    }

    #endregion Constructor

    #region Methods
    
    public IList<ActualHeatRates> Get(GenerationReport inputReport)
    {
      if (inputReport == null)
      {
        _logger.Warn("No report to retrieve ActualHeatRates");
        return null;
      }

      var actualHeatRates = new List<ActualHeatRates>();

      if (inputReport.Coal != null
        && inputReport.Coal.Generators != null)
      {
        for (var index = 0; index < inputReport.Coal.Generators.Count; index++)
        {
          var generator = inputReport.Coal.Generators[index];
          var actualHeatRate = Calculate(generator);
          if (actualHeatRate != null)
            actualHeatRates.Add(actualHeatRate);
        }
      }

      if (!actualHeatRates.Any())
      {
        return null;
      }

      return actualHeatRates;
    }

    private ActualHeatRates Calculate(CoalGenerator coalGenerator)
    {
      var heatRate = string.Empty;
      
      try
      {
        heatRate = _actualHeatRateCalculator.Calculate(coalGenerator.TotalHeatInput, coalGenerator.ActualNetGeneration).ToString();
      }
      catch (DivideByZeroException ex)
      {
        _logger.Error(string.Format("ActualNetGeneration value given as zero for CoalGenerator: {0}. Cannot calculate HeatRate", coalGenerator.Name), ex);
        heatRate = "Cannot calculate. Actual Net Generation is 0";
      }

      return new ActualHeatRates
      {
        Name = coalGenerator.Name,
        HeatRate = heatRate,
      };
    }

    #endregion Methods

  }
}
