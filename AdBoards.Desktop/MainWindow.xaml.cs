using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using System;
using System.Windows;

namespace AdBoardsDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new Uri("Views/AdsPage.xaml", UriKind.Relative));
        }

        private void btnToSearch_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri("Views/AdsPage.xaml", UriKind.Relative));
        }

        private void btnToMyAds_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow != null)
                mainFrame.NavigationService.Navigate(new Uri("Views/MyAdsPage.xaml", UriKind.Relative));
            else
                mainFrame.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }

        private void btnToAddAd_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow != null)
                mainFrame.NavigationService.Navigate(new Uri("Views/AddAdPage.xaml", UriKind.Relative));
            else
                mainFrame.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }

        private void btnToFavoritesAds_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow != null)
                mainFrame.NavigationService.Navigate(new Uri("Views/FavoritesAdsPage.xaml", UriKind.Relative));
            else
                mainFrame.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }

        private async void btnToProfile_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow != null)
            {
                Context.UserNow.Person = await Context.Api.GetMe();
                mainFrame.NavigationService.Navigate(new Uri("Views/ProfilePage.xaml", UriKind.Relative));
            }
            else
                mainFrame.NavigationService.Navigate(new Uri("Views/AuthorizationPage.xaml", UriKind.Relative));
        }
    }
}
