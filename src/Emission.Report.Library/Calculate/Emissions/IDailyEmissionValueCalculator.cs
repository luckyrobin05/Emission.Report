
namespace Emission.Report.Library.Calculate.Emissions
{
  public interface IDailyEmissionValueCalculator
  {
    double Calculate(double energy, double emissionsRating, double emissionFactor);
  }
}
