
#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "GenerationOutput")]
  public class GenerationOutput : IOutputData
  {
    [XmlElement(ElementName = "Totals")]
    public Totals Totals { get; set; }
    [XmlElement(ElementName = "MaxEmissionGenerators")]
    public MaxEmissionGenerators MaxEmissionGenerators { get; set; }
    [XmlElement(ElementName = "ActualHeatRates")]
    public List<ActualHeatRates> ActualHeatRates { get; set; }
  }
}
