using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            DateTime birthday = DateTime.Now;

            string ValidateFields()
            {
                string result = string.Empty;
                if (string.IsNullOrWhiteSpace(tbLogin.Text))
                    result += "Введите логин.\n";

                if (!DateTime.TryParseExact(tbBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday))
                    result += "Введите корректную дату рождения в формате дд.мм.гггг.\n";

                if (string.IsNullOrWhiteSpace(tbEmail.Text) || !IsValidEmail(tbEmail.Text))
                    result += "Введите корректный email.\n";
               
                if (string.IsNullOrWhiteSpace(tbPhone.Text) || !IsValidPhone(tbPhone.Text))
                    result += "Введите корректный номер телефона.\n";

                if (string.IsNullOrWhiteSpace(tbPassword1.Password) || string.IsNullOrWhiteSpace(tbPassword2.Password) || tbPassword1.Password.Length < 8)
                    result += "Введите пароль в оба поля.";
                else if (tbPassword1.Password != tbPassword2.Password)
                    result += "Пароли не совпадают.\n";

                return result;
            }

            bool IsValidEmail(string email)
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                Match match = Regex.Match(email, pattern);
                return match.Success;
            }

            bool IsValidPhone(string phone)
            {
                string pattern = @"^(\+)[1-9][0-9\-().]{9,15}$";
                Match match = Regex.Match(phone, pattern);
                return match.Success;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PersonReg person = new PersonReg();
            person.Login = tbLogin.Text;
            person.Phone = tbPhone.Text;
            person.Email = tbEmail.Text;
            person.Password = tbPassword1.Password;
            person.ConfirmPassword = tbPassword2.Password;
            person.Birthday = birthday;

            var result = (await Context.Api.Registr(person)).ToList();

            if (result.Count == 0)
            {
                this.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
                return;
            }

            var error = string.Join(Environment.NewLine, result.Select(x => x.Message));

            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
