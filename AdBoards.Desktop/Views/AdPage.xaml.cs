using AdBoardsDesktop.Models.db;
using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Extensions;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AdPage.xaml
    /// </summary>
    public partial class AdPage : Page
    {
        bool isF = false;
        public AdPage()
        {
            InitializeComponent();

            tbName.Text = Context.AdNow.Name;
            tbDescription.Text = Context.AdNow.Description;
            tbPhone.Text = Context.AdNow.Person.Phone;
            tbCity.Text = Context.AdNow.City;
            tbPrice.Text = Context.AdNow.Price.ToString();
            imgAd.Source = new BitmapImage(new Uri(Context.AdNow.PhotoName));
            imgSalesman.Source = new BitmapImage(new Uri(Context.AdNow.Person.PhotoName));
            lblSalesman.Text = Context.AdNow.Person.Name;

            isF = Context.AdNow.IsFavorite;
            if (isF)
                btnAddToFavorite.Content = "Удалить из избранного";
        }

        private async void btnAddToFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (isF)
            {
                var result = await Context.Api.DeleteFromFavorites(Context.AdNow.Id);

                if (!result)
                {
                    MessageBox.Show("Что то пошло не так...");
                    return;
                }
                MessageBox.Show("Объявление удалено из избранного");
                btnAddToFavorite.Content = "Добавить в избранное";
                isF = false;
            }
            else
            {
                var result = await Context.Api.AddToFavorites(Context.AdNow.Id);

                if (!result)
                {
                    MessageBox.Show("Что то пошло не так...");
                    return;
                }

                MessageBox.Show("Объявление добавленно в избранное");
                btnAddToFavorite.Content = "Удалить из избранного";
                isF = true;
            }
        }

        private async void btnToComplain_Click(object sender, RoutedEventArgs e)
        {
            var result = await Context.Api.AddToComplaints(Context.AdNow.Id);

            if (!result)
            {
                MessageBox.Show("Вы уже пожаловались");
                return;
            }

            MessageBox.Show("Жалоба успешно отправленна");
        }
    }
}
