using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys;



public class Genre
{
    public int Id { get; set; }
    public string Name {get; set;} = null!;



    public ICollection<Event> Events { get; set; } = [];
}