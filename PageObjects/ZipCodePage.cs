using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationPractice2.PageObjects;

public class ZipCodePage
{
    private IWebDriver driver;

    public ZipCodePage(IWebDriver driver)
    {
        this.driver = driver;
    }

    #region Selectors
    string PageMainURL = "https://www.zip-codes.com/";
    string AdvanceSearchURL = "https://www.zip-codes.com/search.asp?selectTab=3";
    public IWebElement ZipCode => driver.FindElement(By.XPath("//input[@aria-label='fld_zip']"));
    public IWebElement Town_City => driver.FindElement(By.XPath("//input[@aria-label='City']"));
    public IWebElement State => driver.FindElement(By.XPath("(//select[@aria-label='State'])[2]"));
    #endregion
}
