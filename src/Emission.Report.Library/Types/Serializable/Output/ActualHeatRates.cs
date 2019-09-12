
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "ActualHeatRates")]
  public class ActualHeatRates
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "HeatRate")]
    public string HeatRate { get; set; }
  }
}
