using AdBoardsDesktop.Models.db;
using AdBoards.ApiClient.Contracts.Responses;
using AdBoards.ApiClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AdsPage.xaml
    /// </summary>
    public partial class AdsPage : Page
    {
        public AdsPage()
        {
            InitializeComponent();

            getAds();
        }

        private async void btnUseFilter_Click(object sender, RoutedEventArgs e)
        {
            lvAds.ItemsSource = await Context.Api.UseFulter(1, tbPriceFrom.Text, tbPriceUpTo.Text, tbCity.Text, Convert.ToInt32(cbСategories.SelectedIndex), (bool)rbBuy.IsChecked!, (bool)rbSell.IsChecked!);

            if (lvAds.Items.Count == 0)
                MessageBox.Show("С данными фильтрами ничего не найдено");
        }

        private async void lvAds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.AdNow = new Ad();
            Context.AdNow = await Context.Api.GetAd((lvAds.SelectedItem as Ad).Id);

            this.NavigationService.Navigate(new Uri("Views/AdPage.xaml", UriKind.Relative));
        }

        private void tbPriceFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; 
            }
        }

        private async void getAds()
        {
            Context.AdList = new AdListViewModel();
            Context.AdList.Ads = await Context.Api.GetAds();
            lvAds.ItemsSource = Context.AdList.Ads.ToList();
        }

        private void btnDropFilter_Click(object sender, RoutedEventArgs e)
        {
            tbCity.Text = "";
            tbPriceFrom.Text = "";
            tbPriceUpTo.Text = "";
            cbСategories.SelectedIndex = 0;
            getAds();
        }
    }
}
