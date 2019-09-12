
#region

using System.ServiceModel;
using System.ServiceModel.Channels;

using System.ServiceModel.Description;

using System.ServiceModel.Web;

#endregion

namespace Emission.Report.Library.Service
{
  [ServiceContract]
  public interface IReportService
  {
    bool Start();
    bool Stop();

    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "ping")]
    [OperationContract(Name = "Ping")]
    string Ping();
    
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, 
      UriTemplate = "process?format={format}&path={path}")]
    [OperationContract(Name = "Process")]
    string Process(string format, string path);
  }
}
