namespace DTOs;




public record class PaginationDTO
{
    public int Total { get; init; }
    public int Page { get; init; }
    public int PerPage { get; init; }
    public int TotalPages { get; init; }
}