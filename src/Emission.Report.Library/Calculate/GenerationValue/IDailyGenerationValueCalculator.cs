
using System.Collections.Generic;
using Emission.Report.Library.Types.Serializable.Input;

namespace Emission.Report.Library.Calculate.GenerationValue
{
  public interface IDailyGenerationValueCalculator
  {
    double Calculate(double energy, double price, double valueFactor);
  }
}
