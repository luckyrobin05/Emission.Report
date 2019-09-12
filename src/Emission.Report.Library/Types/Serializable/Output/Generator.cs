
#region

using Emission.Report.Library.Types.Serializable.Output.Interfaces;
using System.Xml.Serialization;

#endregion

namespace Emission.Report.Library.Types.Serializable.Output
{
  [XmlRoot(ElementName = "Generator")]
  public class Generator : IOutputGenerator
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Total")]
    public double Total { get; set; }
  }
}
