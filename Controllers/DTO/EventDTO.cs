using Models;

namespace DTOs;



public record class EventDTO
{
    public Guid Id {get; init;}
    public String Name {get; init;}
    public String Genre {get; init;}
    public DateTime Beginning {get; init;}
    public String Venue {get; init;}
    public float BasePrice {get; init;}
    public double? AvgRating {get; init;}

    public EventDTO(Guid id, String name, String genre, DateTime beginning, string venue, float basePrice, double? avgRating)
    {
        Id = id;
        Name = name;
        Genre = genre;
        Beginning = beginning;
        Venue = venue;
        BasePrice = basePrice;
        AvgRating = avgRating;
    }
}

public record class EventDTOList
{
    public List<EventModel> events {get; init;}
}