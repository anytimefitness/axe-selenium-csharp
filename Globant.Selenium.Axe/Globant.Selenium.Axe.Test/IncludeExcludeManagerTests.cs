using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Globant.Selenium.Axe.Test
{
    public class IncludeExcludeManagerTests
    {
        [Test]
        public void GivenNoExtraSettings_WhenIGetContextJson_ThenTheResultShouldBeTheWholeDocument()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("document");
        }

        [Test]
        public void GivenThatOneSelectorIsIncluded_WhenIGetContextJson_ThenTheResultShouldIncludeThatSelector()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();
            includeExcludeManager.IncludeSelector(new ByCssSelector("bar"));

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("{\"include\":[[\"bar\"]]}");
        }

        [Test]
        public void GivenThatTwoSelectorsAreIncluded_WhenIGetContextJson_ThenTheResultShouldIncludeEachSelectorInTheirOwnArray()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();
            includeExcludeManager.IncludeSelector(new ByCssSelector("foo"));
            includeExcludeManager.IncludeSelector(new ByCssSelector("bar"));

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("{\"include\":[[\"foo\"],[\"bar\"]]}");
        }

        [Test]
        public void GivenThatOneSelectorIsExcluded_WhenIGetContextJson_ThenTheResultShouldExcludeThatSelector()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();
            includeExcludeManager.ExcludeSelector(new ByCssSelector("foobar"));

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("{\"exclude\":[[\"foobar\"]]}");
        }

        [Test]
        public void GivenThatOneSelectorIsIncludedAndOneIsExcluded_WhenIGetContextJson_ThenTheResultShouldHaveBothIncludeAndExclude()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();
            includeExcludeManager.IncludeSelector(new ByCssSelector("foo"));
            includeExcludeManager.ExcludeSelector(new ByCssSelector("bar"));

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("{\"include\":[[\"foo\"]],\"exclude\":[[\"bar\"]]}");
        }

        [Test]
        public void GivenThatOneSelectorIsIncludedFromAnIFrame_WhenIGetContextJson_ThenTheResultShouldHaveTheSelectorAndIFrameSelectorInTheRightFormat()
        {
            //arrange
            var includeExcludeManager = new IncludeExcludeManager();
            includeExcludeManager.IncludeSelector(new ByCssSelector("foo").InIFrameIdentifiedByCssSelector("#iFrame"));

            // act
            var contextJson = includeExcludeManager.GetContextJson();

            //Assert
            contextJson.Should().BeEquivalentTo("{\"include\":[[\"foo\",\"#iFrame\"]]}");
        }
    }
}
