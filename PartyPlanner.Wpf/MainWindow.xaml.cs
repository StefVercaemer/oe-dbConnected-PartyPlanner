using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PartyPlanning.Lib;

namespace PartyPlanner.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataView dvMedeWerkers;
        const int CnId = 0;
        const int CnMedewerker = 1;
        const int CnGeboortedatum = 2;

        public MainWindow()
        {
            InitializeComponent();
            DataView gesorteerdeTabel = new DataView
            {
                Table = MedewerkerBeheer.GeefAlleRecords(),
                Sort = "Medewerker ASC"
            };
            dvMedeWerkers = gesorteerdeTabel;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgMedewerkers.ItemsSource = dvMedeWerkers;

        }

        private void dgMedewerkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView medewerker = (DataRowView)dgMedewerkers.SelectedItem;
            txtNaam.Text = medewerker[CnMedewerker].ToString();
            dtpGeboortedatum.SelectedDate = (DateTime)medewerker[CnGeboortedatum];
        }
    }
}
