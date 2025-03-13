using ForTheLife.DbContexts;
using ForTheLife.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace ForTheLife.Views
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public OrdersWindow()
        {
            InitializeComponent();

            LoadOrdersData();
            CheckRolePermissions();
        }

        private void CheckRolePermissions()
        {
            var roleId = ApplicationData.CurrentUser!.RoleId;

            if (roleId == 1)
            {
                UpdateOrderButton.Visibility = Visibility.Collapsed;
            }
            else if (roleId == 5)
            {
                UpdateOrderButton.Visibility = Visibility.Visible;
            }
            else if (roleId == 2)
            {
                UpdateOrderButton.Visibility = Visibility.Visible;

            }
           
        }

        private void LoadOrdersData()
        {
            var user = ApplicationData.CurrentUser!;
            var roleId = user.RoleId; 

            using var dbContext = new ForTheLifeDbContext();
            var orders = new List<Order>();
                
            if (roleId == 1)
            {
                orders = dbContext.Orders
                    .Include(x => x.OrdersProducts)
                    .Include(x => x.User)
                    .Where(x => x.UserId == user.Id)
                    .ToList();
            }
            else
            {
                orders = dbContext.Orders
                    .Include(x => x.OrdersProducts)
                    .Include(x => x.User)
                    .ToList();
            }
                

            OrdersCountTB.Text = $"Кол-во заказов {orders.Count}";
            OrdersLV.ItemsSource = orders;
        }

        private void ShowCreateOrderWindow(object sender, RoutedEventArgs e)
        {
            var createOrderWindow = new CreateOrderWindow();
            var result = createOrderWindow.ShowDialog();

            if (result == false) return;
            LoadOrdersData();
        }

        private void ShowUpdateOrderWindow(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersLV.SelectedItem as Order;
            if (selectedOrder == null) return;

            var updateOrderWindow = new UpdateOrderWindow(selectedOrder.Id);    
            var result = updateOrderWindow.ShowDialog();

            if (result == false) return;    
            LoadOrdersData();
        }
    }
}
