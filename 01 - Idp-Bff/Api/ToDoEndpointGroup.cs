using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

namespace Api;

public static class TodoEndpointGroup
{
    private static readonly List<ToDo> _data =
    [
        new() { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow, Name = "Playing football", User = "2 (Lionel Messi)" },
        new() { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow.AddHours(1), Name = "Stop playing ps5", User = "2 (Lionel Messi)" },
        new() { Id = ToDo.NewId(), Date = DateTimeOffset.UtcNow.AddHours(4), Name = "Have Dinner", User = "1 (Cristiano Ronaldo)" },
    ];

    public static RouteGroupBuilder ToDoGroup(this RouteGroupBuilder group)
    {
        // GET
        group.MapGet("/", () => _data);
        group.MapGet("/{id}", (int id) =>
        {
            var item = _data.FirstOrDefault(x => x.Id == id);
        });

        // POST
        group.MapPost("/", (ToDo model, ClaimsPrincipal user, HttpContext context) =>
        {
            model.Id = ToDo.NewId();
            model.User = $"{user.FindFirst("sub")?.Value} ({user.FindFirst("client_id")?.Value})";

            _data.Add(model);

            var url = new Uri($"{context.Request.GetEncodedUrl()}/{model.Id}");

            return Results.Created(url, model);
        });

        // PUT
        group.MapPut("/{id}", (int id, ToDo model, ClaimsPrincipal User) =>
        {
            var item = _data.FirstOrDefault(x => x.Id == id);
            if (item == null) return Results.NotFound();

            item.Date = model.Date;
            item.Name = model.Name;

            return Results.NoContent();
        });

        // DELETE
        group.MapDelete("/{id}", (int id) =>
        {
            var item = _data.FirstOrDefault(x => x.Id == id);
            if (item == null) return Results.NotFound();

            _data.Remove(item);

            return Results.NoContent();
        });

        return group;
    }
}

public class ToDo
{
    static int _nextId = 1;
    public static int NewId()
    {
        return _nextId++;
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Name { get; set; }
    public string? User { get; set; }
}