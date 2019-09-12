
#region

using System;

#endregion

namespace Emission.Report.Library.Calculate.ActualHeatRate
{
  public class CoalActualHeatRateCalculator : IActualHeatRateCalculator
  {

    #region Fields

    #endregion Fields

    #region Constructor

    #endregion Constructor

    #region Methods

    public double Calculate(double totalHeatInput, double actualNetGeneration)
    {
      if (actualNetGeneration == 0)
      {
        throw new DivideByZeroException("ActualNetGeneration is zero. Cannot calculate ActualHeatRate");
      }

      return totalHeatInput / actualNetGeneration;
    }

    #endregion Methods

  }
}
