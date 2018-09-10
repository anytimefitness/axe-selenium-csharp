using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Globant.Selenium.Axe
{
    public class AxeResults
    {
        public AxeResultItem[] Violations { get; set; }
        public AxeResultItem[] Passes { get; set; }
        public AxeResultItem[] Inapplicable { get; set; }
        public AxeResultItem[] Incomplete { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string Url { get; set; }
    }
}
