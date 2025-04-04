
using Dtos;
using System.Collections.Generic;

namespace Dtos;

public static class ProductDataGenerator
{
    public static List<ProductInputDto> Generate(int count)
    {
        var list = new List<ProductInputDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new ProductInputDto
            {
                Name = $"Product {i}",
                Description = $"Description {i}",
                Category = $"Category {i % 5}",
                Stock = i * 10
            });
        }
        return list;
    }
}
