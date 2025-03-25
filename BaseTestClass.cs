using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AutomationPractice2;

public class BaseTestClass
{
    protected IWebDriver driver;
    protected WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        var options = new ChromeOptions();
        options.AddArgument(@"user-data-sir=C:\Users\PERSONAL\AppData\Local\Google\Chrome\User Data\Default");
        driver = new ChromeDriver(options);
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        driver.Quit();
    }
}
