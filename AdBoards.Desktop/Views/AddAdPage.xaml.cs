using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AddAdPage.xaml
    /// </summary>
    public partial class AddAdPage : Page
    {
        AddAdModel ad = new AddAdModel();
        public AddAdPage()
        {
            InitializeComponent();

            tbAddingCity.Text = Context.UserNow.Person.City;
        }

        private async void btnAddPost_Click(object sender, RoutedEventArgs e)
        {
            int price;
            string ValidateFields()
            {
                var result = string.Empty;
                if (!int.TryParse(tbAddingPrice.Text, out price) || price < 0)
                    result += "Некорректная цена\n";


                if (string.IsNullOrWhiteSpace(tbAddingName.Text))
                    result += "Название объявления является обязательным полем.\n";

                if (string.IsNullOrWhiteSpace(tbAddingCity.Text))
                    result += "Город является обязательным полем.\n";

                return result;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ad.Name = tbAddingName.Text;
            ad.City = tbAddingCity.Text;
            ad.CategoryId = cbAddingСategories.SelectedIndex + 1;
            ad.Description = tbAddingDescription.Text;
            ad.Price = price;
            if (rbBuy.IsChecked == true)
                ad.AdTypeId = 1;
            else
                ad.AdTypeId = 2;

            Context.AdNow = await Context.Api.AddAd(ad);
            ad.Id = Context.AdNow.Id;
            if (ad.Photo != null)
                Context.AdNow = await Context.Api.UpdateAdPhoto(ad);

            if (Context.AdNow == null)
            {
                MessageBox.Show("Что то пошло не так! \nОбъявление добавить не удалось...");
                return;
            }

            MessageBox.Show("Объявление успешно добавленно");
            this.NavigationService.Navigate(new Uri("Views/MyAdsPage.xaml", UriKind.Relative));

        }

        private void btnToBackFrame_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/AdsPage.xaml", UriKind.Relative));
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.BMP; *.JPG; *.GIF; *.PNG)| *.BMP; *.JPG; *.GIF; *.PNG | All files(*.*) | *.* ";
            if (o.ShowDialog() == true)
            {
                try
                {
                    imgAd.Source = LoadImage.Load(File.ReadAllBytes(o.FileName));

                    var stream = new FileStream(o.FileName, FileMode.Open);
                    var formFile = new FormFile(stream, 0, stream.Length, "streamFile", Path.GetFileName(o.FileName));

                    ad.Photo = formFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tbAddingPrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
