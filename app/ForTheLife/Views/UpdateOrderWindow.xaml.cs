using ForTheLife.DbContexts;
using ForTheLife.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows;
using ForTheLife.Models;

namespace ForTheLife.Views;

public partial class UpdateOrderWindow : Window
{
    private int _orderId;
    private List<ProductInOrder> ProductsInOrder = new List<ProductInOrder>(); 

    public UpdateOrderWindow(int orderId)
    {
        InitializeComponent();

        _orderId = orderId;

        CheckUserPermissions();

        LoadProductsData();
        LoadOrdersStatusesData();
        LoadOrderData();
    }

    private void CheckUserPermissions()
    {
        var user = ApplicationData.CurrentUser;
        var roleId = user.RoleId;

        if (roleId == 1)
        {
            ChooseStatusPanel.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Visible;

        }
        else if (roleId == 5)
        {
            ChooseStatusPanel.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;

        }
        else if (roleId == 2)
        {
            ChooseStatusPanel.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

    }

    private void LoadProductsData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var products = dbContext.Products.Include(x => x.ProductName).ToList();
        ProductsLV.ItemsSource = products;
    }

    private void LoadOrdersStatusesData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var ordersStatuses = dbContext.OrdersStatuses.ToList();
        OrdersStatusesCB.ItemsSource = ordersStatuses;
    }

    private void LoadOrderData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var order = dbContext.Orders
            .Include(x => x.OrdersProducts)
            .ThenInclude(x => x.Product)
            .SingleOrDefault(x => x.Id == _orderId);
        if (order == null)
        {
            MessageBox.Show("Заказ не найден!");
            DialogResult = false;
            return;
        }

        var productsInOrder = new List<ProductInOrder>();
        foreach (var orderProduct in order.OrdersProducts)
        {
            if (orderProduct.OrderId != order.Id) continue;

            var productInOrder = new ProductInOrder() 
            {
                Product = orderProduct.Product,
                Quantity = orderProduct.Quantity
            };
            ProductsInOrder.Add(productInOrder);
        }

        LoadProductsInOrderData();


        var selectedOrderStatus = OrdersStatusesCB.Items.Cast<OrdersStatus>().SingleOrDefault(x => x.Id == order.OrdersStatusId);
        OrdersStatusesCB.SelectedItem = selectedOrderStatus;
    }

    private void UpdateOrder(object sender, RoutedEventArgs e)
    {
        try
        {
            var selectedProductsInOrder = ProductsInOrder;
            var selectedOrderStatus = OrdersStatusesCB.SelectedItem as OrdersStatus;

            using var dbContext = new ForTheLifeDbContext();
            var order = dbContext.Orders.Include(x => x.OrdersProducts).SingleOrDefault(x => x.Id == _orderId);
            if (order == null)
            {
                MessageBox.Show("Заказ не найден!");
                DialogResult = false;
                return;
            }

            if (!selectedProductsInOrder.Any())
            {
                MessageBox.Show("Товары не выбраны!");
                return;
            }

            var ordersProducts = dbContext.OrdersProducts.ToList();
            dbContext.OrdersProducts.RemoveRange(ordersProducts);

            order.OrdersStatusId = selectedOrderStatus.Id;
            foreach (var productInOrder in selectedProductsInOrder)
            {
                var ordersProduct = new OrdersProduct()
                {
                    OrderId = order.Id,
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

    private void DeleteOrder(object sender, RoutedEventArgs e)
    {
        try
        {
            using var dbContext = new ForTheLifeDbContext();
            var order = dbContext.Orders.Include(x => x.OrdersProducts).SingleOrDefault(x => x.Id == _orderId);
            if (order == null)
            {
                MessageBox.Show("Заказ не найден!");
                DialogResult = false;
                return;
            }



            var ordersProducts = dbContext.OrdersProducts.ToList();
            dbContext.OrdersProducts.RemoveRange(ordersProducts);

            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }


    }

    private void LoadProductsInOrderData()
    {
        ProductsInOrderLV.ItemsSource = new List<ProductInOrder>();
        ProductsInOrderLV.ItemsSource = ProductsInOrder;

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

            LoadProductsInOrderData();

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

            LoadProductsInOrderData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
