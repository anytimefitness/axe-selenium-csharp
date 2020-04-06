using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Globant.Selenium.Axe.OptionsHelper;
using NUnit.Framework;

namespace Globant.Selenium.Axe.Test
{
    public class OptionsManagerTests
    {
        [Test]
        public void GivenNoOptionsConfigured_WhenIGenerateOptionsJson_ThenTheEmptyObjectNotationIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{}");
        }

        [Test]
        public void GivenTags_WhenIGenerateOptionsJson_ThenTheCorrectJsonIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.IncludeTags("a", "b");

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{\"runOnly\":{\"type\":\"tag\",\"values\":[\"a\",\"b\"]}}");
        }

        [Test]
        public void GivenRules_WhenIGenerateOptionsJson_ThenTheCorrectJsonIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.IncludeRules("a", "b");

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{\"runOnly\":{\"type\":\"rule\",\"values\":[\"a\",\"b\"]}}");
        }

        [Test]
        public void GivenTagsAndRules_WhenIGenerateOptionsJson_ThenTheCorrectJsonIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.IncludeTags("tag1");
            OptionsManager.IncludeRules("a", "b");

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{\"rules\":{\"a\":{\"enabled\":true},\"b\":{\"enabled\":true}},\"runOnly\":{\"type\":\"tag\",\"values\":[\"tag1\"]}}");
        }

        [Test]
        public void GivenTagsAndRulesAndExcludedRules_WhenIGenerateOptionsJson_ThenTheCorrectJsonIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.IncludeTags("tag1");
            OptionsManager.IncludeRules("a", "b");
            OptionsManager.ExcludedRules("c", "d");

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{\"rules\":{\"a\":{\"enabled\":true},\"b\":{\"enabled\":true},\"c\":{\"enabled\":false},\"d\":{\"enabled\":false}},\"runOnly\":{\"type\":\"tag\",\"values\":[\"tag1\"]}}");
        }

        [Test]
        public void GivenIncludedAndExcludedRulesAndNoTags_WhenIGenerateOptionsJson_ThenAnInvalidOperationtionExceptionOccurs()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.IncludeRules("a", "b");
            OptionsManager.ExcludedRules("c", "d");

            // act
            Assert.Throws<InvalidOperationException>(() =>
            {
                var optionsJson = OptionsManager.GenerateOptionsJson();
            });
        }

        [Test]
        public void GivenExcludedRules_WhenIGenerateOptionsJson_ThenTheCorrectJsonIsReturned()
        {
            //arrange
            var OptionsManager = new OptionsManager();
            OptionsManager.ExcludedRules("c", "d");

            // act
            var optionsJson = OptionsManager.GenerateOptionsJson();

            //Assert
            optionsJson.Should().BeEquivalentTo("{\"rules\":{\"c\":{\"enabled\":false},\"d\":{\"enabled\":false}}}");
        }
    }
}
