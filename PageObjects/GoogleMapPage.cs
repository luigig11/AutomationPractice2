using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AutomationPractice2.PageObjects;

public class GoogleMapPage
{
    private IWebDriver driver;

    public GoogleMapPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    #region Selectors
    public string GoogleMapUrl = "https://www.google.com/maps";
    public IWebElement sceneDiv => driver.FindElement(By.Id("scene"));
    public IWebElement searchInput => driver.FindElement(By.Id("searchboxinput"));
    public IWebElement searchButton => driver.FindElement(By.Id("searchbox-searchbutton"));
    public IWebElement coodrinatesHeader => driver.FindElement(By.ClassName("lMbq3e"));
    #endregion

    public void GoToGoogleMapPage(string url)
    {
        driver.Navigate().GoToUrl(url);
    }

    public void SearchLocation(string location)
    {
        searchInput.SendKeys(location);
        searchButton.Click();
    }

    public string GenerateGoogleMapLinkByCoordinates(string latitude, string longitude)
    {
        string googleMapLink = "https://www.google.com/maps/search/?api=1&query=";
        return $"{googleMapLink}{latitude}%2C{longitude}";

    }
}