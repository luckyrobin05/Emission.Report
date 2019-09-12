
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Reference
{
  [XmlRoot(ElementName = "Factors")]
  public class Factors
  {
    [XmlElement(ElementName = "ValueFactor")]
    public ValueFactor ValueFactor { get; set; }
    [XmlElement(ElementName = "EmissionsFactor")]
    public EmissionsFactor EmissionsFactor { get; set; }
  }
}
