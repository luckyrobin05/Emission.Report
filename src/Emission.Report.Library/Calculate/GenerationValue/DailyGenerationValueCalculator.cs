
#region

#endregion

namespace Emission.Report.Library.Calculate.GenerationValue
{
  public class DailyGenerationValueCalculator : IDailyGenerationValueCalculator
  {

    #region Fields

    #endregion Fields

    #region Constructor

    #endregion Constructor

    #region Methods

    public double Calculate(double energy, double price, double valueFactor)
    {
      return energy * price * valueFactor;
    }

    #endregion Methods

  }
}
