namespace EduService.Application.Dtos
{
    public class ImportResultDto
    {
        public int SavedTotal { get; set; }
        public int ImportTotal { get; set; }
        public int ImportSuccessCount { get; set; }
        public int ImportErrorCount => ImportErrors.Count;
        public List<ImportErrorDto> ImportErrors { get; set; } = new();
    }
}
