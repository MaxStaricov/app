using Models;

namespace DTOs;



public record class EventPaginationDTO
{
    public PaginationDTO Pagination { get; init; }
    public List<EventModel> Events { get; init; }
}