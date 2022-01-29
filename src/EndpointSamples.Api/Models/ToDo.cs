namespace EndpointSamples.Api.Models;

public class ToDo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}