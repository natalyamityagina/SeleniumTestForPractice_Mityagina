using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace SeleniumMityagina;

public class StaffSeleniumTests : BaseSeleniumTest
{
    private const string LOGIN = "natalyamityaginalinn@yandex.ru";
    private const string PASSWORD = "22!9mdG£Ekv7";

    [Test] // 1. Главная. Проверка УСПЕШНОЙ авторизации
    public void AuthorizationTest()
    {
        const string EXPECTED_URL = "https://staff-testing.testkontur.ru/news";

        // 1. Авторизуемся
        Authorize(LOGIN, PASSWORD);

        // 2. Сохраняем текущий Url в переменную
        var currentUrl = driver.Url;

        Assert.That(currentUrl, Is.EqualTo(EXPECTED_URL),
            "После авторизации, фактический Url не совпадает с ожидаемым" +
            $"Ожидаемый Url {EXPECTED_URL}" +
            $"Фактический Url {currentUrl}");
    }

    [Test] // 2. Главная. Проверка ошибок/сообщений валидации при авторизации
    public void AuthorizationValidationMessageTest()
    {
        const string INCORRECT_PASSWORD = "incorrectPassword";
        const string EXPECTED_VALIDATION_ERROR_MESSAGE_TEXT = "Неверный логин или пароль";

        // Локатор ошибки/сообщения валидации
        var validationErrorLocator = By.CssSelector(".login-form .validation-summary-errors");

        // 1. Заполняем логин и некорректный пароль, и нажимаем Войти
        SignIn(LOGIN, INCORRECT_PASSWORD);

        // 2. Дожидаемся появления ошибки/сообщения валидации
        wait.Until(ExpectedConditions.ElementIsVisible(validationErrorLocator));

        // 3. Сохраняем текст ошибки/сообщения валидации в переменную
        var validationErrorText = driver.FindElement(validationErrorLocator).Text;

        Assert.That(validationErrorText, Is.EqualTo(EXPECTED_VALIDATION_ERROR_MESSAGE_TEXT),
            "Текст ошибки/сообщения валидации не совпадает с ожидаемым" +
            $"Ожидаемый текст: {EXPECTED_VALIDATION_ERROR_MESSAGE_TEXT}" +
            $"Фактический текст: {validationErrorText}");
    }


    [Test] // 3. Мероприятия. В лайтбоксе "Создание мероприятия" подтягивается карта
    public void LightboxMapTest()
    {
        // Локатор кнопки "Создать" на странице Мероприятия
        var addButtonLocator = By.CssSelector("[data-tid='AddButton'] button");
        // Локатор содержимого лайтбокса "Создание мероприятия"
        var lightboxModalContentLocator = By.CssSelector("[data-tid='ModalPageBody']");
        // Локатор карты в лайтбоксе "Создание мероприятия"
        var lightboxMapLocator = By.CssSelector("[data-tid='ModalPageBody'] .map-wrapper");

        // 1. Авторизуемся
        Authorize(LOGIN, PASSWORD);

        // 2. Переходим на страницу Мероприятия
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");

        // 3. Кликаем по кнопке "Создать"
        driver.FindElement(addButtonLocator).Click();

        // 4. Дожидаемся появления лайтбокса
        wait.Until(ExpectedConditions.ElementIsVisible(lightboxModalContentLocator));

        // 5. Находим элемент с картой
        var lightboxMapElement = driver.FindElement(lightboxMapLocator);

        Assert.That(lightboxMapElement.Displayed, Is.True,
            "Не появилась карта в лайтбоксе 'Создание мероприятия' при создании мероприятия");
    }

