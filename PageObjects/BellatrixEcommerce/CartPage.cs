using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.BellatrixEcommerce;

public class CartPage
{
    private IWebDriver _driver;
    private WebDriverWait _wait;

    public CartPage(IWebDriver driver, WebDriverWait wait)
    {
        _driver = driver;
        _wait = wait;
    }

    #region Selector
    public IWebElement H1_Title => _driver.FindElement(By.XPath("//h1[contains(.,'Cart')]"));
    public IWebElement Product => _driver.FindElement(By.XPath("//td[@data-title='Product']"));
    public IWebElement Price => _driver.FindElement(By.XPath("//td[@data-title='Price']"));
    public IWebElement Quantity => _driver.FindElement(By.XPath("//td[@data-title='Quantity']/div//input"));
    public IWebElement ProductSubtotal => _driver.FindElement(By.XPath("//td[@class='product-subtotal'][@data-title='Subtotal']"));
    public IWebElement CartSubtotal => _driver.FindElement(By.XPath("//tr[@class='cart-subtotal']/td[@data-title='Subtotal']"));
    public IWebElement VAT => _driver.FindElement(By.XPath("//td[@data-title='VAT']"));
    public IWebElement Total => _driver.FindElement(By.XPath("//td[@data-title='Total']"));
    public IWebElement ProceedToCheckoutButton => _driver.FindElement(By.XPath("//a[contains(.,'Proceed to checkout')]"));
    public IWebElement CartUpdatedAlert => _driver.FindElement(By.XPath("//div[contains(.,'Cart updated')]"));

    public void GoToCheckout()
    {
        ProceedToCheckoutButton.Click();
    }

    public void SetQuantity(string quantity, string productSubtotal)
    {
        Quantity.Clear();
        Quantity.SendKeys(quantity);
        Quantity.SendKeys(Keys.Enter);
        _wait.Until(x => ProductSubtotal.Text.Contains(productSubtotal) == true);
        //_wait.Until(x => CartUpdatedAlert.Displayed == true);
    }
    #endregion
}
