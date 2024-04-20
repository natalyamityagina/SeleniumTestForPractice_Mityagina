using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[assembly: LevelOfParallelism(4)]

namespace SeleniumMityagina;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class BaseSeleniumTest
{
    protected IWebDriver driver;
    protected WebDriverWait wait;

    private readonly string[] optionsArguments =
    {
        "--no-sandbox",
        "--start-maximized",
        "--disable-extensions",
    };

    [SetUp]
    public void Setup()
    {
        // Создаем опции запуска браузера
        var options = new ChromeOptions();
        options.AddArguments(optionsArguments);

        // Создаем драйвер
        driver = new ChromeDriver(options);

        // Устанавливаем явное ожидание
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        // Устанавливаем неявные ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}