var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


builder.WebHost.UseUrls("http://0.0.0.0:80");

app.MapGet("/echo/{message}", (string message) =>
{
    return Results.Json(new { echo = message });
});

app.Run();