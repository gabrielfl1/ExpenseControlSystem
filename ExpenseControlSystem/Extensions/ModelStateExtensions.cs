using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseControlSystem.Extensions {
    public static class ModelStateExtensions {

        private static readonly Dictionary<string, string?> ErrorMap = new Dictionary<string, string?>() {
                { "could not be converted to System.Nullable`1[System.Guid]", "CategoryId inválido. Informe um GUID válido." },
                { "The JSON object contains a trailing comma", "JSON inválido." },
                { "'\"' is invalid after a value", "JSON inválido." },
                { "The dto field is required.", null }
        };

        public static List<string> GetErrors(this ModelStateDictionary modelState) {

            var result = new List<string>();

            foreach (var item in modelState.Values) {

                foreach (var error in item.Errors) {

                    var errorMessage = error.ErrorMessage;
                    var mapped = ErrorMap.FirstOrDefault(x => errorMessage.Contains(x.Key));

                    if (!string.IsNullOrEmpty(mapped.Key)) {
                        if (mapped.Value != null)
                            result.Add(mapped.Value);

                        continue;
                    }

                    result.Add(error.ErrorMessage);
                }
            }
            return result;
        }
    }
}
