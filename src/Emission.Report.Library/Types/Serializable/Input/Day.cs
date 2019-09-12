
#region

using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Input
{
  [XmlRoot(ElementName = "Day")]
  public class Day
  {
    [XmlElement(ElementName = "Date")]
    public string Date { get; set; }
    [XmlElement(ElementName = "Energy")]
    public double Energy { get; set; }
    [XmlElement(ElementName = "Price")]
    public double Price { get; set; }
  }
}
