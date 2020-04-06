using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using OpenQA.Selenium;
using Moq;
using NUnit.Framework;

namespace Globant.Selenium.Axe.Test
{
    public class AxeBuilderTest
    {
        [Test]
        public void ThrowWhenDriverIsNull()
        {
            //arrange / act /assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var axeBuilder = new AxeBuilder(null);
            });
        }

        [Test]
        public void ThrowWhenOptionsAreNull()
        {
            //arrange
            var driver = new Mock<IWebDriver>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var axeBuilder = new AxeBuilder(driver.Object, null);
            });
        }

        [Test]
        public void ShouldExecuteAxeScript()
        {
            //arrange
            var driver = new Mock<IWebDriver>();
            var jsExecutor = driver.As<IJavaScriptExecutor>();
            var targetLocator = new Mock<ITargetLocator>();

            driver
                .Setup(d => d.FindElements(It.IsAny<By>()))
                .Returns(new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0)));

            driver.Setup(d => d.SwitchTo()).Returns(targetLocator.Object);
            targetLocator.Setup(t => t.DefaultContent()).Returns(driver.Object);

            jsExecutor
                .Setup(js => js.ExecuteAsyncScript(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns(
                    new
                    {
                        results = new
                        {
                            violations = new object[] { },
                            passes = new object[] { },
                            inapplicable = new object[] { },
                            incomplete = new object[] { },
                            timestamp = DateTimeOffset.Now,
                            url = "www.test.com",
                        }
                    });

            var builder = new AxeBuilder(driver.Object);
            var result = builder.Analyze();

            result.Should().NotBeNull();
            result.Error.Should().BeNull();
            result.Results.Should().NotBeNull();
            result.Results.Inapplicable.Should().NotBeNull();
            result.Results.Incomplete.Should().NotBeNull();
            result.Results.Passes.Should().NotBeNull();
            result.Results.Violations.Should().NotBeNull();

            result.Results.Inapplicable.Length.Should().Be(0);
            result.Results.Incomplete.Length.Should().Be(0);
            result.Results.Passes.Length.Should().Be(0);
            result.Results.Violations.Length.Should().Be(0);

            driver.VerifyAll();
            targetLocator.VerifyAll();
            jsExecutor.VerifyAll();
        }
    }
}
