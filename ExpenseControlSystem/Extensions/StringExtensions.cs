namespace ExpenseControlSystem.Extensions {
    public static class StringExtensions {

        public static string StringEditor(string text) {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Trim();

            return text = char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }
    }
}
