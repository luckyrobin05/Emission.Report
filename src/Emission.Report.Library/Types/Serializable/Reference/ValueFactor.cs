
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Reference
{
  [XmlRoot(ElementName = "ValueFactor")]
  public class ValueFactor
  {
    [XmlElement(ElementName = "High")]
    public double High { get; set; }
    [XmlElement(ElementName = "Medium")]
    public double Medium { get; set; }
    [XmlElement(ElementName = "Low")]
    public double Low { get; set; }
  }
}
