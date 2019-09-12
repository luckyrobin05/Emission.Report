
#region 

using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;

#endregion

namespace Emission.Report.Library.Calculate.GenerationValue
{
  public class ValueFactorRetriever : IValueFactorRetriever
  {

    #region Fields

    private readonly ILogger _logger;
    private readonly IConfigSettings _configSettings;

    private const string OFFSHORE = "OFFSHORE";
    private const string ONSHORE = "ONSHORE";

    #endregion Fields

    #region Constructor

    public ValueFactorRetriever(
      ILoggerCreator loggerCreator,
      IConfigSettings configSettings)
    {
      _logger = loggerCreator.GetTypeLogger<ValueFactorRetriever>();
      _configSettings = configSettings;
    }

    #endregion Constructor

    #region Methods

    public double Get(string location)
    {
      location = string.IsNullOrWhiteSpace(location) ? string.Empty : location.Trim().ToUpper();

      switch (location)
      {
        case OFFSHORE:
          return _configSettings.CurrentReferenceData.Factors.ValueFactor.Low;

        case ONSHORE:
          return _configSettings.CurrentReferenceData.Factors.ValueFactor.High;

        default:
          return _configSettings.CurrentReferenceData.Factors.ValueFactor.Medium;
      }
    }

    #endregion Methods

  }
}
