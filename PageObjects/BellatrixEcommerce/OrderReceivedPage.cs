using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.BellatrixEcommerce;

public class OrderReceivedPage
{
    private IWebDriver _driver;

    public OrderReceivedPage(IWebDriver driver)
    {
        _driver = driver;
    }

    #region Selectors
    public IWebElement H1_Title => _driver.FindElement(By.XPath("//h1[contains(.,'Order received')]"));
    public IWebElement OrderNumber => _driver.FindElement(By.XPath("//ul/li[contains(.,'Order number')]/strong"));
    public IWebElement OrderDate => _driver.FindElement(By.XPath("//ul/li[contains(.,'Date')]/strong"));
    public IWebElement PaymentMethod => _driver.FindElement(By.XPath("//ul/li[contains(.,'Payment method')]/strong"));
    public IWebElement Product => _driver.FindElement(By.XPath("//table/tbody/tr/td[1]"));
    public IWebElement Price => _driver.FindElement(By.XPath("//table/tbody/tr/td[2]"));
    public IWebElement ProductSubtotal => _driver.FindElement(By.XPath("//table/tfoot/tr[1]/td"));
    public IWebElement VAT => _driver.FindElement(By.XPath("//table/tfoot/tr[2]/td"));
    public IWebElement PaymentMethod_2 => _driver.FindElement(By.XPath("//table/tfoot/tr[3]/td"));
    public IWebElement Total => _driver.FindElement(By.XPath("//table/tfoot/tr[4]/td"));
    #endregion
}