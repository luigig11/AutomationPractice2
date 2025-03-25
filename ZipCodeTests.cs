using AutomationPractice2.PageObjects;
using AutomationPractice2.PageObjects.Models;
using AutomationPractice2.TestUtilities;
//using OpenQA.Selenium.Interactions.Internal;
//using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
//using System.Collections.ObjectModel;

namespace AutomationPractice2;

[TestFixture]
public class ZipCodeTests : BaseTestClass
{
    private ZipCodePage zipCodePage;
    private GoogleMapPage googleMapPage;

    [SetUp]
    public void SetUp()
    {
        zipCodePage = new ZipCodePage(driver);
        googleMapPage = new GoogleMapPage(driver);
    }

    [Test]
    public void TakeScrennshot_When_SearchingALocationInGoogleMap([Values("mar")] string search)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        zipCodePage.GoToAdvanceSearchPage();
        wait.Until(x => zipCodePage.zipCodeInput.Displayed == true);
        zipCodePage.SearchZipCodeByTown(search);
        wait.Until(x => zipCodePage.zipTable.Displayed == true);
        List<ZipCodeSearchResults> zipCodes = zipCodePage.GetZipCodeInfo(10);
        foreach (var zipCode in zipCodes)
        {

            //if (string.IsNullOrEmpty(zipCode.Latitude) || string.IsNullOrEmpty(zipCode.Longitude))
            //{
            //    continue;
            //}
            googleMapPage.GoToGoogleMapPage(googleMapPage.GoogleMapUrl);
            wait.Until(x => googleMapPage.sceneDiv.Displayed == true);
            googleMapPage.SearchLocation($"{zipCode.Latitude},{zipCode.Longitude}");
            wait.Until(x => googleMapPage.coodrinatesHeader.Displayed == true);
            Utilities.TakeFullScreenShot(driver, $"{zipCode.City}-{zipCode.State}-{zipCode.ZipCode}.jpg");
        }

    }

    [Test]
    public void TakeScreenShot_When_NavigatingToGoogleMapLink([Values("mar")] string search)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        zipCodePage.GoToAdvanceSearchPage();
        wait.Until(x => zipCodePage.zipCodeInput.Displayed == true);
        zipCodePage.SearchZipCodeByTown(search);
        wait.Until(x => zipCodePage.zipTable.Displayed == true);
        List<ZipCodeSearchResults> zipCodes = zipCodePage.GetZipCodeInfo(10);
        foreach (var zipCode in zipCodes)
        {
            string googleMapLink = googleMapPage.GenerateGoogleMapLinkByCoordinates(zipCode.Latitude, zipCode.Longitude);
            googleMapPage.GoToGoogleMapPage(googleMapLink);
            wait.Until(x => googleMapPage.coodrinatesHeader.Displayed == true);
            Utilities.TakeFullScreenShot(driver, $"{zipCode.City}-{zipCode.State}-{zipCode.ZipCode}.jpg");
        }
    }
}