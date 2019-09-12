
#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "Coal")]
  public class Coal
  {
    [XmlElement(ElementName = "CoalGenerator")]
    public List<CoalGenerator> Generators { get; set; }
  }
}
