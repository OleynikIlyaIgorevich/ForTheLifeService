using ForTheLife.DbContexts;
using ForTheLife.Entities;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ForTheLife.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateProductWindow.xaml
    /// </summary>
    public partial class CreateProductWindow : Window
    {
        private string baseImagesPath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images");
        private string defaultImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images\\picture.png");
        private string? selectedImagePath;
        public CreateProductWindow()
        {
            InitializeComponent();

            SelectedImage.Source = new BitmapImage(new Uri(defaultImagePath, UriKind.RelativeOrAbsolute));

            LoadCategoriesData();
            LoadUnitsData();
            LoadSuppliersData();
            LoadProducersData();
            LoadProductsNamesData();
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

        private void CreateProduct(object sender, RoutedEventArgs e)
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

                if (selectedImagePath != null)
                {
                    var isImagesDirectoryExists = Directory.Exists(baseImagesPath);
                    if (!isImagesDirectoryExists) Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Resources\\Images"));

                    var imageName = Path.GetFileName(selectedImagePath);
                    var imagesPath = Path.Combine(Environment.CurrentDirectory, "Resources\\Images");
                    var saveFilePath = Path.Combine(imagesPath, imageName);
                    File.Copy(selectedImagePath, saveFilePath, true);
                }


                var product = new Product()
                {
                    CategoryId = selectedCategory.Id,
                    ProducerId = selectedProducer.Id,
                    SupplierId = selectedSupplier.Id,
                    ProductNameId = selectedProductName.Id,
                    UnitId = selectedUnit.Id,
                    Article = article,
                    Price = price,
                    MaxSale = maxSale,
                    CurrentSale = currentSale,
                    Count = count,
                    Description = description,
                    ImageUrl = selectedImageName,
                };

                using var dbContext = new ForTheLifeDbContext();
                dbContext.Products.Add(product);
                dbContext.SaveChanges();

                DialogResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




    }
}
