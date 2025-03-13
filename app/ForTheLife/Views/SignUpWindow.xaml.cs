using ForTheLife.DbContexts;
using ForTheLife.Entities;
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
using System.Windows.Shapes;

namespace ForTheLife.Views
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            var firstName = FirstNameTB.Text;
            var lastName = LastNameTB.Text; 
            var middleName = MiddleNameTB.Text;
            var username = UsernameTB.Text;
            var password = PasswordPB.Password;

            using var dbContext = new ForTheLifeDbContext();
            var isExistByUsername = dbContext.Users.Any(x => x.Username == username);
            if (isExistByUsername)
            {
                MessageBox.Show("Пользователь с данным именем уже существует!");
                return;
            }

            var user = new User()
            {
                Lastname = lastName,
                Firstname = firstName,
                Middlename = middleName,
                Username = username,
                Pass = password,
                RoleId = 1
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            DialogResult = true;
        }
    }
}
