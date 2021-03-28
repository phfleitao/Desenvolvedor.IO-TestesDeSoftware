using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace NerdStore.BDD.Tests.Config
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(Browser browser, string caminhoDriver, bool headless)
        {
            IWebDriver webDriver = null;

            switch (browser)
            {
                case Browser.Firefox:
                    var optionsFireFox = new FirefoxOptions();
                    if (headless)
                        optionsFireFox.AddArgument("--headless");

                    webDriver = new FirefoxDriver(caminhoDriver, optionsFireFox);

                    break;
                case Browser.Chrome:
                    var optionsChrome = new ChromeOptions();
                    optionsChrome.AddArgument("--ignore-ssl-errors=yes");
                    optionsChrome.AddArgument("--ignore-certificate-errors");

                    if (headless)
                        optionsChrome.AddArgument("--headless");

                    webDriver = new ChromeDriver(caminhoDriver, optionsChrome);

                    break;
            }

            return webDriver;
        }
    }
}
