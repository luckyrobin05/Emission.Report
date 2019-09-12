
#region

using System;
using Emission.Report.Common.Logging;
using Emission.Report.Library.Enums;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.Calculate.Emissions
{
  public class EmissionFactorRetriever : IEmissionFactorRetriever
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IConfigSettings _configSettings;

    #endregion Fields

    #region Constructor

    public EmissionFactorRetriever(
      ILoggerCreator loggerCreator,
      IConfigSettings configSettings)
    {
      _logger = loggerCreator.GetTypeLogger<EmissionFactorRetriever>();
      _configSettings = configSettings;
    }

    #endregion Constructor

    #region Methods

    public double Get(GeneratorType generatorType)
    {
      switch (generatorType)
      {
        case GeneratorType.Coal:
          return _configSettings.CurrentReferenceData.Factors.EmissionsFactor.High;

        case GeneratorType.Gas:
          return _configSettings.CurrentReferenceData.Factors.EmissionsFactor.Medium;

        default:
          throw new NotImplementedException(string.Format("EmissionFactor not set for: {0}", generatorType.ToString()));
      }
    }

    #endregion Methods

  }
}
