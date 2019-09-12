
#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "MaxEmissionGenerators")]
  public class MaxEmissionGenerators
  {
    [XmlElement(ElementName = "Day")]
    public List<Day> Day { get; set; }
  }
}
