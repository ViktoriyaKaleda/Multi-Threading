using DevExpress.Mvvm;
using FoodShop.Core;
using FoodShop.Core.Entities;
using FoodShop.Core.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.WpfApp
{
    public class MainViewModel : BindableBase
    {
        public List<Product> Products { get; } = new ProductContext().Products;

        public Basket Basket { get; } = new Basket();

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public string BasketMessage
        {
            get { return GetProperty(() => BasketMessage); }
            set { SetProperty(() => BasketMessage, value); }
        }

        public ICommand AddProductToBasket
           => new DelegateCommand<int>(async (int productId) =>
           {
               var product = Products.First(p => p.Id == productId);
               var messageAdding = new Message { Content = $"Adding product '{product.Name}' to basket..." };
               Messages.Add(messageAdding);
               await Basket.AddProduct(product);
               Messages.Remove(messageAdding);
               var messageAdded = new Message { Content = $"Product '{product.Name}' is added to basket." };
               Messages.Add(messageAdded);
               BasketMessage = $"Basket total cost: {Basket.TotalCost}";
               await Task.Delay(2000);
               Messages.Remove(messageAdded);
           });
    }
}
