using System.Collections.Generic;


namespace Globant.Selenium.Axe
{
    public class AxeResultNode
    {
        public List<string> Target { get; set; }
        public string Html { get; set; }
        public string Impact { get; set; }
        public List<AxeResultNodeCheck> Any { get; set; }
        public List<AxeResultNodeCheck> All { get; set; }
        public List<AxeResultNodeCheck> None { get; set; }
    }
}
