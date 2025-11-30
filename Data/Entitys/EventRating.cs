using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys;



public class EventRating
{
    public int Id { get; set; } 
    public Guid EventId { get; set; }
    public double Rating { get; set; }



    public Event Event { get; set; } = null!;
}