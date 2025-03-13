using ForTheLife.DbContexts;
using ForTheLife.Entities;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ForTheLife.Views;

/// <summary>
/// Логика взаимодействия для UpdateProductWindow.xaml
/// </summary>
public partial class UpdateProductWindow : Window
{
    private string baseImagesPath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images");
    private string defaultImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images\\picture.png");
    private string? selectedImagePath;

    private string? oldImageName;

    private readonly int _productId;
   

    public UpdateProductWindow(int productId)
    {
        InitializeComponent();

        _productId = productId;

        LoadCategoriesData();
        LoadUnitsData();
        LoadSuppliersData();
        LoadProducersData();
        LoadProductsNamesData();

        LoadProductData();
    }

    private void LoadCategoriesData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var categories = dbContext.Categories.ToList();
        CategoriesCB.ItemsSource = categories;
    }

    private void LoadUnitsData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var units = dbContext.Units.ToList();
        UnitsCB.ItemsSource = units;
    }

    private void LoadSuppliersData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var suppliers = dbContext.Suppliers.ToList();
        SuppliersCB.ItemsSource = suppliers;
    }

    private void LoadProducersData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var producers = dbContext.Producers.ToList();
        ProducersCB.ItemsSource = producers;
    }

    private void LoadProductsNamesData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var productsNames = dbContext.ProductNames.ToList();
        ProductsNamesCB.ItemsSource = productsNames;
    }

    private void LoadProductData()
    {
        using var dbContext = new ForTheLifeDbContext();
        var product = dbContext.Products
            .SingleOrDefault(x => x.Id == _productId);
        if (product == null)
        {
            MessageBox.Show("Товар не найден!");
            DialogResult = false;
            return;
        }

        ArticleTB.Text = product.Article;

        var selectedProductName = ProductsNamesCB.Items.Cast<ProductName>().SingleOrDefault(x => x.Id == product.ProductNameId);
        ProductsNamesCB.SelectedItem = selectedProductName;

        var selectedCategory = CategoriesCB.Items.Cast<Category>().SingleOrDefault(x => x.Id == product.CategoryId);
        CategoriesCB.SelectedItem = selectedCategory;

        var selectedProducer = ProducersCB.Items.Cast<Producer>().SingleOrDefault(x => x.Id == product.ProducerId);
        ProducersCB.SelectedItem = selectedProducer;

        var selectedSupplier = SuppliersCB.Items.Cast<Supplier>().SingleOrDefault(x => x.Id == product.SupplierId);   
        SuppliersCB.SelectedItem = selectedSupplier;

        var selectedUnit = UnitsCB.Items.Cast<Unit>().SingleOrDefault(x => x.Id == product.UnitId);
        UnitsCB.SelectedItem = selectedUnit;

        PriceTB.Text = product.Price.ToString();
        MaxSaleTB.Text = product.MaxSale.ToString();
        CurrentSaleTB.Text = product.CurrentSale.ToString();
        CountTB.Text = product.Count.ToString();
        DesciptionTB.Text = product.Description;

        if (!string.IsNullOrWhiteSpace(product.ImageUrl))
        {
            oldImageName = product.ImageUrl;
            selectedImagePath = Path.Combine(baseImagesPath, product.ImageUrl);
            SelectedImage.Source = new BitmapImage(new Uri(selectedImagePath, UriKind.RelativeOrAbsolute));
        }
        else
        {
            SelectedImage.Source = new BitmapImage(new Uri(defaultImagePath, UriKind.RelativeOrAbsolute));
        }
    }

    private void LoadImageUrl(object sender, RoutedEventArgs e)
    {
        var imageFileDialog = new OpenFileDialog
        {
            Title = "Выберите картинку для товара",
            Filter = "Графические изображения |*.jpg;*.jpeg;*.png|" +
                     "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                     "PNG (*.png)|*.png"
        };
        if (imageFileDialog.ShowDialog() == false)
        {
            selectedImagePath = null;
            SelectedImage.Source = new BitmapImage(new Uri(defaultImagePath, UriKind.RelativeOrAbsolute));
            return;
        }

        selectedImagePath = imageFileDialog.FileName;
        SelectedImage.Source = new BitmapImage(new Uri(imageFileDialog.FileName));
    }

    private void UpdateProduct(object sender, RoutedEventArgs e)
    {
        try
        {
            var article = ArticleTB.Text;
            var selectedProductName = ProductsNamesCB.SelectedItem as ProductName;
            var selectedCategory = CategoriesCB.SelectedItem as Category;
            var selectedProducer = ProducersCB.SelectedItem as Producer;
            var selectedSupplier = SuppliersCB.SelectedItem as Supplier;
            var selectedUnit = UnitsCB.SelectedItem as Unit;
            var price = Convert.ToDecimal(PriceTB.Text);
            var maxSale = Convert.ToInt32(MaxSaleTB.Text);
            var currentSale = Convert.ToInt32(CurrentSaleTB.Text);
            var count = Convert.ToInt32(CountTB.Text);
            var description = DesciptionTB.Text;
            var selectedImageName = Path.GetFileName(selectedImagePath);

            var isValidCurrentSale = maxSale >= currentSale;
            if (!isValidCurrentSale)
            {
                MessageBox.Show("Максимальная скидка не может быть меньше чем действующая скидка");
                return;
            }

            using var dbContext = new ForTheLifeDbContext();
            var product = dbContext.Products
                .SingleOrDefault(x => x.Id == _productId);
            if (product == null)
            {
                MessageBox.Show("Товар не найден!");
                DialogResult = false;
                return;
            }

            if (selectedImagePath != null && oldImageName != selectedImageName)
            {
                var isImagesDirectoryExists = Directory.Exists(baseImagesPath);
                if (!isImagesDirectoryExists) Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Resources\\Images"));

                var imageName = Path.GetFileName(selectedImagePath);
                var imagesPath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images");
                var saveFilePath = Path.Combine(imagesPath, imageName);
                File.Copy(selectedImagePath, saveFilePath, true);
            }


            product.Article = article;
            product.ProductNameId = selectedProductName.Id;
            product.CategoryId = selectedCategory.Id;
            product.ProducerId = selectedProducer.Id;
            product.SupplierId = selectedSupplier.Id;
            product.UnitId = selectedUnit.Id;
            product.Price = price;
            product.MaxSale = maxSale;
            product.CurrentSale = currentSale;
            product.Count = count;
            product.Description = description;
            product.ImageUrl = Path.GetFileName(selectedImagePath);

            dbContext.Update(product);
            dbContext.SaveChanges();

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DeleteProduct(object sender, RoutedEventArgs e)
    {
        using var dbContext = new ForTheLifeDbContext();
        var product = dbContext.Products
            .SingleOrDefault(x => x.Id == _productId);
        if (product == null)
        {
            MessageBox.Show("Товар не найден!");
            DialogResult = false;
            return;
        }

        var dialogResult = MessageBox.Show(
            "Вы уверены что хотите удалить выбранный товар?",
            "Удаление товара!",
            MessageBoxButton.YesNo);
        if (dialogResult == MessageBoxResult.No) return;

        dbContext.Remove(product);
        dbContext.SaveChanges();

        DialogResult = true;
    }
}
