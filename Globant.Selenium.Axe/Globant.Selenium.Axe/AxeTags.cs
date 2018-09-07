using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globant.Selenium.Axe
{
    public enum AxeTags
    {
        [Description("wcag2a")]
        Wcag2A,
        [Description("wcag2aa")]
        Wcag2AA,
        [Description("section508")]
        Section508,
        [Description("best-practice")]
        BestPractice,
        [Description("experimental")]
        Experimental
    }
}
