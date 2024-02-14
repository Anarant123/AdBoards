using AdBoardsDesktop.Models.db;
using AdBoards.ApiClient.Contracts.Responses;
using AdBoards.ApiClient.Extensions;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace AdBoardsDesktop.Views
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
