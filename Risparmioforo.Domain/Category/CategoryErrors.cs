using Risparmioforo.Shared.Base;

namespace Risparmioforo.Domain.Category;

public abstract class CategoryErrors
{
    public static Error NotFound(int id) => new(
        $"The category with ID = '{id}' was not found.",
        "Category.NotFound");
    
    public static Error NotUnique => new(
        "The category is not unique.",
        "Category.NotUnique");
}
