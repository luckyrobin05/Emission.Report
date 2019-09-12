
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "GenerationReport")]
  public class GenerationReport : IInputData
  {
    [XmlElement(ElementName = "Wind")]
    public Wind Wind { get; set; }
    [XmlElement(ElementName = "Gas")]
    public Gas Gas { get; set; }
    [XmlElement(ElementName = "Coal")]
    public Coal Coal { get; set; }
  }
}
