
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "Day")]
  public class Day
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Date")]
    public string Date { get; set; }
    [XmlElement(ElementName = "Emission")]
    public double Emission { get; set; }
  }
}
