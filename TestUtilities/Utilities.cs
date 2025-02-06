using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;
using OpenQA.Selenium.Chrome;

namespace AutomationPractice2.TestUtilities;

public static class Utilities
{
    public static void TakeFullScreenShot(IWebDriver driver, string fileName)
    {
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(fileName);
    }

}