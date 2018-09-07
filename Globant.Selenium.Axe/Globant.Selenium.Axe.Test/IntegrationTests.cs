﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using FluentAssertions;
using System;
using Globant.Selenium.Axe.AxeDto;

namespace Globant.Selenium.Axe.Test
{
    [TestClass]
    public class IntegrationTests
    {
        private IWebDriver _webDriver;
        private const string TargetTestUrl = "https://www.facebook.com/";

        [TestInitialize]
        public void Initialize()
        {
            _webDriver = new FirefoxDriver();
            _webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromMinutes(3));
            _webDriver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public virtual void TearDown()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestAnalyzeTarget()
        {
            _webDriver.Navigate().GoToUrl(TargetTestUrl);
            AxeResponse response = _webDriver.Analyze();
            response.Should().NotBeNull(nameof(response));
        }

    }
}
