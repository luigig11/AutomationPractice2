using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using Proxy = Titanium.Web.Proxy.Http;

namespace AutomationPractice2;

public class BaseTestClass
{
    protected IWebDriver driver;
    protected WebDriverWait wait;
    protected ProxyServer _proxyServer;
    protected readonly IDictionary<int, Proxy.Request> _requestsHistory = new ConcurrentDictionary<int, Proxy.Request>();
    protected readonly IDictionary<int, Proxy.Response> _responsesHistory = new ConcurrentDictionary<int, Proxy.Response>();

    [OneTimeSetUp]
    public void ClassInit()
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        StartProxyServer();
    }

    [SetUp]
    public void Setup()
    {
        //new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        var proxy = new OpenQA.Selenium.Proxy
        {
            HttpProxy = "http://localhost:18882",
            SslProxy = "http://localhost:18882",
            FtpProxy = "http://localhost:18882"
        };
        var options = new ChromeOptions();
        options.AddArgument(@"user-data-sir=C:\Users\PERSONAL\AppData\Local\Google\Chrome\User Data\Default");
        options.Proxy = proxy;
        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
        _requestsHistory.Clear();
        _responsesHistory.Clear();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _proxyServer.Stop();
        _proxyServer.Dispose();
        driver.Quit();
    }

    private void StartProxyServer()
    {
        _proxyServer = new ProxyServer();
        var explicitEndpoint = new ExplicitProxyEndPoint(System.Net.IPAddress.Any, 18882, true);
        _proxyServer.AddEndPoint(explicitEndpoint);
        _proxyServer.Start();
        _proxyServer.SetAsSystemHttpProxy(explicitEndpoint);
        _proxyServer.SetAsSystemHttpsProxy(explicitEndpoint);
        _proxyServer.BeforeRequest += OnRequestCaptureTrafficEventHandler;
        _proxyServer.BeforeResponse += OnResponseCaptureTrafficEventHandler;
    }

    private async Task OnRequestCaptureTrafficEventHandler(object sender, SessionEventArgs e)
    {
        await Task.Run(() =>
        {
            if (!_requestsHistory.ContainsKey(e.HttpClient.Request.GetHashCode()) && e.HttpClient != null && e.HttpClient.Request != null)
            {
                _requestsHistory.Add(e.HttpClient.Request.GetHashCode(), e.HttpClient.Request);
            }
        });
    }

    private async Task OnResponseCaptureTrafficEventHandler(object sender, SessionEventArgs e)
    {
        await Task.Run(() =>
        {
            if (!_responsesHistory.ContainsKey(e.HttpClient.Response.GetHashCode()) && e.HttpClient != null && e.HttpClient.Response != null)
            {
                _responsesHistory.Add(e.HttpClient.Response.GetHashCode(), e.HttpClient.Response);
            }
        });
    }
}
