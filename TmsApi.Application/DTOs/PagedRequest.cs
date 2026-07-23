namespace TmsApi.Application.DTOs;
public record PagedRequest
{
private const int MaxPageSize = 50;
public int Page { get; init; } = 1;

public int PageSize
{
    get;
    init => field = value < 1 ? 20 : value > MaxPageSize ? MaxPageSize : value;
} = 20;

public string? Search { get; init; }
public string OrderBy { get; init; } = "Title";
public bool Descending { get; init; }
}