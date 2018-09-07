using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Globant.Selenium.Axe.OptionsHelper
{
    public class OptionsManager
    {
        private List<string> _includedTags = new List<string>();
        private List<string> _excludedTags = new List<string>();
        private List<string> _includedRules = new List<string>();
        private List<string> _excludedRules = new List<string>();

        public string GenerateOptionsJson()
        {
            AxeOptions axeOptions = new AxeOptions();

            if (_includedTags.Count > 0)
            {
                axeOptions.RunOnly = new RunOnly
                {
                    Type = "tag",
                    Values = _includedTags
                };

                foreach (var includedRule in _includedRules)
                {
                    axeOptions.Rules.Add(includedRule, new RuleOverride {Enabled = true});
                }

                foreach (var excludedRule in _excludedRules)
                {
                    axeOptions.Rules.Add(excludedRule, new RuleOverride{Enabled = false});
                }
            }
            else if(_includedRules.Count > 0)
            {
                axeOptions.RunOnly = new RunOnly
                {
                    Type = "rule",
                    Values = _includedRules
                };
                if (_excludedRules.Count > 0)
                {
                    throw new InvalidOperationException("If you specify included rules, and not tags, then only specify the ones that you want, not the ones you don't.");
                }
            }
            else if(_excludedRules.Count > 0)
            {
                foreach (var excludedRule in _excludedRules)
                {
                    axeOptions.Rules.Add(excludedRule, new RuleOverride { Enabled = false });
                }
            }

            if (axeOptions.Rules.Count == 0)
            {
                //This allows it to not be in the json at all.
                axeOptions.Rules = null;
            }

            return JsonConvert.SerializeObject(axeOptions, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public void IncludeTags(params AxeTags[] tags)
        {
            _includedTags.AddRange(tags.Select(tag=> tag.GetStringValue()).ToList());
        }

        public void IncludeTags(params string[] tags)
        {
            _includedTags.AddRange(tags.ToList());
        }

        //Technically this should be possible, but the logic starts getting complicated in the OptionsManager.
        //I leave this to a future date or future contributor.
        //public void ExcludeTags(params string[] tags)
        //{
        //    _excludedTags.AddRange(tags.ToList());
        //}

        public void IncludeRules(params string[] rules)
        {
            _includedRules.AddRange(rules.ToList());
        }

        public void ExcludedRules(params string[] rules)
        {
            _excludedRules.AddRange(rules.ToList());
        }
    }
}
