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
using AutomationPractice2.TestUtilities;

namespace AutomationPractice2;

public class BaseTestClass
{
    protected IWebDriver driver;
    protected WebDriverWait wait;
    protected ProxyService _proxyService;

    [OneTimeSetUp]
    public void ClassInit()
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        _proxyService = new ProxyService();
        _proxyService.Start();
    }

    [SetUp]
    public void Setup()
    {
        //new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        var proxy = new OpenQA.Selenium.Proxy
        {
            HttpProxy = $"http://localhost:{_proxyService.Port}",
            SslProxy = $"http://localhost:{_proxyService.Port}",
            FtpProxy = $"http://localhost:{_proxyService.Port}"
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
        _proxyService.RequestsHistory.Clear();
        _proxyService.ResponsesHistory.Clear();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _proxyService.Dispose();
        driver.Quit();
    }
}
