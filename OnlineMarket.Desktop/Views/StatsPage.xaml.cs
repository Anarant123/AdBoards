using AdBoards.ApiClient.Contracts.Responses;
using AdBoards.ApiClient.Extensions;
using AdBoardsDesktop.Models.db;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;

namespace AdBoardsDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        public StatsPage()
        {
            InitializeComponent();

            getCountOfPerson();
            getAds();
        }

        private async void getCountOfPerson()
        {
            var count = await Context.Api.GetCountOfClient();
            lblCountOfClient.Text = $"Всего пользователей в системе: {count}";
        }

        private async void getAds()
        {
            var ads = await Context.Api.GetAds();

            lblCountOfAd.Text += " " + ads.Count;
            lblCountOfC1.Text += " " + ads.Where(x => x.Category.Id == 1).Count();
            lblCountOfC2.Text += " " + ads.Where(x => x.Category.Id == 2).Count();
            lblCountOfC3.Text += " " + ads.Where(x => x.Category.Id == 3).Count();
            lblCountOfC4.Text += " " + ads.Where(x => x.Category.Id == 4).Count();
            lblCountOfC5.Text += " " + ads.Where(x => x.Category.Id == 5).Count();
            lblCountOfT1.Text += " " + ads.Where(x => x.AdType.Id == 1).Count();
            lblCountOfT2.Text += " " + ads.Where(x => x.AdType.Id == 2).Count();

        }
    }
}
