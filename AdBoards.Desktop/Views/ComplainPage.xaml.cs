using AdBoardsDesktop.Models.db;
using AdBoards.ApiClient.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using AdBoards.ApiClient.Extensions;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ComplainPage.xaml
    /// </summary>
    public partial class ComplainPage : Page
    {
        public ComplainPage()
        {
            InitializeComponent();

            getAds();
        }

        private void lvAds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.AdNow = new Ad();
            Context.AdNow = (lvAds.SelectedItem as Ad);

            this.NavigationService.Navigate(new Uri("Views/AdminAdPage.xaml", UriKind.Relative));
        }

        private async void getAds()
        {
            Context.AdList = new AdListViewModel();
            Context.AdList.Ads = await Context.Api.GetAds();
            lvAds.ItemsSource = Context.AdList.Ads.ToList().Where(x => x.Complaints.Count() > 0);
        }
    }
}
