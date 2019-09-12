
using Emission.Report.Library.Enums;

namespace Emission.Report.Library.Calculate.Emissions
{
  public interface IEmissionFactorRetriever 
  {
    double Get(GeneratorType generatorType);
  }
}
