
#region

using System.Xml.Serialization;
using System.Collections.Generic;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "Generation")]
  public class Generation
  {
    [XmlElement(ElementName = "Day")]
    public List<Day> Days { get; set; }
  }
}
