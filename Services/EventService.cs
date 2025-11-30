using System.Linq.Expressions;
using Data;
using Entitys;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services;



public class EventService: IEventService
{
    public readonly AppDbContext db;
    
    
    public EventService(AppDbContext appDbContext) => db = appDbContext;

    private IQueryable<EventModel> GetQueryEventModel()
    {
        return db.Events
            .AsNoTracking()
            .Select(e => new EventModel {
                Id = e.Id,
                Name = e.Name,
                Genre = e.Genre.Name,
                Beginning = e.Beginning,
                Venue = e.Venue,
                BasePrice = e.BasePrice,
                AvgRating = e.Ratings.Select(r => (double?)r.Rating).Average() 
                // AvgRating = e.Ratings.Select(r => (double?)r.Rating).DefaultIfEmpty(null).Average() 
        });
    } 

    private static readonly Dictionary<string, Expression<Func<EventModel, object>>> SortMap = new() {
        ["price"]     = e => e.BasePrice,
        ["beginning"] = e => e.Beginning,
        ["avgRating"] = e => e.AvgRating
    };

    private static IQueryable<EventModel> SortQuery(IQueryable<EventModel> query, string[]? fields)
    {
        if (fields == null || fields.Length <= 0)
            return query.OrderBy(e => e.Id);

        IOrderedQueryable<EventModel>? orderedQuery = null;

        foreach (var f in fields) {
            if (string.IsNullOrWhiteSpace(f))
                continue;

            bool reverse = f.StartsWith("-");
            string field = reverse ? f.Substring(1) : f;

            SortMap.TryGetValue(field, out var expr);
            expr = (expr is not null) ? expr : e => e.Id;

            orderedQuery = (orderedQuery == null) ? 
                (reverse ? query.OrderByDescending(expr) : query.OrderBy(expr)) :
                (reverse ? orderedQuery.ThenByDescending(expr) : orderedQuery.ThenBy(expr));
        }

        orderedQuery = (orderedQuery ?? query.OrderBy(e => e.Id)).ThenBy(e => e.Id);

        return orderedQuery;
    }

    public async Task<List<EventModel>> GetSortedList(string[]? fields) 
    {
        var query = GetQueryEventModel();
        return await SortQuery(query, fields).ToListAsync();
    }


    public async Task<EventPaginationModel> GetPaginationEvents(string? pageNumber, string? pageSize) 
    {
        int pnum = int.TryParse(pageNumber, out int parsedPageNumber) ? parsedPageNumber : 1;
        int psize = int.TryParse(pageSize, out int parsedPageSize) ? parsedPageSize : 10;

        pnum = Math.Max(1, pnum);
        psize = Math.Max(1, psize);

        var query = GetQueryEventModel();

        var orderedQuery = query.OrderBy(e => e.Id);

        PaginationModel pagination = await BuildPagination(orderedQuery, pnum, psize);

        int skip = (pnum - 1) * psize;
        int totalItems = pagination.Total;
        int perPage = pagination.PerPage;
        List<EventModel> items = (skip >= totalItems) ?
            new List<EventModel>() :
            await orderedQuery.Skip(skip).Take(perPage).ToListAsync();
        
        return new EventPaginationModel(pagination, items);
    }



    private IQueryable<Event> FilterQuery(IQueryable<Event> query, string? genre, decimal? min_price, decimal? max_price, DateTime? from, DateTime? to, string? name) 
    {
        if (!string.IsNullOrEmpty(genre))
            query = query.Where(e => e.Genre.Name == genre);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(e => e.Name.Contains(name));

        if (min_price.HasValue)
            query = query.Where(e => e.BasePrice >= min_price.Value);

        if (max_price.HasValue)
            query = query.Where(e => e.BasePrice <= max_price.Value);

        if (from.HasValue)
            query = query.Where(e => e.Beginning >= from.Value);

        if (to.HasValue)
            query = query.Where(e => e.Beginning <= to.Value);

        return query;
    }

