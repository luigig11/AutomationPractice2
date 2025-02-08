using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;
using OpenQA.Selenium.Chrome;

namespace AutomationPractice2.TestUtilities;

public static class Utilities
{
    public static void TakeFullScreenShot(IWebDriver driver, string fileName)
    {
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        string localPath = GenerateLocalPath(fileName);
        screenshot.SaveAsFile(localPath);
    }

    private static string GenerateLocalPath(string fileName)
    {
        string localScreenshotPath = $@"C{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}Users{Path.DirectorySeparatorChar}PERSONAL{Path.DirectorySeparatorChar}pictures{Path.DirectorySeparatorChar}Screenshots";
        if (Directory.Exists(localScreenshotPath))
        {
            Directory.CreateDirectory(localScreenshotPath);
        }
        return Path.Combine(localScreenshotPath, fileName);
    }

    private static string GetProjectPath()
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        projectPath = projectPath.Substring(0, projectPath.IndexOf("bin"));
        return projectPath;
    }

    private static string GetScreenshotsPath()
    {
        string projectPath = GetProjectPath();
        string screenshotsPath = Path.Combine(projectPath, "Screenshots");
        if (!Directory.Exists(screenshotsPath))
        {
            Directory.CreateDirectory(screenshotsPath);
        }
        return screenshotsPath;
    }

}