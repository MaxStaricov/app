using Entitys;
using Microsoft.EntityFrameworkCore;

namespace Data;



public class AppDbContext : DbContext
{
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventRating> EventRatings { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Rock" },
            new Genre { Id = 2, Name = "Pop" },
            new Genre { Id = 3, Name = "Jazz" }
        );

        modelBuilder.Entity<Event>().HasData(
        new Event
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Rock Concert",
            GenreId = 1,
            Beginning = new DateTime(2025, 12, 5, 19, 0, 0, DateTimeKind.Utc),
            Venue = "Stadium A",
            BasePrice = 50
        },
        new Event
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Rock Concert",
            GenreId = 1,
            Beginning = new DateTime(2025, 12, 5, 19, 0, 0, DateTimeKind.Utc),
            Venue = "Stadium A",
            BasePrice = 50
        },
        new Event
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Jazz Night",
            GenreId = 3,
            Beginning = new DateTime(2025, 12, 15, 20, 0, 0, DateTimeKind.Utc),
            Venue = "Club C",
            BasePrice = 40
        },
        new Event
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Pop Festival",
            GenreId = 2,
            Beginning = new DateTime(2025, 12, 10, 18, 0, 0, DateTimeKind.Utc),
            Venue = "Arena B",
            BasePrice = 50
        }
        );

        modelBuilder.Entity<EventRating>().HasData(
            new EventRating { Id = 1, EventId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Rating = 4.5 },
            new EventRating { Id = 2, EventId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Rating = 3.8 },
            new EventRating { Id = 3, EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Rating = 4.9 },
            new EventRating { Id = 4, EventId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Rating = 4.2 },
            new EventRating { Id = 5, EventId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Rating = 4.5 }

        );
    }
}