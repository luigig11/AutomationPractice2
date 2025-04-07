using Titanium.Web.Proxy.Http;

namespace AutomationPractice2.TestUtilities;

public class MeasuredRequest
{
    public MeasuredRequest(DateTime creationTime, Request request)
    {
        CreationTime = creationTime;
        Request = request;
    }

    public DateTime CreationTime { get; set; }
    public Request Request { get; set; }
}