using Risparmioforo.Domain.Common;

namespace Risparmioforo.Services.CategoryService;

public class CreateCategoryCommand
{
    public string Name { get; set; }
    public Flow Flow { get; set; }
    public List<string> Keywords { get; set; } = [];
}

public class UpdateCategoryCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Keywords { get; set; } = [];
}

public class RemoveCategoryCommand
{
    public int Id { get; set; }
}