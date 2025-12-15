namespace SyncFlow.Application.DTOs.Shared
{
    public class PagedFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string[]? OrderBy { get; set; }         // ej: ["Name", "StartDate"]
        public string[]? OrderDirection { get; set; }  // ej: ["asc", "desc"]
    }


}
