using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Contracts.Responses;
using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для MyAdPage.xaml
    /// </summary>
    public partial class MyAdPage : Page
    {
        AddAdModel ad = new AddAdModel();
        public MyAdPage()
        {
            InitializeComponent();

            tbName.Text = Context.AdNow.Name;
            tbDescription.Text = Context.AdNow.Description;
            cbСategories.SelectedIndex = Context.AdNow.AdType.Id;
            tbCity.Text = Context.AdNow.City;
            tbPrice.Text = Context.AdNow.Price.ToString();
            imgAd.Source = new BitmapImage(new Uri(Context.AdNow.PhotoName));
            if (Context.AdNow.AdType.Id == 1)
                rbBuy.IsChecked = true;
            else
                rbSell.IsChecked = true;
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            int price;
            string ValidateFields()
            {
                var result = string.Empty;
                if (!int.TryParse(tbPrice.Text, out price) || price < 0)
                    result += "Некорректная цена\n";

                if (string.IsNullOrWhiteSpace(tbName.Text))
                    result += "Название объявления является обязательным полем.\n";

                if (string.IsNullOrWhiteSpace(tbCity.Text))
                    result += "Город является обязательным полем.\n";

                return result;
            }


            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ad.Id = Context.AdNow.Id;
            ad.Name = tbName.Text;
            ad.Description = tbDescription.Text;
            ad.CategoryId = cbСategories.SelectedIndex + 1;
            if (rbBuy.IsChecked == true)
                ad.AdTypeId = 1;
            else if (rbSell.IsChecked == true)
                ad.AdTypeId = 2;
            ad.Price = price;
            ad.City = tbCity.Text;

            Context.AdNow = await Context.Api.AdUpdate(ad);
            ad.Id = Context.AdNow.Id;
            if (ad.Photo != null)
                Context.AdNow = await Context.Api.UpdateAdPhoto(ad);

            if (Context.AdNow == null)
            {
                MessageBox.Show("Что то пошло не так\n");
                return;
            }

            MessageBox.Show("Вы успешно изменили объявление!");
        }

        private async void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            var result = await Context.Api.DeleteAd(Context.AdNow.Id);

            if (result)
            {
                this.NavigationService.Navigate(new Uri("Views/MyAdsPage.xaml", UriKind.Relative));
                return;
            }
            MessageBox.Show("Что то пошло не так...");
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

        private void tbPrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
