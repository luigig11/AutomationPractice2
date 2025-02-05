using AutomationPractice2;
using AutomationPractice2.PageObjects;
using AutomationPractice2.PageObjects.Models;
using OpenQA.Selenium.Support.UI;

namespace NunitSeleniumTemplate;

[TestFixture]
public class Tests : BaseTestClass
{
    private ZipCodePage zipCodePage;

    [SetUp]
    public void SetUp()
    {
        zipCodePage = new ZipCodePage(driver);
    }

    [Test]
    public void Test1([Values("mar")] string search)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        zipCodePage.GoToAdvanceSearchPage();
        AdvanceSearchForm searchForm = new AdvanceSearchForm(town_city: search);
        wait.Until(x => zipCodePage.zipCodeInput.Displayed == true);
        zipCodePage.SearchZipCodeByTown(search);
    }
}