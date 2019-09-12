
#region

using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "CoalGenerator")]
  public class CoalGenerator : IEmissionGenerator
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Generation")]
    public Generation Generation { get; set; }
    [XmlElement(ElementName = "TotalHeatInput")]
    public double TotalHeatInput { get; set; }
    [XmlElement(ElementName = "ActualNetGeneration")]
    public double ActualNetGeneration { get; set; }
    [XmlElement(ElementName = "EmissionsRating")]
    public double EmissionsRating { get; set; }
  }
}
