using Entitys;

namespace Models;



public class EventPaginationModel
{
    public PaginationModel Pagination {get; set;}
    public List<EventModel> Events {get; set;}

    public EventPaginationModel() {}
    public EventPaginationModel(PaginationModel pagination, List<EventModel> events)
    {
        Pagination = pagination;
        Events = events;
    }
}