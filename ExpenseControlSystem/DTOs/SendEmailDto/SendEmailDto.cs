using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.UserDtos {
    public class SendEmailDto {

        [Required(ErrorMessage = "O parametro ToName é obrigatório")]
        [MinLength(3, ErrorMessage = "O parâmetro ToName deve ter no mínimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "O parâmetro ToName deve ter no máximo 100 caracteres")]
        public string ToName { get; set; }

        [Required(ErrorMessage = "O parametro ToEmail é obrigatório")]
        [MinLength(3, ErrorMessage = "O parâmetro ToEmail deve ter no mínimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "O parâmetro ToEmail deve ter no máximo 100 caracteres")]
        public string ToEmail { get; set; }

        public List<Guid>? UserId { get; set; }
        public List<Guid>? SubCategoryId { get; set; }

        public bool? IsPaid { get; set; }
        public bool? LatePayment { get; set; }
    }
}
