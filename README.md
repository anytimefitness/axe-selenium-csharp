# axe-selenium-csharp
Tools for using aXe for web accessibility testing with C# and Selenium. Inspired on [axe-selenium-java](https://github.com/dequelabs/axe-selenium-java)

This project born as a need to have a clean .NET wrapper for aXe.

**Work in progress!! Stay tunned.**

## Getting Started

Install via Nuget: 
```powershell
PM> Install-Package Globant.Selenium.Axe
```

Import this namespace:
```csharp
using Globant.Selenium.Axe;
```

and call the extension method ```Analyze``` from your WebDriver object
```csharp
IWebDriver webDriver = new FirefoxDriver();
AxeResponse response = webDriver.Analyze();
response.Error.Should().BeNull();
response.Results.Violations.Length.ShouldBeEquivalentTo(0);
```

## Documentation
Work in progress!!

### BuilderExamples
Run only rules that are in the Wcag2AA and Wcag2A categories, there aren't any iframes on the page (so don't bother injecting into them), and only analyzed the relevant section (and all its children):
```csharp
IWebElement elementToAnalyze = webDriver.FindElement(By.Id("importantSection"));
var response = new AxeBuilder(WebDriver).OptionsIncludeTags(AxeTags.Wcag2AA, AxeTags.Wcag2A).SkipIFrames().Analyze(elementToAnalyze);
```

Run only rules in the Wcag2AA catgory (note, this isn't a superset of Wcag2A... you'd need to specify that separately), and only analyze divs within the iframe with the id "iframeId":
```csharp
var response = new AxeBuilder(WebDriver).OptionsIncludeTags(AxeTags.Wcag2AA).IncludeElementsMatching(new ByCssSelector("div").InIFrameIdentifiedByCssSelector("#iframeId")).Analyze();
```

## Thanks
Specially thanks to @jdmesalosada to make this happen and to always improve our jobs.
