
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Reference
{
  [XmlRoot(ElementName = "ReferenceData")]
  public class ReferenceData : IInputData
  {
    [XmlElement(ElementName = "Factors")]
    public Factors Factors { get; set; }
  }
}
