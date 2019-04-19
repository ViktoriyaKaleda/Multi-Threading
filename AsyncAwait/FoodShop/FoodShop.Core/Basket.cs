using FoodShop.Core.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FoodShop.Core
{
    public class Basket
    {
        public decimal TotalCost { get; private set; }

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public async Task AddProduct(Product product)
        {
            Products.Add(product);
            await Task.Delay(500);
            TotalCost += product.Price;
        }

        public async Task RemoveProduct(Product product)
        {
            Products.Remove(product);
            await Task.Delay(500);
            TotalCost -= product.Price;
        }
    }
}
