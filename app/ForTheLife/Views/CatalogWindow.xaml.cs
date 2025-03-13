using ForTheLife.DbContexts;
using ForTheLife.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;


namespace ForTheLife.Views;
public partial class CatalogWindow : Window
{
    public CatalogWindow()
    {
        InitializeComponent();

        LoadSaleFilterData();
        LoadProductsData();
        CheckAuthState();
    }

    private void CheckAuthState()
    {
        var authUser = ApplicationData.CurrentUser;
        if (authUser == null)
        {
            AuthPanel.Visibility = Visibility.Collapsed;
            NotAuthPanel.Visibility = Visibility.Visible;

            CheckRolePermissions(0);
            return;
        }

        AuthPanel.Visibility = Visibility.Visible;
        NotAuthPanel.Visibility = Visibility.Collapsed;

        UsernameTB.Text = $"Имя пользователя: {authUser.Username}.";
        FullNameTB.Text = $"ФИО: {authUser.Lastname} {authUser.Firstname} {authUser.Middlename}.";
        RoleTB.Text = $"Роль: {authUser.Role.Title}.";

        CheckRolePermissions(authUser.RoleId);
    }

    private void CheckRolePermissions(int roleId)
    {
        if (roleId == 1)
        {
            AdminProductsButtonsPanel.Visibility = Visibility.Collapsed;
            OrdersButtonsPanel.Visibility = Visibility.Visible;
        }
        else if (roleId == 5)
        {
            AdminProductsButtonsPanel.Visibility = Visibility.Collapsed;
            OrdersButtonsPanel.Visibility = Visibility.Visible;
        }
        else if (roleId == 2)
        {
            AdminProductsButtonsPanel.Visibility = Visibility.Visible;
            OrdersButtonsPanel.Visibility = Visibility.Visible;
        }
        else
        {
            AdminProductsButtonsPanel.Visibility = Visibility.Collapsed;
            OrdersButtonsPanel.Visibility = Visibility.Collapsed;
        }
    }

    private void LoadSaleFilterData()
    {
        var saleFilters = new List<string>
        {
            "Все диапозоны",
            "0 - 24%",
            "25 - 49%",
            "50 - 74%",
            "75 - 100%"
        };
        SaleFilterCB.ItemsSource = saleFilters;

        var defaultSelectedSaleFilter = SaleFilterCB.Items.Cast<string>().SingleOrDefault(x => x == "Все диапозоны");
        SaleFilterCB.SelectedItem = defaultSelectedSaleFilter;
    }

    private void LoadProductsData()
    {
        var saleFilter = SaleFilterCB.SelectedItem as string;
        using var dbContext = new ForTheLifeDbContext();

        var productsQuary = dbContext.Products
            .Include(x => x.Category)
            .Include(x => x.ProductName);
        var products = new List<Product>();

        var searchTerms = SearchTB.Text;

        switch (saleFilter)
        {
            case "0 - 24%":
                if (!string.IsNullOrWhiteSpace(searchTerms))
                    products = productsQuary.Where(x => x.ProductName.Title.ToLower().Contains(searchTerms.ToLower()) && x.CurrentSale >= 0 && 24 >= x.CurrentSale).ToList();
                else
                    products = productsQuary.Where(x => x.CurrentSale >= 0 && 24 >= x.CurrentSale).ToList();
                break;
            case "25 - 49%":
                if (!string.IsNullOrWhiteSpace(searchTerms))
                    products = productsQuary.Where(x => x.ProductName.Title.ToLower().Contains(searchTerms.ToLower()) && x.CurrentSale >= 25 && 49 >= x.CurrentSale).ToList();
                else
                    products = productsQuary.Where(x => x.CurrentSale >= 25 && 49 >= x.CurrentSale).ToList();
                break;
            case "50 - 74%":
                if (!string.IsNullOrWhiteSpace(searchTerms))
                    products = productsQuary.Where(x => x.ProductName.Title.ToLower().Contains(searchTerms.ToLower()) && x.CurrentSale >= 50 && 74 >= x.CurrentSale).ToList();
                else
                    products = productsQuary.Where(x => x.CurrentSale >= 50 && 74 >= x.CurrentSale).ToList();
                break;
            case "75 - 100%":
                if (!string.IsNullOrWhiteSpace(searchTerms))
                    products = productsQuary.Where(x => x.ProductName.Title.ToLower().Contains(searchTerms.ToLower()) && x.CurrentSale >= 75 && 100 >= x.CurrentSale).ToList();
                else
                    products = productsQuary.Where(x => x.CurrentSale >= 74 && 100 >= x.CurrentSale).ToList();
                break;
            default:
                if (!string.IsNullOrWhiteSpace(searchTerms))
                    products = productsQuary.Where(x => x.ProductName.Title.ToLower().Contains(searchTerms.ToLower())).ToList();
                else
                    products = productsQuary.ToList();
                break;
        }

        ProductsCountTB.Text = $"Найдено {products.Count} товаров";
        ProductsLV.ItemsSource = products;
    }

    private void ShowSignInWindow(object sender, RoutedEventArgs e)
    {
        var signInWindow = new SignInWindow();
        var result = signInWindow.ShowDialog();
        if (result == false) return;
        
        CheckAuthState();
    }

    private void ShowSignUpWindow(object sender, RoutedEventArgs e)
    {
        var signUpWindow = new SignUpWindow();
        signUpWindow.ShowDialog();
    }

    private void SignOut(object sender, RoutedEventArgs e)
    {
        ApplicationData.CurrentUser = null;
        CheckAuthState();
    }

    private void ShowCreateProductWindow(object sender, RoutedEventArgs e)
    {
        var createProductWindow = new CreateProductWindow();
        var result = createProductWindow.ShowDialog();

        if (result == false) return;
        LoadProductsData();
    }

    private void ShowUpdateProductWindow(object sender, RoutedEventArgs e)
    {
        var selectedProduct = ProductsLV.SelectedItem as Product;
        if (selectedProduct == null) return;

        var updateProductWindow = new UpdateProductWindow(selectedProduct.Id);
        var result = updateProductWindow.ShowDialog();

        if (result == false) return;
        LoadProductsData();
    }

    private void OnSaleFilterChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) => LoadProductsData();

    private void SearchProducts(object sender, System.Windows.Controls.TextChangedEventArgs e) => LoadProductsData();

    private void ShowOrdersWindow(object sender, RoutedEventArgs e)
    {
        var ordersWindow = new OrdersWindow();
        ordersWindow.ShowDialog();
    }
}
