using AdBoardsDesktop.Models.db;
using System;
using System.Net.Http;
using AdBoards.ApiClient.Contracts.Requests;
using AdBoards.ApiClient.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminAdPage.xaml
    /// </summary>
    public partial class AdminAdPage : Page
    {
        public AdminAdPage()
        {
            InitializeComponent();

            tbName.Text = Context.AdNow.Name;
            tbDescription.Text = Context.AdNow.Description;
            tbPhone.Text = Context.AdNow.Person.Phone;
            tbCity.Text = Context.AdNow.City;
            tbPrice.Text = Context.AdNow.Price.ToString();
            imgAd.Source = new BitmapImage( new Uri(Context.AdNow.PhotoName));
            imgSalesman.Source = new BitmapImage(new Uri(Context.AdNow.Person.PhotoName));
            lblSalesman.Text = Context.AdNow.Person.Name;
        }

        private async void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            var result = await Context.Api.DeleteAd(Context.AdNow.Id);

            if (result)
            {
                this.NavigationService.Navigate(new Uri("Views/ComplainPage.xaml", UriKind.Relative));
                return;
            }
            MessageBox.Show("Что то пошло не так...");
        }

        private async void btnWithdrawTheComplaint_Click(object sender, RoutedEventArgs e)
        {
            var result = await Context.Api.DeleteFromComplaints(Context.AdNow.Id);

            if (result)
            {
                MessageBox.Show("Жалоба снята успешно!");
                this.NavigationService.Navigate(new Uri("Views/ComplainPage.xaml", UriKind.Relative));
                return;
            }
            MessageBox.Show("Что то пошло не так...");
        }
    }
}
