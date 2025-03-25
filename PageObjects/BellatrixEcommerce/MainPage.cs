using AutomationPractice2.PageObjects.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.BellatrixEcommerce;

public class MainPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    #region Selectors
    private string MainUrl = "https://demos.bellatrix.solutions/";
    public ReadOnlyCollection<IWebElement> rocketsList => _driver.FindElements(By.XPath("//ul[@class='products columns-4']/li"));
    public IWebElement H1_Title => _driver.FindElement(By.XPath("//h1[contains(.,'Shop')]"));
    #endregion

    public MainPage(IWebDriver driver, WebDriverWait wait)
    {
        _driver = driver;
        _wait = wait;
    }

    public void GoToMainPage()
    {
        _driver.Navigate().GoToUrl(MainUrl);
        _wait.Until(x => H1_Title.Displayed == true);
    }
    public IWebElement? GetRocket(string rocket)
    {
        var rocketElement = rocketsList.FirstOrDefault(x => x.FindElement(By.TagName("h2")).Text == rocket);
        return rocketElement;
    }
    public void AddRocketToShoppingCart(IWebElement element)
    {
        element.FindElements(By.TagName("a"))[1].Click();
        //_wait.Until(x => x.FindElement(By.XPath("//a[@title='View cart']")));
        _wait.Until(x => element.FindElements(By.TagName("a"))[1].GetAttribute("class") == "button product_type_simple add_to_cart_button ajax_add_to_cart added");
        _wait.Until(x => element.FindElement(By.XPath("//a[@title='View cart']")).Displayed == true);  
    }

    public void OpenCart(IWebElement? element)
    {
        if (element is null)
        {
            _driver.FindElement(By.CssSelector(".cart-contents")).Click();
            _wait.Until(x => x.FindElement(By.ClassName("entry-title")));
        }
        else
        {
            element.FindElement(By.XPath("//a[@title='View cart']")).Click();
            _wait.Until(x => x.FindElement(By.ClassName("entry-title")));
        }

    }


}
