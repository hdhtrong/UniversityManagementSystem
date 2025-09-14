using ClosedXML.Excel;

namespace EduService.Application.Extentions
{
    public static class ClosedXmlExtensions
    {
        public static T? GetNullable<T>(this IXLCell cell) where T : struct
        {
            if (cell.TryGetValue<T>(out var value))
                return value;
            return null;
        }
    }
}
