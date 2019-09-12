
#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "Gas")]
  public class Gas
  {
    [XmlElement(ElementName = "GasGenerator")]
    public List<GasGenerator> Generators { get; set; }
  }
}
