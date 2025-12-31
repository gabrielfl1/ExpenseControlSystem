using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseControlSystem.Extensions {
    public static class ModelStateExtensions {

        private static readonly Dictionary<string, string?> ErrorMap = new Dictionary<string, string?>() {
                { "The JSON value could not be converted to System.Nullable`1[System.Guid]. Path: $.CategoryId", "CategoryId inválido. Informe um GUID válido." },
                { "The JSON value could not be converted to System.Nullable`1[System.Guid]. Path: $.UserId", "UserId inválido. Informe um GUID válido." },
                { "The JSON value could not be converted to System.Nullable`1[System.Guid]. Path: $.SubCategoryId", "SubCategoryId inválido. Informe um GUID válido." },
                { "The JSON object contains a trailing comma", "Modelo de JSON inválido." },
                { "'\"' is invalid after a value", "Modelo de JSON inválido." },
                { "The dto field is required.", null },
                {"The JSON value could not be converted to System.Nullable`1[System.DateTime]. Path: $.DueDate", "DueDate inválido. Informe uma data válida" },
                {"The JSON value could not be converted to System.Nullable`1[System.DateTime]. Path: $.PaidAt", "PaidAt inválido. Informe uma data válida" },
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
