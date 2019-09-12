
#region

using Emission.Report.Library.Types.Serializable.Input.Interfaces;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "GasGenerator")]
  public class GasGenerator : IEmissionGenerator
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Generation")]
    public Generation Generation { get; set; }
    [XmlElement(ElementName = "EmissionsRating")]
    public double EmissionsRating { get; set; }
  }
}
