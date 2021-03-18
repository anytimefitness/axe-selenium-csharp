using System;
using System.Linq;
using System.Text;

namespace Globant.Selenium.Axe
{
    public class AxeResultItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Help { get; set; }
        public string HelpUrl { get; set; }
        public string Impact { get; set; }
        public string[] Tags { get; set; }
        public AxeResultNode[] Nodes { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"AxeResultItem:");
            stringBuilder.AppendLine($"\tId: {Id}");
            stringBuilder.AppendLine($"\tDescription: {Description}");
            stringBuilder.AppendLine($"\tHelp: {Help}");
            stringBuilder.AppendLine($"\tHelpUrl: {HelpUrl}");
            stringBuilder.AppendLine($"\tImpact: {Impact}");
            stringBuilder.AppendLine($"\tTags: {String.Join(",", Tags)}");
            stringBuilder.AppendLine($"\tNodes:");
            foreach (var axeResultNode in Nodes)
            {
                stringBuilder.AppendLine($"\t\t{axeResultNode.Html}");
                if (axeResultNode.All.Any())
                {
                    stringBuilder.AppendLine($"\t\t\tAll:");
                    foreach(var axeResultNodeCheck in axeResultNode.All)
                    {
                        stringBuilder.AppendLine($"\t\t\t\t{axeResultNodeCheck.Message}");
                    }
                }
                if (axeResultNode.Any.Any())
                {
                    stringBuilder.AppendLine($"\t\t\tAny:");
                    foreach (var axeResultNodeCheck in axeResultNode.Any)
                    {
                        stringBuilder.AppendLine($"\t\t\t\t{axeResultNodeCheck.Message}");
                    }
                }
                if (axeResultNode.None.Any())
                {
                    stringBuilder.AppendLine($"\t\t\tNone:");
                    foreach (var axeResultNodeCheck in axeResultNode.None)
                    {
                        stringBuilder.AppendLine($"\t\t\t\t{axeResultNodeCheck.Message}");
                    }
                }
            }

            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }
    }
}
