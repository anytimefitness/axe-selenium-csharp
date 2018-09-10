using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globant.Selenium.Axe
{
    /// <summary>
    /// Builder for CssSelectors to be passed to the AxeBuilder.
    /// Specify either just a string if the item is in the top level document
    /// Or chain on IFrame identifiers if the item is in an iframe.
    /// </summary>
    public class ByCssSelector
    {
        private List<string> orderedSelectorList = new List<string>();

        public ByCssSelector(string cssSelector)
        {
            orderedSelectorList.Add(cssSelector);
        }

        /// <summary>
        /// Use this (and chain them together) if you want to specify items in an iframe
        /// </summary>
        public ByCssSelector InIFrameIdentifiedByCssSelector(string cssSelectorThatIdentifiesIFrame)
        {
            orderedSelectorList.Add(cssSelectorThatIdentifiesIFrame);
            return this;
        }

        /// <summary>
        /// This is called by the IncludeExcludeManager to put the CssSelector into a format that Axe expects.
        /// </summary>
        /// <returns></returns>
        internal string[] FormatForAxe()
        {
            return orderedSelectorList.ToArray();
        }
    }
}
