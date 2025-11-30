var PORT = Environment.GetEnvironmentVariable("PORT") ?? "80";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://0.0.0.0:{PORT}");
var app = builder.Build();


app.MapGet("/echo/{message}", (string message) =>
{
    return Results.Json(new { echo = message });
});

 app.MapGet("/health", () => Results.Ok("OK"));


app.Run();