    public async Task<List<EventModel>> GetFilteredList(string? genre, string? min_price, string? max_price, string? from, string? to, string? name) 
    {
        string? _genre = string.IsNullOrWhiteSpace(genre) ? null : genre.Trim();
        decimal? _minPrice = decimal.TryParse(min_price, out decimal minResult) ? minResult : null;
        decimal? _maxPrice = decimal.TryParse(max_price, out decimal maxResult) ? maxResult : null;
        DateTime? _from = DateTime.TryParse(from, out DateTime fromResult) ? fromResult.ToUniversalTime() : null;
        DateTime? _to = DateTime.TryParse(to, out DateTime toResult) ? toResult.ToUniversalTime() : null;
        string? _name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();

        // var query = GetQueryAsNoTracking();
        var query = db.Events.AsNoTracking();

        query = FilterQuery(query, _genre, _minPrice, _maxPrice, _from, _to, _name);
        query = query.OrderBy(e => e.Id);
        
        // return await query.ToListAsync();
        return await query.Select(e => new EventModel {
            Id = e.Id,
            Name = e.Name,
            Genre = e.Genre.Name,
            Beginning = e.Beginning,
            Venue = e.Venue,
            BasePrice = e.BasePrice,
            AvgRating = e.Ratings.Select(r => (double?)r.Rating).Average()
        }).ToListAsync();

    }

    private static async Task<PaginationModel> BuildPagination(IQueryable<EventModel>? query, int pnum, int psize) 
    {
        if(query is null) return new PaginationModel(0, pnum, 0, 0);

        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)psize);
        int skip = (pnum - 1) * psize;
        // int jump = pnum * psize;
        // int perPage = (jump <= totalItems) ? jump - skip : totalItems - skip;
        int perPage = Math.Max(0, Math.Min(psize, totalItems - skip));


        return new PaginationModel(totalItems, pnum, perPage, totalPages);
    }

    public async Task<EventPaginationModel> GetSelection(string? genre, string? min_price, string? max_price, string? from, string? to, string? name, 
                                                    string[]? fields, 
                                                    string? pageNumber, string? pageSize)
    {
        string? _genre = string.IsNullOrWhiteSpace(genre) ? null : genre.Trim();
        decimal? _minPrice = decimal.TryParse(min_price, out decimal minResult) ? minResult : null;
        decimal? _maxPrice = decimal.TryParse(max_price, out decimal maxResult) ? maxResult : null;
        DateTime? _from = DateTime.TryParse(from, out DateTime fromResult) ? fromResult.ToUniversalTime() : null;
        DateTime? _to = DateTime.TryParse(to, out DateTime toResult) ? toResult.ToUniversalTime() : null;
        string? _name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();

        int pnum = Math.Max(1, int.TryParse(pageNumber, out int parsedPageNumber) ? parsedPageNumber : 1);
        int psize = Math.Max(1, int.TryParse(pageSize, out int parsedPageSize) ? parsedPageSize : 10);

        var filteredQuery = FilterQuery(db.Events.AsNoTracking(), _genre, _minPrice, _maxPrice, _from, _to, _name);
        var query = filteredQuery.Select(e => new EventModel {
            Id = e.Id,
            Name = e.Name,
            Genre = e.Genre.Name,
            Beginning = e.Beginning,
            Venue = e.Venue,
            BasePrice = e.BasePrice,
            AvgRating = e.Ratings.Select(r => (double?)r.Rating).Average()
        });

        IQueryable<EventModel> sortedQuery = SortQuery(query, fields);
        PaginationModel pagination = await BuildPagination(sortedQuery, pnum, psize);

        int skip = (pnum - 1) * psize;
        int totalItems = pagination.Total;
        int perPage = pagination.PerPage;
        List<EventModel> items = (skip >= totalItems) ? 
            new List<EventModel>() : 
            await sortedQuery.Skip(skip).Take(perPage).ToListAsync();

        return new EventPaginationModel(pagination, items);
    }
}