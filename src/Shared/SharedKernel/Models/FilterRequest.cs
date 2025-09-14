using Shared.SharedKernel.CustomQuery;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.SharedKernel.Models
{
    public class FilterRequest
    {
        [SwaggerSchema(Description ="Trang bắt đầu (0-based index)")]
        public int PageIndex { get; set; } = 0;

        [SwaggerSchema(Description = "Số record mỗi trang")]
        public int PageSize { get; set; } = 10;

        [SwaggerSchema(Description = "Danh sách sort (field + direction: asc|desc)")]
        public IEnumerable<Sort>? Sort { get; set; }

        [SwaggerSchema(
        Description = "Biểu thức filter.\n" +
                      "Operators supported: \n" +
                      "eq (=), neq (!=), ct (Contains), nct (Not Contains), \n" +
                      "sw (StartsWith), ew (EndsWith), \n" +
                      "gt (>), gte (>=), lt (<), lte (<=)"
        )]
        public Filter? Filter { get; set; }
    }
}
