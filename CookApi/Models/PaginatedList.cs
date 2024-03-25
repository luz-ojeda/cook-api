using Microsoft.EntityFrameworkCore;

namespace CookApi.Models;

public class PaginatedList<T>
{
    public List<T> Data { get; set; } = [];
    public Pagination Pagination { get; set; }

    public PaginatedList(List<T> data, Pagination pagination)
    {
        Data = data;
        Pagination = pagination;
    }
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int limit)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * limit).Take(limit).ToListAsync();
        var pagination = new Pagination(page, limit, count);

        return new PaginatedList<T>(items, pagination);
    }
}

public class Pagination
{
    public Pagination(int pageNumber, int pageSize, int count)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalRecords = count;
    }

    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
}