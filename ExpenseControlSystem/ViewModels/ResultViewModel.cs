namespace ExpenseControlSystem.ViewModels {
    public class ResultViewModel<T> {
        public T Result { get; private set; }
        public List<string> Errors { get; private set; } = new();

        public ResultViewModel(T result, List<string> errors) {
            Result = result;
            Errors = errors;
        }

        public ResultViewModel(T result) {
            Result = result;
        }

        public ResultViewModel(List<string> error) {
            Errors = error;
        }

        public ResultViewModel(string error) {
            Errors.Add(error);
        }
    }
}
