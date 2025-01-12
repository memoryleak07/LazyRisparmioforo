namespace Risparmioforo.Domain.Category;

public class CategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Flow { get; set; } = string.Empty;
    public List<string> Keywords { get; set; } = [];
}

public static class CategoryMappingExtension
{
    public static ICollection<CategoryDto> ToDto(this ICollection<Category> categories) =>
        categories.Select(t => t.ToDto()).ToList();

    public static CategoryDto ToDto(this Category category) => 
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Flow = category.Flow.ToString(),
            Keywords = category.Keywords,
        };
}