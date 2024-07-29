using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Internal;

namespace Appium_Tests_Ecex
{
    public class SummatorTests
    {
        private AndroidDriver _driver;

        private AppiumLocalService _appiumLocalService;

        [OneTimeSetUp]
        public void Setup()
        {
            _appiumLocalService = new AppiumServiceBuilder()
                .WithIPAddress("127.0.0.1")
                .UsingPort(4723)
                .Build();
            _appiumLocalService.Start();

            var androidOptions = new AppiumOptions()
            {
                PlatformName = "Android",
                AutomationName = "UiAutomator2",
                DeviceName = "Pixel 7 API 34",
                App = @"C:\\com.example.androidappsummator.apk",
                PlatformVersion = "14"
            };

            _driver = new AndroidDriver(_appiumLocalService, androidOptions);

        }
        [OneTimeTearDown]

        public void OneTimeTerDown()
        {
            _driver.Quit();
            _driver.Dispose();
            _appiumLocalService.Dispose();
        }

        [Test]
        public void TestWithValidData()
        {
            IWebElement field1 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear(); 
            field1.SendKeys("5");

            IWebElement field2 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id / editText2"));
            field2.Clear();
            field2.SendKeys("3");

            IWebElement clickButton = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum"));
            clickButton.Click();

            IWebElement result = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editTextSum"));

            Assert.That(result.Text, Is.EqualTo("8"));            

        }
        [Test] 

        public void TestWithInvalidData()
        {
            IWebElement field1 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear();
            

            IWebElement field2 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id / editText2"));
            field2.Clear();
            

            IWebElement clickButton = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum"));
            clickButton.Click();

            IWebElement result = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editTextSum"));

            Assert.That(result.Text, Is.EqualTo("error"));
        }

        [Test] 

        public void TestWithFirstDataValid()
        {
            IWebElement field1 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear();
            field1.SendKeys("10"); 


            IWebElement field2 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id / editText2"));
            field2.Clear();
            field2.SendKeys(".");


            IWebElement clickButton = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum"));
            clickButton.Click();

            IWebElement result = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editTextSum"));

            Assert.That(result.Text, Is.EqualTo("error"));
        }
        [Test] 

        public void TestWithSecondDataValid()
        {
            IWebElement field1 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear();
            field1.SendKeys("..");


            IWebElement field2 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id / editText2"));
            field2.Clear();
            field2.SendKeys("15");


            IWebElement clickButton = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum"));
            clickButton.Click();

            IWebElement result = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editTextSum"));

            Assert.That(result.Text, Is.EqualTo("error"));
        }

        [Test]
        [TestCase("10", "20","30")]

        public void ParameTrizedTests_WithValidData(string input1, string input2, string result)
        {
            IWebElement field1 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear();
            field1.SendKeys(input1);

            IWebElement field2 = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editText1"));
            field1.Clear();
            field1.SendKeys(input2);

            IWebElement clickButton = _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum"));
            clickButton.Click();

            IWebElement data= _driver.FindElement(MobileBy.Id("com.example.androidappsummator:id/editTextSum"));

            Assert.That(data.Text, Is.EqualTo("result"));
        }
    }
}
