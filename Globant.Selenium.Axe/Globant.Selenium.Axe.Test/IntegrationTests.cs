using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using FluentAssertions;
using System;
using Globant.Selenium.Axe.AxeDto;
using NUnit.Framework;

namespace Globant.Selenium.Axe.Test
{
    public class IntegrationTests
    {
        private IWebDriver _webDriver;
        private const string TargetTestUrl = "https://www.facebook.com/";

        [SetUp]
        public void SetupBeforeEachTest()
        {
            _webDriver = new FirefoxDriver();
            _webDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
            _webDriver.Manage().Window.Maximize();
        }

        [TearDown]
        public virtual void TeardownAfterEachTest()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }

        [Test]
        [Category("Integration")]
        public void TestAnalyzeTarget()
        {
            _webDriver.Navigate().GoToUrl(TargetTestUrl);
            AxeResponse response = _webDriver.Analyze();
            response.Should().NotBeNull(nameof(response));
        }

    }
}
