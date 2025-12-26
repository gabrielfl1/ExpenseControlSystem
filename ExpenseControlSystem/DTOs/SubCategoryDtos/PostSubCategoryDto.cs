using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.SubCategoryDtos {
    public class PostSubCategoryDto {

        [Required(ErrorMessage = "Name é obrigatório")]
        [MinLength(3, ErrorMessage = "Name deve ter no mínimo 3 caracteres")]
        [MaxLength(100, ErrorMessage = "Name deve ter no máximo 100 caracteres")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "Description deve ter no mínimo 3 caracteres")]
        [MaxLength(250, ErrorMessage = "Description deve ter no máximo 250 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "CategoryId é obrigatório")]
        public Guid? CategoryId { get; set; }
    }
}
