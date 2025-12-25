namespace ExpenseControlSystem.Services {
    public class ServiceResult<T> {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public T? Result { get; set; }
    }
}
