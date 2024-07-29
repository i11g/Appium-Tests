using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Internal;

namespace NotePadProject
{
    public class NotepadTests
    {
        private AndroidDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var androidOptions = new AppiumOptions
            {
                PlatformName="Android",
                AutomationName="UIAutomator2",
                DeviceName="Pixel 7a API 31",
                App= @"C:\\Notepad.apk\"
            };

            androidOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);
            _driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), androidOptions);

            _driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(10);

            try
            {
                var skipTutarial = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/btn_start_skip"));
                skipTutarial.Click();
            }
            catch (NoSuchElementException)
            {

            }
        }
        [OneTimeTearDown] 

        public void OnetimeTearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
        [Test, Order(1)]
        public void Test_CreateNote()
        {
            var addNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1"));
            addNote.Click();
            var createTextNote = _driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"Text\")"));
            createTextNote.Click();
            var writetextField = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note"));
            writetextField.Click();

            var clickNote = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn"));
            clickNote.Click();
            clickNote.Click();

            var noteCreated = _driver.FindElement(MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/title"));

            Assert.That(noteCreated, Is.Not.Null, "The not was not cretaed succesfully");
            Assert.That(noteCreated.Text, Is.EqualTo("Test"));
        }


        [Test, Order(2)]
        public void Test_EditNote()
        {
               
        }
    }
}