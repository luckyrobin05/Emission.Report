
#region

using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using Emission.Report.Library.Types.Serializable.Output.Interfaces;

#endregion

namespace Emission.Report.Library.Calculate.GenerationValue
{
  public interface ITotalGenerationValueCalculator
  {
    IOutputGenerator GetTotalGenerationValue(IInputGenerator generator, double valueFactor);
  }
}
