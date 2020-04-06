using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Globant.Selenium.Axe
{
    /// <summary>
    /// Handle all initialization, serialization and validations for includeExclude aXe object.
    /// For more info check this: https://github.com/dequelabs/axe-core/blob/master/doc/API.md#include-exclude-object
    /// </summary>
    [JsonObject]
    public class IncludeExcludeManager
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            //Formatting = Formatting.Indented, //Useful for debugging, but will break unit tests.
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };


        private List<string[]> _includeList = new List<string[]>();
        private List<string[]> _excludeList = new List<string[]>();

        [JsonProperty]
        private string[][] Include => _includeList.Count > 0 ? _includeList.ToArray() : null;

        [JsonProperty]
        private string[][] Exclude => _excludeList.Count > 0 ? _excludeList.ToArray() : null;

        /// <summary>
        /// Include the given selectors, i.e "#foo", "ul.bar .target", "div"
        /// </summary>
        /// <param name="selector">Selectors to include</param>
        public void IncludeSelector(ByCssSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            _includeList.Add(selector.FormatForAxe());
        }

        /// <summary>
        /// Exclude the given selectors, i.e "frame", "div.foo", "#foo"
        /// </summary>
        /// <param name="selector">Selector to exclude</param>
        public void ExcludeSelector(ByCssSelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            _excludeList.Add(selector.FormatForAxe());
        }

        /// <summary>
        /// Gets the json object which can be passed into the axe.run call.
        /// </summary>
        public string GetContextJson()
        {
            string command;
            if (_includeList.Count > 0 || _excludeList.Count > 0)
            {
                command = JsonConvert.SerializeObject(this, JsonSerializerSettings);
            }
            else
            {
                command = "document";
            }

            return command;
        }
    }
}
