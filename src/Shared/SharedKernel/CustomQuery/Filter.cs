
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.SharedKernel.CustomQuery
{
    public class Filter
    {
        [SwaggerSchema(Description = "Tên field trong entity để filter, ví dụ: 'Fullname', 'Gender', 'TrainingFromYear'")]
        public string Field { get; set; }

        [SwaggerSchema(
            Description = "Toán tử so sánh:\n" +
            " - eq  : bằng (=)\n" +
            " - neq : khác (!=)\n" +
            " - ct  : chứa (Contains)\n" +
            " - nct : không chứa (NOT Contains)\n" +
            " - sw  : bắt đầu với (StartsWith)\n" +
            " - ew  : kết thúc với (EndsWith)\n" +
            " - gt  : lớn hơn (>)\n" +
            " - gte : lớn hơn hoặc bằng (>=)\n" +
            " - lt  : nhỏ hơn (<)\n" +
            " - lte : nhỏ hơn hoặc bằng (<=)")]
        public string Operator { get; set; }

        [SwaggerSchema(Description = "Giá trị filter. Kiểu dữ liệu phải khớp với field (string, int, DateTime, v.v.)")]
        public object Value { get; set; }

        [SwaggerSchema(Description ="Logic nối các điều kiện con: 'and' hoặc 'or'. Mặc định là 'and' nếu để trống.")]
        public string? Logic { get; set; }

        [SwaggerSchema(Description = "Danh sách filter con (nested filters). Dùng để group nhiều điều kiện lại.")]
        public List<Filter> Filters { get; set; } = new();
    }
}
