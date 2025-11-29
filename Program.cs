var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/echo/{message}", (string message) =>
{
    return Results.Json(new { echo = message });
});

app.Run();