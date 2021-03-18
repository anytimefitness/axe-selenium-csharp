using System;
using System.Collections.Generic;
using System.Text;

namespace Globant.Selenium.Axe
{
    public class AxeResultNodeCheck
    {
        public string Id { get; set; }
        public string Impact { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<AxeResultNodeRelatedNode> RelatedNodes { get; set; }
    }
}
