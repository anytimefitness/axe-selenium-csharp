using Globant.Selenium.Axe.AxeDto;
using Globant.Selenium.Axe.OptionsHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenQA.Selenium;
using System;

namespace Globant.Selenium.Axe
{
    /// <summary>
    /// Fluent style builder for invoking aXe. Instantiate a new Builder and configure testing with the include(),
    /// exclude(), and options() methods before calling analyze() to run.
    /// </summary>
    public class AxeBuilder
    {
        private readonly IWebDriver _webDriver;
        private readonly IncludeExcludeManager _includeExcludeManager = new IncludeExcludeManager();
        private AxeBuilderOptions _axeBuilderOptions;

        private readonly OptionsManager _optionsManager = new OptionsManager();

        private static readonly AxeBuilderOptions DefaultOptions = new AxeBuilderOptions {ScriptProvider = new EmbeddedResourceAxeProvider()};

        private bool ShouldSkipFrames { get; set; } = false;

        private bool ShouldSkipInject { get; set; } = false;
    

        /// <summary>
        /// Initialize an instance of <see cref="AxeBuilder"/>
        /// </summary>
        /// <param name="webDriver">Selenium driver to use</param>
        public AxeBuilder(IWebDriver webDriver): this(webDriver, DefaultOptions)
        {
        }

        /// <summary>
        /// Initialize an instance of <see cref="AxeBuilder"/>
        /// </summary>
        /// <param name="webDriver">Selenium driver to use</param>
        /// <param name="options">Builder options</param>
        public AxeBuilder(IWebDriver webDriver, AxeBuilderOptions axeBuilderOptions)
        {
            if (webDriver == null)
                throw new ArgumentNullException(nameof(webDriver));

            if (axeBuilderOptions == null)
                throw new ArgumentNullException(nameof(axeBuilderOptions));

            _webDriver = webDriver;
            _axeBuilderOptions = axeBuilderOptions;
        }

        /// <summary>
        /// Execute the script into the target.
        /// </summary>
        /// <param name="command">Script to execute.</param>
        /// <param name="args"></param>
        private AxeResponse Execute(string command, params object[] args)
        {
            if (!ShouldSkipInject)
            {
                _webDriver.Inject(_axeBuilderOptions.ScriptProvider, injectIntoFrames:!ShouldSkipFrames);
            }

            object response = ((IJavaScriptExecutor)_webDriver).ExecuteAsyncScript(command, args);
            
            var jObject = JObject.FromObject(response);
            var responseString = jObject.ToString();
            
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            return JsonConvert.DeserializeObject<AxeResponse>(responseString, settings);
        }

        /// <summary>
        /// Selectors to include in the validation.
        /// </summary>
        /// <param name="selectors">Any valid CSS selectors</param>
        public AxeBuilder IncludeElementsMatchingAnyOf(params string[] selectors)
        {
            foreach (var selector in selectors)
            {
                _includeExcludeManager.IncludeSelector(new ByCssSelector(selector));
            }
            
            return this;
        }

        /// <summary>
        /// Selectors to include in the validation.
        /// </summary>
        /// <param name="selector">Any valid CSS selectors</param>
        public AxeBuilder IncludeElementsMatching(ByCssSelector selector)
        {
            _includeExcludeManager.IncludeSelector(selector);
            return this;
        }

        /// <summary>
        /// Exclude selectors
        /// Selectors to exclude from the validation.
        /// </summary>
        /// <param name="selectors">Any valid CSS selectors</param>
        public AxeBuilder ExcludeElementsMatchingAnyOf(params string[] selectors)
        {
            foreach (var selector in selectors)
            {
                _includeExcludeManager.ExcludeSelector(new ByCssSelector(selector));
            }
            
            return this;
        }

        /// <summary>
        /// Selectors to Exclude from the validation.
        /// </summary>
        /// <param name="selector">Any valid CSS selectors</param>
        public AxeBuilder ExcludeElementsMatching(ByCssSelector selector)
        {
            _includeExcludeManager.ExcludeSelector(selector);
            return this;
        }

        /// <summary>
        /// Only test the main document. Don't inject any iFrames with the script so they can be scanned.
        /// This can be a performance improvement. Use it if you know there aren't any iFrames on the page.
        /// </summary>
        /// <returns></returns>
        public AxeBuilder SkipIFrames()
        {
            ShouldSkipFrames = true;

            return this;
        }

        /// <summary>
        /// The analyzer needs to inject a large chunk of javascript into the page before it can analyze.
        /// If you've already analyzed something on the page and haven't navigated away since then,
        /// you can make the test go faster by not taking the time to inject the script again.
        /// </summary>
        public AxeBuilder SkipInject()
        {
            ShouldSkipInject = true;

            return this;
        }

        /// <summary>
        /// Run the rules included in these tags
        /// </summary>
        public AxeBuilder OptionsIncludeTags(params AxeTags[] tags)
        {
            _optionsManager.IncludeTags(tags);

            return this;
        }

        /// <summary>
        /// Run the rules included in these tags
        /// </summary>
        public AxeBuilder OptionsIncludeTags(params string[] tags)
        {
            _optionsManager.IncludeTags(tags);

            return this;
        }

        private string GetCallbackBoilerPlate()
        {
            return @"
                var seleniumProvidedCallback = arguments[arguments.length - 1];
                var axeCallback = function (err,results){
                    var resultsToSelenium = { Error:err, Results: results};
                    seleniumProvidedCallback(resultsToSelenium);
                };
            ";

        }

        /// <summary>
        /// Run aXe against a specific WebElement.
        /// </summary>
        /// <param name="targetedElement"> An IWebElement to test</param>
        /// <returns>An aXe results document</returns>
        public AxeResponse Analyze(IWebElement targetedElement)
        {
            string options = _optionsManager.GenerateOptionsJson();
            //The targted element ends up as arguments[0]
            string command = GetCallbackBoilerPlate() + $"axe.run(arguments[0], {options}, axeCallback);";
            return Execute(command, targetedElement);
        }

        /// <summary>
        /// Run aXe as configured by the builder
        /// </summary>
        /// <returns>An aXe results document</returns>
        public AxeResponse Analyze()
        {
            string context = _includeExcludeManager.GetContextJson();
            string options = _optionsManager.GenerateOptionsJson();
            
            var command = GetCallbackBoilerPlate() + $"axe.run({context}, {options}, axeCallback);";

            return Execute(command);
        }
    }
}