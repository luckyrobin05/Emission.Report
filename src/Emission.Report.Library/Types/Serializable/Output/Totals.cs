
#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "Totals")]
  public class Totals
  {
    [XmlElement(ElementName = "Generator")]
    public List<Generator> Generator { get; set; }
  }
}
