using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.CategoryDtos {
    public class PutCategoryDto {

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O parâmetro Name deve ter no mínimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "O parâmetro Name deve ter no máximo 100 caracteres")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "O parâmetro Description deve ter no mínimo 3 caracteres")]
        [MaxLength(250, ErrorMessage = "O parâmetro Description deve ter no máximo 250 caracteres")]
        public string? Description { get; set; }

    }
}
