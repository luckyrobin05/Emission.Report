
namespace Emission.Report.Library.Calculate.ActualHeatRate
{
  public interface IActualHeatRateCalculator
  {
    double Calculate(double totalHeatInput, double actualNetGeneration);
  }
}
