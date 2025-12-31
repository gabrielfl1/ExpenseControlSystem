using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.ExpenseDtos {
    public class PutExpenseDto {

        [Required(ErrorMessage = "O parâmetro Description é obrigatório")]
        [MinLength(3, ErrorMessage = "O parâmetro Description deve ter no mínimo 3 caracteres")]
        [MaxLength(255, ErrorMessage = "O parâmetro Description deve ter no maximo 255 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "o parametro Amount é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "O parâmetro DueDate é obrigatório")]
        public DateTime? DueDate { get; set; }

        public DateTime? PaidAt { get; set; }
        public bool IsPaid { get; set; }

        [Required(ErrorMessage = "O parâmetro SubCategoryId é obrigatório")]
        public Guid? SubCategoryId { get; set; }

        [Required(ErrorMessage = "O parâmetro UserId é obrigatório")]
        public Guid? UserId { get; set; }
    }
}
