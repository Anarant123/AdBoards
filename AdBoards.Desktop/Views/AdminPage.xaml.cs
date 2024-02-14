using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            getPeople();
        }

        private async void getPeople()
        {
            dgPeople.ItemsSource = await Context.Api.GetPeople();
        }

        private async void btnDropClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                MessageBox.Show("Введите корректный логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = await Context.Api.DeletePeople(tbLogin.Text);

            if (result)
            {
                MessageBox.Show("Пользователь успешно удален");
                getPeople();
                return;
            }
            MessageBox.Show("Что то пошло не так...");
        }
    }
}
