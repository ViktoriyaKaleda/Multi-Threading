using FoodShop.Core.Entities;
using System.Collections.Generic;

namespace FoodShop.Core.Storage
{
    public class ProductContext
    {
        public List<Product> Products { get; } = new List<Product>
        {
            new Product { Id = 1, Name = "Milk", Price = 1.10M, Description = "Milk description.", PhotoPath = @"C:\Users\Viktoriya_Kaleda\images\milk.jpg" },
            new Product { Id = 2, Name = "Bread", Price = 0.70M, Description = "Bread description.", PhotoPath = @"C:\Users\Viktoriya_Kaleda\images\bread.jpg"},
            new Product { Id = 3, Name = "Cheese", Price = 3.20M, Description = "Cheese description.", PhotoPath = @"C:\Users\Viktoriya_Kaleda\images\cheese.jpeg"},

        };
    }
}
