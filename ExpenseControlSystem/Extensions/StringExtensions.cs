using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace ExpenseControlSystem.Extensions {
    public static class StringExtensions {

        public static string StringTitleEditor(string text) {

            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Trim();

            return text = char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

        public static string StringNameEditor(string text) {

            if (string.IsNullOrWhiteSpace(text))
                return text;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            return  textInfo.ToTitleCase(text.ToLower()) ;
        }
    }
}
