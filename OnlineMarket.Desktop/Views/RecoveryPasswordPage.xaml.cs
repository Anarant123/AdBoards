using OnlineMarket.ApiClient.Contracts.Responses;
using OnlineMarket.ApiClient.Extensions;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using OnlineMarket.Desktop.Models.db;

namespace OnlineMarket.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для RecoveryPasswordPage.xaml
    /// </summary>
    public partial class RecoveryPasswordPage : Page
    {
        public RecoveryPasswordPage()
        {
            InitializeComponent();
        }

        private async void btnRecover_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                MessageBox.Show("Введите логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await Context.Api.Recover(tbLogin.Text);

            this.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }
    }
}
