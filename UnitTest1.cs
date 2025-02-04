using AutomationPractice2;

namespace NunitSeleniumTemplate;

public class Tests : BaseTestClass
{
    

    [Test]
    public void Test1()
    {
        driver.Navigate().GoToUrl("https://www.google.com");
    }
}