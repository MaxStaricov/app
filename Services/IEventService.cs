using Models;

namespace Services;



public interface IEventService
{
    Task<List<EventModel>> GetSortedList(string[]? fields);
    Task<EventPaginationModel> GetPaginationEvents(string? pageNumber, string? pageSize);
    Task<List<EventModel>> GetFilteredList(string? genre, string? min_price, string? max_price, string? from, string? to, string? name);
    Task<EventPaginationModel> GetSelection(string? genre, string? min_price, string? max_price, string? from, string? to, string? name,
                                        string[]? fields,
                                        string? pageNumber, string? pageSize);
}