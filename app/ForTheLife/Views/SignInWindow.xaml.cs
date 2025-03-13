using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ForTheLife.Views;

/// <summary>
/// Логика взаимодействия для SignInWindow.xaml
/// </summary>
public partial class SignInWindow : Window
{
    public SignInWindow()
    {
        InitializeComponent();
    }

    private void SignIn(object sender, RoutedEventArgs e)
    {
        var username = UsernameTB.Text;
        var password = PasswordPB.Password; 

        using var dbContext = new DbContexts.ForTheLifeDbContext();
        var user = dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefault(x => x.Username == username && x.Pass == password);
        if (user == null)
        {
            MessageBox.Show("Неправильный логин или пароль!");
            return;
        }

        ApplicationData.CurrentUser = user;
        DialogResult = true;
    }
}
