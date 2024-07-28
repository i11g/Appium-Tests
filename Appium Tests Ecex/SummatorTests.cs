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
                DeviceName = "Pixel 7 API 35",
                App = @"C:\\com.example.androidappsummator.apk",
                PlatformVersion = "15"
            };

            _driver = new AndroidDriver (_appiumLocalService, androidOptions);

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}