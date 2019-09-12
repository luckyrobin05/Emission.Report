
#region

#endregion

namespace Emission.Report.Library.Calculate.Emissions
{
  public class DailyEmissionValueCalculator : IDailyEmissionValueCalculator
  {

    #region Fields

    #endregion Fields

    #region Constructor

    #endregion Constructor

    #region Methods

    public double Calculate(double energy, double emissionsRating, double emissionFactor)
    {
      return energy * emissionsRating * emissionFactor;
    }

    #endregion Constructor

  }
}
