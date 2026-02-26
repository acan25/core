namespace NArchitecture.Core.Application.Requests;

public class FilterRequest
{
    public string FilterBy { get; set; } = string.Empty;
    public string FilterValue { get; set; } = string.Empty;
}