    [Test] // 4. Главная. Переход в профиль пользователя через поиск
    public void SearchProfileTest()
    {
        const string EMPLOYEE_FIO = "Иванова Ирина Александровна";

        // Локатор поля поиска
        var searchBarLocator = By.CssSelector("[data-tid='SearchBar']");
        // Локатор поля ввода поиска
        var searchBarInputLocator = By.CssSelector("[data-tid='SearchBar'] input");
        // Локатор сотрудника в саджесте
        var employeeSudgestLocator = By.CssSelector($"[data-tid='ScrollContainer__inner'] [title='{EMPLOYEE_FIO}']");
        // Локатор контактов на странице сотрудника
        var contactCardLocator = By.CssSelector("[data-tid='ContactCard']");
        // Локатор ФИО сотрудника на странице сотрудника
        var employeeFioLocator = By.CssSelector("[data-tid='EmployeeName']");

        // 1. Авторизуемся
        Authorize(LOGIN, PASSWORD);

        // 2. Кликаем по полю поиска чтобы сделать элемент активным
        driver.FindElement(searchBarLocator).Click();

        // 2. Вводим в поле поиска искомого сотрудника
        driver.FindElement(searchBarInputLocator).SendKeys(EMPLOYEE_FIO);

        // 3. Дожидаемся появления саджеста с искомым сотрудником
        wait.Until(ExpectedConditions.ElementIsVisible(employeeSudgestLocator));

        // 4. Кликаем по результату поиска
        driver.FindElement(employeeSudgestLocator).Click();

        // 5. Дожидаемся появления карточки контактов сотрудника
        wait.Until(ExpectedConditions.ElementIsVisible(contactCardLocator));

        // 6. Сохраняем ФИО сотрудника в переменную
        var employeeFio = driver.FindElement(employeeFioLocator).Text;

        Assert.That(employeeFio, Is.EqualTo(EMPLOYEE_FIO),
            "После перехода на страницу сотрудника из поиска, фактическое ФИО не совпадает с ожидаемым" +
            $"Ожидаемое ФИО {EMPLOYEE_FIO}" +
            $"Фактическое ФИО {employeeFio}");
    }

    [Test] // 5. Документы. При создании заявки открывается лайтбокс
    public void LightboxDocumentsTest()
    {
        const string CREATE_ORDER_TEXT = "Создать заявку";

        // Локатор кнопки "Создать" на странице Документы
        var addDocumentLocator = By.XPath("//*[@data-tid='PageHeader']//*[contains(text(),'СОЗДАТЬ')]");
        // Локатор лайтбокса "Создать заявку"
        var lightboxModalContentLocator = By.CssSelector("[data-tid='modal-content']");
        // Локатор заголовка лайтбокса "Создать заявку"
        var lightboxPageHeaderLocator = By.CssSelector("[data-tid='ModalPageHeader']");

        // 1. Авторизуемся
        Authorize(LOGIN, PASSWORD);

        // 2. Переходим на страницу Документы
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/documents");

        // 3. Кликаем по кнопке "Создать"
        driver.FindElement(addDocumentLocator).Click();

        // 4. Дожидаемся появления лайтбокса
        wait.Until(ExpectedConditions.ElementIsVisible(lightboxModalContentLocator));

        // 5. Сохраняем значение заголовка лайтбокса в переменную
        var lightboxPageHeaderText = driver.FindElement(lightboxPageHeaderLocator).Text;

        Assert.That(lightboxPageHeaderText, Is.EqualTo(CREATE_ORDER_TEXT),
            "Заголовок в лайтбоксе Создания заявки не соответствует ожидаемому" +
            $"Ожидаемый заголовок: {CREATE_ORDER_TEXT}" +
            $"Фактический заголовок: {lightboxPageHeaderText}");
    }

    private void Authorize(string login, string password)
    {
        SignIn(login, password);
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    private void SignIn(string login, string password)
    {
        var loginInputLocator = By.CssSelector("#Username");
        var passwordInputLocator = By.CssSelector("#Password");
        var loginButtonLocator = By.CssSelector("#login_form button[value='login']");

        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        driver.FindElement(loginInputLocator).SendKeys(login);
        driver.FindElement(passwordInputLocator).SendKeys(password);
        driver.FindElement(loginButtonLocator).Click();
    }
}