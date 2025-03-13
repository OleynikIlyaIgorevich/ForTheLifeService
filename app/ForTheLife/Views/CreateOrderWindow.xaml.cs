using ForTheLife.DbContexts;
using ForTheLife.Entities;
using ForTheLife.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace ForTheLife.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private List<ProductInOrder> ProductsInOrder = [];

        public CreateOrderWindow()
        {
            InitializeComponent();

            CheckUserPermissions();

            LoadClientsData();
            LoadProductsData();
        }

        private void CheckUserPermissions()
        {
            var user = ApplicationData.CurrentUser;
            var roleId = user.RoleId;

            if (roleId == 1)
            {
                ChooseClientPanel.Visibility = Visibility.Collapsed;
            }
            else if (roleId == 5)
            {
                ChooseClientPanel.Visibility = Visibility.Visible;
            }
            else if (roleId == 2)
            {
                ChooseClientPanel.Visibility = Visibility.Visible;
            }
         
        }


        private void LoadClientsData()
        {
            using var dbContext = new ForTheLifeDbContext();
            var clients = dbContext.Users.Where(x => x.RoleId == 1).ToList();
            ClientsLV.ItemsSource = clients;
        }

        private void LoadProductsData()
        {
            using var dbContext = new ForTheLifeDbContext();
            var products = dbContext.Products.Include(x => x.ProductName).ToList();
            ProductsLV.ItemsSource = products;
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                User selectedClient = ApplicationData.CurrentUser!.RoleId == 1 
                    ? ApplicationData.CurrentUser!
                    : ClientsLV.SelectedItem as User;
                var selectedProductsInOrder = ProductsInOrder;

                if (!selectedProductsInOrder.Any())
                {
                    MessageBox.Show("Товары не выбраны!");
                    return;
                }

                var order = new Order()
                {
                    UserId = selectedClient.Id,
                    OrdersStatusId = 1
                };

                using var dbContext = new ForTheLifeDbContext();

                var createdOrder = dbContext.Orders.Add(order);

                dbContext.SaveChanges();

                foreach (var productInOrder in selectedProductsInOrder)
                {
                    var ordersProduct = new OrdersProduct() 
                    {
                        OrderId = createdOrder.Entity.Id,
                        ProductId = productInOrder.Product.Id,
                        Quantity = productInOrder.Quantity 
                    };
                    dbContext.OrdersProducts.Add(ordersProduct);
                }

                dbContext.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void AddProductToOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = ProductsLV.SelectedItem as Product;
                if (selectedProduct == null) return;

                var quantity = Convert.ToInt32(QuantityTB.Text);

                var productCart = new ProductInOrder()
                {
                    Product = selectedProduct,
                    Quantity = quantity
                };
                ProductsInOrder.Add(productCart);

                LoadProductsCartData();

                ProductsInOrderLV.SelectedItem = null;
                QuantityTB.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void DeleteProductFromOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = ProductsInOrderLV.SelectedItem as ProductInOrder;
                if (selectedProduct == null) return;

                ProductsInOrder.Remove(selectedProduct);

                LoadProductsCartData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadProductsCartData()
        {
            ProductsInOrderLV.ItemsSource = new List<ProductInOrder>();
            ProductsInOrderLV.ItemsSource = ProductsInOrder;
        }

      
    }
}
