using System.ComponentModel.DataAnnotations;

namespace ExpenseControlSystem.DTOs.SubCategoryDtos {
    public class GetSubCategoryDto {

        [Required(ErrorMessage = "Page é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "A pagina não deve ser um numero negativo ou zero")]
        public int? Page { get; set; }

        [Required(ErrorMessage = "PageSize é obrigatório")]
        [Range(1, 100, ErrorMessage = "A quantidade por pagina não deve exceder 100")]
        public int? PageSize { get; set; }

    }
}
