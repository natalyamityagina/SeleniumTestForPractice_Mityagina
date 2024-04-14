using System.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumMityagina;

public class SeleniumTestForPractice
{

    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // зайти в хром ( с помощью вебдрайвера) 
        var driver = new ChromeDriver(options); //  Открыть хром
        
        // перейти на  https://staff-testing.testkontur.ru/
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");

        // ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("natalyamityaginalinn@yandex.ru");
        
        var password =  driver.FindElement(By.Name("Password"));
        password.SendKeys("22!9mdG£Ekv7");
        Thread.Sleep(3000);
        // нажать на кнопку "войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        Thread.Sleep(3000);
        
        // проверяем что мы находимся на нужной странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news2");
        
        // Закрываем браузер и убиваем процесс драйвера 
        driver.Quit();
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}