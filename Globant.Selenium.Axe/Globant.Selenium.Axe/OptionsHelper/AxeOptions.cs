using System.Collections.Generic;

namespace Globant.Selenium.Axe.OptionsHelper
{
    public class AxeOptions
    {
        public RunOnly RunOnly { get; set; }

        public Dictionary<string, RuleOverride> Rules = new Dictionary<string, RuleOverride>();
    }
}
