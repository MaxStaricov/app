using System.Reflection.Metadata.Ecma335;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers;



[ApiController]
[Route("api/[controller]")]
public class EventsController: ControllerBase
{
    private readonly IEventService eventService;
    

    public EventsController(IEventService eventService) {this.eventService = eventService;}

    private static PaginationDTO PaginationMapper(PaginationModel paginationModel)
    {
        return new PaginationDTO {
            Total = paginationModel.Total,
            Page = paginationModel.Page,
            PerPage = paginationModel.PerPage,
            TotalPages = paginationModel.TotalPages
        };
    }
    private static EventPaginationDTO EventPaginationMapper(EventPaginationModel eventPaginationModel)
    {
        return new EventPaginationDTO {
            Pagination = PaginationMapper(eventPaginationModel.Pagination),
            Events = eventPaginationModel.Events
        };
    }
    private static EventDTO DtoMapper(EventModel model)
    {
        return new EventDTO(model.Id, model.Name, model.Genre, model.Beginning, model.Venue, (float)model.BasePrice, model.AvgRating);
    }

    private static List<EventDTO> ListMapper(List<EventModel>? models)
    {
        List<EventDTO> dtos = new List<EventDTO>();
        if(models == null) return dtos;

        foreach(var model in models) {
            dtos.Add(DtoMapper(model));
        }

        return dtos;
    }
    
    // [HttpGet]
    [NonAction]
    public async Task<IActionResult> GetSortedList([FromQuery(Name = "sort")] string[]? fields)
    {
        var events = await eventService.GetSortedList(fields);
        
        var dtos = events.Select(e => new EventDTO(
            e.Id,
            e.Name,
            e.Genre,
            e.Beginning,
            e.Venue,
            (float)e.BasePrice,
            e.AvgRating
        )).ToList();

        return StatusCode(200, new { Events = dtos });
    }



    // [HttpGet]
    [NonAction]
    public async Task<IActionResult> Pagination([FromQuery(Name = "page")] string? page, [FromQuery(Name = "per_page")] string? perPage)
    {
        var pagination = await eventService.GetPaginationEvents(page, perPage);
        return StatusCode(200, EventPaginationMapper(pagination));
    }


    // [HttpGet]
    [NonAction]
    public async Task<IActionResult> Filter(
        [FromQuery(Name = "genre")] string? genre, 
        [FromQuery(Name = "min_price")] string? min_price,
        [FromQuery(Name = "max_price")] string? max_price,
        [FromQuery(Name = "from")] string ? from,
        [FromQuery(Name = "to")] string? to,
        [FromQuery(Name = "name")] string? name)
    {
        var filteredList = await eventService.GetFilteredList(genre, min_price, max_price, from, to, name);
        return StatusCode(200, new {Events = ListMapper(filteredList)});
    }

    [HttpGet]
    public async Task<IActionResult> Selection([FromQuery(Name = "genre")] string? genre, 
                                                    [FromQuery(Name = "min_price")] string? min_price,
                                                    [FromQuery(Name = "max_price")] string? max_price,
                                                    [FromQuery(Name = "from")] string ? from,
                                                    [FromQuery(Name = "to")] string? to,
                                                    [FromQuery(Name = "name")] string? name,
                                                    [FromQuery(Name = "page")] string? page, 
                                                    [FromQuery(Name = "per_page")] string? perPage,
                                                    [FromQuery(Name = "sort")] string[]? fields) 
    {
        var pagination = await eventService.GetSelection(genre, min_price, max_price, from, to, name, fields, page, perPage);
        return StatusCode(200, EventPaginationMapper(pagination));
    }
}