using System;
using System.Windows;

namespace AdBoardsDesktop
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            
            mainFrame.NavigationService.Navigate(new Uri("Views/ComplainPage.xaml", UriKind.Relative));
        }

        private void btnToComplaint_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri("Views/ComplainPage.xaml", UriKind.Relative));
        }

        private void btnToStats_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri("Views/StatsPage.xaml", UriKind.Relative));
        }

        private void btnToClient_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri("Views/AdminPage.xaml", UriKind.Relative));
        }
    }
}
