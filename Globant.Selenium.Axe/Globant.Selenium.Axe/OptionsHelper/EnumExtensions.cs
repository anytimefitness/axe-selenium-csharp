using System.ComponentModel;
using System.Linq;

namespace Globant.Selenium.Axe.OptionsHelper
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this System.Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return attr != null ? attr.Description : value.ToString();
        }
    }
}
