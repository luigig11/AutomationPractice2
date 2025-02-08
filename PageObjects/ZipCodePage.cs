using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using AutomationPractice2.PageObjects.Models;
using AutomationPractice2.TestUtilities;
using System.Collections.ObjectModel;

namespace AutomationPractice2.PageObjects;

public class ZipCodePage
{
    private IWebDriver driver;
    private GoogleMapPage googleMapPage;
    private Func<IWebDriver, bool> Func;

    public ZipCodePage(IWebDriver driver)
    {
        this.driver = driver;
        Func = IsElementDisplayed;
    }

    #region Selectors
    string PageMainURL = "https://www.zip-codes.com/";
    string AdvanceSearchURL = "https://www.zip-codes.com/search.asp?selectTab=3";
    List<ZipCodeSearchResults> zipCodeSearchResults = new List<ZipCodeSearchResults>();
    public IWebElement zipCodeInput => driver.FindElement(By.XPath("//input[@aria-label='fld_zip']"));
    public IWebElement town_CityInput => driver.FindElement(By.XPath("//input[@aria-label='City'][@class='form-control'][@placeholder='City']"));
    public IWebElement stateSelect => driver.FindElement(By.XPath("(//select[@aria-label='State'])[2]"));
    public IWebElement countyInput => driver.FindElement(By.XPath("(//input[@aria-label='County'])[2]"));
    public IWebElement areaCodeInput => driver.FindElement(By.XPath("(//input[@aria-label='Area Code'])[2]"));
    public IWebElement searchButton => driver.FindElement(RelativeBy.WithLocator(By.TagName("input")).Below(areaCodeInput));
    public IWebElement zipTable => driver.FindElement(By.Id("tblZIP"));
    public IWebElement infoDiv => driver.FindElement(By.Id("info"));
    public IWebElement zipCodeTableInfo => infoDiv.FindElement(By.TagName("table"));

    #endregion

    public void GoToZipCodePage()
    {
        driver.Navigate().GoToUrl(PageMainURL);
    }

    public void GoToAdvanceSearchPage()
    {
        driver.Navigate().GoToUrl(AdvanceSearchURL);
    }

    public void SearchZipCodes(AdvanceSearchForm formParameters)
    {
        zipCodeInput.SendKeys(formParameters.ZipCode);
        town_CityInput.SendKeys(formParameters.Town_City);
        SelectElement select = new SelectElement(stateSelect);
        select.SelectByText(formParameters.State);
        countyInput.SendKeys(formParameters.County);
        areaCodeInput.SendKeys(formParameters.AreaCode);
        searchButton.Click();
    }

    public void SearchZipCodeByTown(string town)
    {
        town_CityInput.SendKeys(town);
        searchButton.Click();
    }

    public void SaveInfo(int numberOfRows)
    {
        List<IWebElement> rows = GetNthElementsFromZipTable(numberOfRows);
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        foreach (var row in rows)
        {
            try
            {
                ZipCodeSearchResults zipCodeSearchResults = new ZipCodeSearchResults();
                zipCodeSearchResults.ZipCode = row.FindElements(By.TagName("td"))[0].Text; //esta fila corresponde a la primea fila, la de los encabezados. por eso da error
                zipCodeSearchResults.City = row.FindElements(By.TagName("td"))[1].Text;
                zipCodeSearchResults.State = row.FindElements(By.TagName("td"))[2].Text;
                zipCodeSearchResults.ZipCodeUrl = row.FindElements(By.TagName("td"))[0].FindElement(By.TagName("a")).GetAttribute("href");
                string coordinates = GetCoordinates(driver, zipCodeSearchResults.ZipCodeUrl);
                if (string.IsNullOrEmpty(coordinates))
                {
                    continue;
                }

                var newWindow = driver.SwitchTo().NewWindow(WindowType.Tab);
                ReadOnlyCollection<string> windowHandles = driver.WindowHandles;
                string mainTab = windowHandles.First();
                googleMapPage = new GoogleMapPage(newWindow);
                googleMapPage.GoToGoogleMapPage();
                wait.Until(x => googleMapPage.sceneDiv.Displayed == true);
                googleMapPage.SearchLocation(coordinates);
                wait.Until(x => googleMapPage.coodrinatesHeader.Displayed == true);
                Utilities.TakeFullScreenShot(driver, $"{zipCodeSearchResults.City}-{zipCodeSearchResults.State}-{zipCodeSearchResults.ZipCode}.jpg");
                newWindow.Close();
                driver.SwitchTo().Window(mainTab);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }

    private string GetCoordinates(IWebDriver driver, string url)
    {
        var newWindow = driver.SwitchTo().NewWindow(WindowType.Tab);
        ReadOnlyCollection<string> windowHandles = driver.WindowHandles;
        string firstTab = windowHandles.First();
        //string lastTab = windowHandles.Last();
        WebDriverWait wait = new WebDriverWait(newWindow, TimeSpan.FromSeconds(10));
        newWindow.Navigate().GoToUrl(url);
        wait.Until(Func);
        if (newWindow.FindElements(By.XPath("//h1[contains(., 'Error on Zip-Codes.com')]")).Count > 0)
        {
            newWindow.Close();
            driver.SwitchTo().Window(firstTab);
            return "";
        }
        IWebElement coordinatesRow = zipCodeTableInfo.FindElements(By.TagName("tr"))[8];
        string coordinates = coordinatesRow.FindElements(By.TagName("td"))[1].Text;
        newWindow.Close();
        driver.SwitchTo().Window(firstTab);
        return coordinates;
    }

    private List<IWebElement> GetNthElementsFromZipTable(int n)
    {
        List<IWebElement> rows = zipTable.FindElements(By.TagName("tr")).ToList();
        //have ajuste para tomar solo las filas del body de la tabla
        return rows.Skip(1).Take(n).ToList();
    }

    
    private bool IsElementDisplayed(IWebDriver driver)
    {
        try
        {
            driver.FindElement(By.Id("info")).Displayed.Equals(true);
            return true;
        }
        catch (NoSuchElementException)
        {
            if (driver.FindElement(By.XPath("//h1[contains(., 'Error on Zip-Codes.com')]")).Displayed)
            {
                return true;
            }
            throw;
        }
    }


}
