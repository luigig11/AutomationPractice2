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

namespace AutomationPractice2.PageObjects;

public class ZipCodePage
{
    private IWebDriver driver;
    private GoogleMapPage googleMapPage;

    public ZipCodePage(IWebDriver driver)
    {
        this.driver = driver;
        googleMapPage = new GoogleMapPage(driver);
    }

    #region Selectors
    string PageMainURL = "https://www.zip-codes.com/";
    string AdvanceSearchURL = "https://www.zip-codes.com/search.asp?selectTab=3";
    List<ZipCodeSearchResults> zipCodeSearchResults = new List<ZipCodeSearchResults>();
    public IWebElement zipCodeInput => driver.FindElement(By.XPath("//input[@aria-label='fld_zip']"));
    public IWebElement town_CityInput => driver.FindElement(By.XPath("//input[@aria-label='City']"));
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
            ZipCodeSearchResults zipCodeSearchResults = new ZipCodeSearchResults();
            zipCodeSearchResults.ZipCode = row.FindElements(By.TagName("td"))[1].Text; //esta fila corresponde a la primea fila, la de los encabezados. por eso da error
            zipCodeSearchResults.City = row.FindElements(By.TagName("td"))[2].Text;
            zipCodeSearchResults.State = row.FindElements(By.TagName("td"))[3].Text;
            row.FindElements(By.TagName("td"))[1].Click();
            wait.Until(x => infoDiv.Displayed == true);
            (string latitude, string longitude) coordinates = GetCoordinates();
            googleMapPage.GoToGoogleMapPage();
            wait.Until(x => googleMapPage.sceneDiv.Displayed == true);
            googleMapPage.SearchLocation($"{coordinates.latitude}, {coordinates.longitude}");
            wait.Until(x => googleMapPage.coodrinatesHeader.Displayed == true);
            Utilities.TakeFullScreenShot(driver, $"{zipCodeSearchResults.City}-{zipCodeSearchResults.State}-{zipCodeSearchResults.ZipCode}.jpg");
            //return to the previous zipcode results page page
        }
    }

    private List<IWebElement> GetNthElementsFromZipTable(int n)
    {
        List<IWebElement> rows = zipTable.FindElements(By.TagName("tr")).ToList();
        //have ajuste para tomar solo las filas del body de la tabla
        return rows.Take(n).ToList();
    }

    public (string, string) GetCoordinates()
    {
        IWebElement coordinates = zipCodeTableInfo.FindElements(By.TagName("tr"))[8];
        var latitude = coordinates.FindElements(By.TagName("td"))[0].Text;
        var longitud = coordinates.FindElements(By.TagName("td"))[1].Text;
        return (latitude, longitud);
    }


}
