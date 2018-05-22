using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Driver.WebSite.Source.Security
{
    public class MaliciousRequestsDetector
    {
        private readonly string[] UnsupportedHttpMethods = 
        {
            "PATCH",
            "PUT",
            "DELETE",
            "TRACE",
            "OPTIONS",
            "CONNECT"
        };

        private readonly AppSensorClient _appSensor;

        public MaliciousRequestsDetector(AppSensorClient appSensor)
        {
            _appSensor = appSensor;
        }

        public async Task InspectRequest(HttpRequest request)
        {
            if (UnsupportedHttpMethods.Contains(request.HttpMethod))
            {
                try
                {
                    await _appSensor.ReportEventAsync(
                        new DetectionPoint
                        {
                            Label = "RE1",
                            Category = "Request"
                        });
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
        }
    }
}