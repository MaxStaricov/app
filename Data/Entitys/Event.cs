using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Entitys;



public class Event
{
    public Guid Id {get; set;}
    public string Name {get; set;} = null!;
    public int GenreId {get; set;}
    public DateTime Beginning {get; set;}
    public string Venue {get; set;} = null!;
    public decimal BasePrice {get; set;}



    public ICollection<EventRating> Ratings { get; set; } = [];
    public Genre Genre { get; set; } = null!;

}