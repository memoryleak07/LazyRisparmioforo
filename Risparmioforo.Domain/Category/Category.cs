using Risparmioforo.Domain.Common;

namespace Risparmioforo.Domain.Category;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Flow Flow { get; set; } = Flow.Undefined;
    public List<string> Keywords { get; set; } = [];
}