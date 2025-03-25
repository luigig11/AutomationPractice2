using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.BellatrixEcommerce;

public class CheckoutPage
{
    private IWebDriver _driver;

    public CheckoutPage(IWebDriver driver)
    {
        _driver = driver;
    }

    #region Selectors
    public IWebElement H1_Title => _driver.FindElement(By.XPath("//h1[contains(.,'Checkout')]"));
    public IWebElement FirstName => _driver.FindElement(By.Id("billing_first_name"));
    public IWebElement LastName => _driver.FindElement(By.Id("billing_last_name"));
    public IWebElement CountryContainer => _driver.FindElement(By.Id("select2-billing_country-container"));
    public IWebElement CountrySearch => _driver.FindElement(By.ClassName("select2-search__field"));
    public IWebElement Address1 => _driver.FindElement(By.Id("billing_address_1"));
    public IWebElement City => _driver.FindElement(By.Id("billing_city"));
    public IWebElement StateContainer => _driver.FindElement(By.Id("select2-billing_state-container"));
    public IWebElement StateSearch => _driver.FindElement(By.ClassName("select2-search__field"));
    public IWebElement Zip => _driver.FindElement(By.Id("billing_postcode"));
    public IWebElement Phone => _driver.FindElement(By.Id("billing_phone"));
    public IWebElement Email => _driver.FindElement(By.Id("billing_email"));
    public IWebElement PlaceOrderButton => _driver.FindElement(By.Id("place_order"));
    public IWebElement Product => _driver.FindElement(By.XPath("//tr[@class='cart_item']/td[1]"));
    public IWebElement Price => _driver.FindElement(By.XPath("//tr[@class='cart_item']/td[2]"));
    public IWebElement CartSubtotal => _driver.FindElement(By.XPath("//tr[@class='cart-subtotal']/td[1]"));
    public IWebElement Tax => _driver.FindElement(By.XPath("//tr[@class='tax-total']/td[1]"));
    public IWebElement Total => _driver.FindElement(By.XPath("//tr[@class='order-total']/td[1]"));

    public void FillOutform()
    {
        FirstName.SendKeys("Luis");
        LastName.SendKeys("Garcia");

        Actions actionProvider = new Actions(_driver);
        CountryContainer.Click();
        CountrySearch.SendKeys("Nicaragua");
        actionProvider.KeyDown(Keys.Enter).KeyUp(Keys.Enter).Perform();

        Address1.SendKeys("Calle 1");
        City.SendKeys("Managua");

        StateContainer.Click();
        StateSearch.Click();
        actionProvider.SendKeys("Jinotega").KeyDown(Keys.Enter).KeyUp(Keys.Enter).Perform();

        Zip.SendKeys("11062");
        Phone.SendKeys("88888888");
        Email.SendKeys("wetan77179@envoes.com");
    }

    public void PlaceOrder()
    {
        PlaceOrderButton.Click();
    }
    #endregion


}