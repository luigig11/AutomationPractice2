using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using Response = Titanium.Web.Proxy.Http.Response;

namespace AutomationPractice2.TestUtilities;

public class ProxyService : IDisposable
{
    private readonly ConcurrentDictionary<string, string> _redirectUrls;
    private readonly ConcurrentBag<string> _blockUrls;

    public ProxyService()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            IsEnabled = false;
        }

        if(IsEnabled)
        {
            ProxyServer = new ProxyServer(false);
        }
        _redirectUrls = new ConcurrentDictionary<string, string>();
        _blockUrls = new ConcurrentBag<string>();
        RequestsHistory = new ConcurrentDictionary<int, MeasuredRequest>();
        ResponsesHistory = new ConcurrentDictionary<int, Response>();
    }

    public ProxyServer ProxyServer { get; set; }

    public ConcurrentDictionary<int, MeasuredRequest> RequestsHistory { get; set; }

    public ConcurrentDictionary<int, Response> ResponsesHistory { get; set; }

    public bool IsEnabled { get; set; } = true;

    public int Port { get; set; }

    public void Dispose()
    {
        if (ProxyServer != null && ProxyServer.ProxyRunning)
        {
            ProxyServer?.Stop();
        }

        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        if (IsEnabled)
        {
            Port = GetFreeTcpPort();
            var explicitEndpoint = new ExplicitProxyEndPoint(IPAddress.Any, Port);
            ProxyServer.AddEndPoint(explicitEndpoint);
            ProxyServer.Start();
            //suscriber oyentes a eventos
            ProxyServer.BeforeRequest += OnRequestCaptureTrafficEventHandler;
            ProxyServer.BeforeResponse += OnResponseCaptureTrafficEventHandler;
            ProxyServer.BeforeRequest += OnRequestBlockResourceEventHandler;
            ProxyServer.BeforeRequest += OnRequestRedirectTrafficEventHandler;
            ProxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            ProxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;
        }
    }

    private async Task OnRequestCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
    {
        if (!RequestsHistory.ContainsKey(e.HttpClient.Request.GetHashCode()) && e.HttpClient != null && e.HttpClient.Request != null)
        {
            var measuredRequest = new MeasuredRequest(DateTime.Now, e.HttpClient.Request);
            RequestsHistory.GetOrAdd(e.HttpClient.Request.GetHashCode(), measuredRequest);
        }
    }).ConfigureAwait(false);

    private async Task OnResponseCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
    {
        if (!ResponsesHistory.ContainsKey(e.HttpClient.Response.GetHashCode()) && e.HttpClient?.Response != null)
        {
            ResponsesHistory.GetOrAdd(e.HttpClient.Response.GetHashCode(), e.HttpClient.Response);
        }
    }).ConfigureAwait(false);

    private async Task OnRequestBlockResourceEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
    {
        if (!_blockUrls.IsEmpty)
        {
            foreach (var urlToBeBlocked in _blockUrls)
            {
                if (e.HttpClient.Request.RequestUri.ToString().Contains(urlToBeBlocked))
                {
                    var customBody = string.Empty;
                    e.Ok(Encoding.UTF8.GetBytes(customBody));
                }
            }
        }
    }).ConfigureAwait(false);

    private async Task OnRequestRedirectTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
    {
        if (_redirectUrls.Keys.Count > 0)
        {
            foreach (var redirectUrlPair in _redirectUrls)
            {
                if (e.HttpClient.Request.RequestUri.AbsoluteUri.Contains(redirectUrlPair.Key))
                {
                    e.Redirect(redirectUrlPair.Value);
                }
            }
        }
    }).ConfigureAwait(false);

    private Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
    {
        if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
        {
            e.IsValid = true;
        }

        return Task.FromResult(0);
    }

    private Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
    {
        return Task.FromResult(0);
    }

    private int GetFreeTcpPort()
    {
        Thread.Sleep(100);
        var l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        int port = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return port;
    }



}
