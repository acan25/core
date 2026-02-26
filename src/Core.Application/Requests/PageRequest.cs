namespace NArchitecture.Core.Application.Requests;

public class PageRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<FilterRequest>? Filters { get; set; }
}
