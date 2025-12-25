using ExpenseControlSystem.DTOs.SubCategoryDtos;
using System.Text.Json.Serialization;

namespace ExpenseControlSystem.DTOs.CategoryDtos {
    public class ResponseCategoryDto {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ResponseSubCategoryDto>? SubCategories { get; set; }



    }
}
