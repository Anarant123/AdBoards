using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Contracts.Responses;
using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        EditPersonModel p = new EditPersonModel();
        public ProfilePage()
        {
            InitializeComponent();

            var person = Context.UserNow.Person;

            tbNickName.Text = person.Name;
            tbBirthday.Text = person.Birthday.ToString().Substring(0, 10);
            tbCityName.Text = person.City;
            tbEmail.Text = person.Email;
            tbPhoneNumber.Text = person.Phone;
            imgPerson.Source = new BitmapImage(new Uri(person.PhotoName));
        }

        private void btnSetPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.BMP; *.JPG; *.GIF; *.PNG)| *.BMP; *.JPG; *.GIF; *.PNG | All files(*.*) | *.* ";
            if (o.ShowDialog() == true)
            {
                try
                {
                    imgPerson.Source = LoadImage.Load(File.ReadAllBytes(o.FileName));

                    var stream = new FileStream(o.FileName, FileMode.Open);
                    var formFile = new FormFile(stream, 0, stream.Length, "streamFile", Path.GetFileName(o.FileName));

                    p.Photo = formFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DateTime birthday;

            string ValidateFields()
            {
                var result = string.Empty;

                if (!DateTime.TryParseExact(tbBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday))
                    result += "Введите корректную дату рождения в формате дд.мм.гггг.\n";

                if (string.IsNullOrWhiteSpace(tbEmail.Text) || !IsValidEmail(tbEmail.Text))
                    result += "Введите корректный email.\n";

                if (string.IsNullOrWhiteSpace(tbPhoneNumber.Text) || !IsValidPhone(tbPhoneNumber.Text))
                    result += "Введите корректный номер телефона.\n";

                if (string.IsNullOrWhiteSpace(tbNickName.Text))
                    result += "Имя не корректно.\n";

                if (string.IsNullOrWhiteSpace(tbCityName.Text))
                    result += "Имя не корректно.\n";

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


            p.Name = tbNickName.Text;
            p.Birthday = birthday;
            p.City = tbCityName.Text;
            p.Email = tbEmail.Text;
            p.Phone = tbPhoneNumber.Text;

            var result = await Context.Api.PersonUpdate(p);
            if (!result.IsOk)
            {
                var error = string.Join(Environment.NewLine, result.Error.Select(x => x.Message));
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Person person = result.Ok;

            if (p.Photo != null)
                person = await Context.Api.UpdatePersonPhoto(p);

            if (person == null) 
            {
                MessageBox.Show("Что то пошло не так");
                return;
            }

            Context.UserNow.Person = person;
            MessageBox.Show("Вы успешно изменили данные профиля!");
        }

        private void btnResetInfo_Click(object sender, RoutedEventArgs e)
        {
            var person = Context.UserNow.Person;

            tbNickName.Text = person.Name;
            tbBirthday.Text = person.Birthday.ToString().Substring(0, 10);
            tbCityName.Text = person.City;
            tbEmail.Text = person.Email;
            tbPhoneNumber.Text = person.Phone;
            imgPerson.Source = new BitmapImage(new Uri(person.PhotoName));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Context.UserNow = null;
            this.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }
    }
}
