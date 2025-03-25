using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Globalization;
using AutomationPractice2.PageObjects.BellatrixEcommerce;
using AutomationPractice2.PageObjects.Models;

namespace AutomationPractice2;
[TestFixture]
public class BellatrixEcommerceTests : BaseTestClass
{
    private MainPage mainPage;
    private CartPage cartPage;
    private CheckoutPage checkOutPage;
    private OrderReceivedPage orderReceivedPage;


    [SetUp]
    public void SetUp()
    {
        mainPage = new MainPage(driver, wait);
        cartPage = new CartPage(driver, wait);
        checkOutPage = new CheckoutPage(driver);
        orderReceivedPage = new OrderReceivedPage(driver);
    }

    [Test]
    [Retry(2)]
    [TestCaseSource(typeof(TDM.BellatrixTDM), nameof(TDM.BellatrixTDM.BellatrixData))]
    public void PurchaseRocket_When_UserIsNotLoggedIn(string rocketName, string price, string quantity, string productSubtotal, string cartSubtotal, string vat, string total, string paymentMethod)
    {

        mainPage.GoToMainPage();
        var rocket = mainPage.GetRocket(rocketName);
        rocket.Should().NotBeNull();
        mainPage.AddRocketToShoppingCart(rocket);
        mainPage.OpenCart(rocket);

        CartPageValidations(
            new Dictionary<string, string>()
            {
                { "RocketName", rocketName },
                { "Price", price },
                { "Quantity", quantity },
                { "ProductSubtotal", productSubtotal },
                { "CartSubtotal", cartSubtotal },
                { "VAT", vat },
                { "Total", total }
            }
            );
        cartPage.GoToCheckout();

        wait.Until(x => checkOutPage.H1_Title.Displayed == true);
        CheckoutPageValidations(
            new Dictionary<string, string>()
            {
                { "RocketName", rocketName },
                { "Price", price },
                { "Quantity", quantity },
                { "CartSubtotal", cartSubtotal },
                { "VAT", vat },
                { "Total", total }
            }
            );
        checkOutPage.FillOutform();
        checkOutPage.PlaceOrder();
        wait.Until(x => x.Url.Contains("https://demos.bellatrix.solutions/checkout/order-received"));
        wait.Until(x => orderReceivedPage.H1_Title.Displayed == true);

        OrderReceivedValidations(
            new Dictionary<string, string>()
            {
                { "RocketName", rocketName },
                { "Price", price },
                { "Quantity", quantity },
                { "CartSubtotal", cartSubtotal },
                { "VAT", vat },
                { "PaymentMethod", paymentMethod },
                { "Total", total }
            }
            );

        //Verify gmail received
        //driver.Navigate().GoToUrl("https://mail.google.com/mail/u/0/#inbox");
        //driver.FindElements(By.XPath("//span[contains(.,'Your Bellatrix Demos order has been received!')]")).Should().NotBeEmpty();
    }

    private void CartPageValidations(Dictionary<string, string> validations)
    {
        cartPage.Product.Text.Should().Be(validations["RocketName"]);
        cartPage.Price.Text.Should().Contain(validations["Price"]);
        cartPage.Quantity.GetAttribute("value").Should().Be(validations["Quantity"]);
        cartPage.ProductSubtotal.Text.Should().Contain(validations["ProductSubtotal"]);
        cartPage.CartSubtotal.Text.Should().Contain(validations["CartSubtotal"]);
        cartPage.VAT.Text.Should().Contain(validations["VAT"]);
        cartPage.Total.Text.Should().Contain(validations["Total"]);
    }

    private void CheckoutPageValidations(Dictionary<string, string> validations)
    {
        checkOutPage.Product.Text.Replace(" ", "").Should().Contain($"{validations["RocketName"]}×{validations["Quantity"]}".Replace(" ", ""));
        checkOutPage.Price.Text.Should().Contain(validations["Price"]);
        checkOutPage.CartSubtotal.Text.Should().Contain(validations["CartSubtotal"]);
        checkOutPage.Tax.Text.Should().Contain(validations["VAT"]);
        checkOutPage.Total.Text.Should().Contain(validations["Total"]);
    }

    private void OrderReceivedValidations(Dictionary<string, string> validations)
    {
        int.TryParse(orderReceivedPage.OrderNumber.Text, out int result).Should().BeTrue();
        result.Should().BeGreaterThan(0);

        string date = DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-EN"));
        //orderReceivedPage.OrderDate.Text.Should().Be(date);
        orderReceivedPage.PaymentMethod.Text.Should().Be($"{validations["PaymentMethod"]}");
        orderReceivedPage.Product.Text.Replace(" ", "").Should().Contain($"{validations["RocketName"]} × {validations["Quantity"]}".Replace(" ", ""));
        orderReceivedPage.Price.Text.Should().Contain(validations["Price"]);
        orderReceivedPage.ProductSubtotal.Text.Should().Contain(validations["CartSubtotal"]);
        //orderReceivedPage.VAT.Text.Should().Contain(validations["VAT"]); BUG
        orderReceivedPage.PaymentMethod_2.Text.Should().Contain(validations["PaymentMethod"]);
        //orderReceivedPage.Total.Text.Should().Contain(validations["Total"]); BUG
    }
}
