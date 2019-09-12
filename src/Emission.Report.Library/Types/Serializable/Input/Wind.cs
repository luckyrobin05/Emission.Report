
#region

using System.Xml.Serialization;
using System.Collections.Generic;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "Wind")]
  public class Wind
  {
    [XmlElement(ElementName = "WindGenerator")]
    public List<WindGenerator> Generators { get; set; }
  }
}
