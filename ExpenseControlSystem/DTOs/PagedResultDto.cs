namespace ExpenseControlSystem.DTOs {
    public class PagedResultDto<T> {
        public List<T> Result{ get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
