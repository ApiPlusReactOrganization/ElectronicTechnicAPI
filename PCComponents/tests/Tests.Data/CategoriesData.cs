using Domain.Categories;

namespace Tests.Data;

public static class CategoriesData
{
    public static Category MainCategory => Category.New(
        id: CategoryId.New(),
        name: "TestCategory"
    );
}