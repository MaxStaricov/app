using Entitys;

namespace Models;



public class EventModel
{
    public Guid Id {get; set;}
    public string Name {get; set; } = null!;
    public string Genre {get; set;} = null!;
    public DateTime Beginning {get; set;}
    public string Venue {get; set;} = null!;
    public decimal BasePrice {get; set;}
    public double? AvgRating {get; set;}


    public EventModel() {}
    public EventModel(Guid id, string name, string genre, DateTime beginning, string venue, decimal basePrice, double? avgRating)
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