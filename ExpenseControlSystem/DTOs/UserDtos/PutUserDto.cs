using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.UserDtos {
    public class PutUserDto {
        [Required(ErrorMessage = "Name é obrigatório")]
        [MinLength(3, ErrorMessage = "Nome deve ter no minimo 3 letras")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no maximo 100 letras")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail Invalido")]
        public string Email { get; set; }
    }
}
