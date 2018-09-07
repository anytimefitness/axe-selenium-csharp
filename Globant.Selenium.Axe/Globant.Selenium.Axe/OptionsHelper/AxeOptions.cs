using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globant.Selenium.Axe.OptionsHelper
{
    public class AxeOptions
    {
        public RunOnly RunOnly { get; set; }

        public Dictionary<string, RuleOverride> Rules = new Dictionary<string, RuleOverride>();
    }
}
