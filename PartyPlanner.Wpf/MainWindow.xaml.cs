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
using PartyPlanning.Lib.Entities;

namespace PartyPlanner.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const int CnId = 0;
        const int CnMedewerker = 1;
        const int CnGeboortedatum = 2;
        DataRowView huidigeMedewerker;

        public MainWindow()
        {
            InitializeComponent();
            DataView gesorteerdeTabel = new DataView
            {
                Table = MedewerkerBeheer.GeefAlleRecords(),
                Sort = "Medewerker ASC"
            };
            MedewerkerBeheer.dvMedeWerkers = gesorteerdeTabel;
        }

        void KoppelLijsten()
        {
            dgMedewerkers.ItemsSource = MedewerkerBeheer.dvMedeWerkers;
        }

        bool SlaMedewerkerOp()
        {
            bool opgeslagen = true;
            try
            {
                int id = huidigeMedewerker == null ?
                    MedewerkerBeheer.GeefNieuwId() :
                    (int)huidigeMedewerker[CnId];

                string naam = txtNaam.Text;
                DateTime? geboorteDatum = dtpGeboortedatum.SelectedDate;
                Medewerker medewerker = new Medewerker(id, naam, (DateTime)geboorteDatum);
                MedewerkerBeheer.SlaOp(medewerker);
                ToonMelding($"{medewerker.Naam} is opgeslagen", true);
                WisMedewerkerInput();
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
                opgeslagen = false;
            }
            return opgeslagen;
        }

        void WisMedewerkerInput()
        {
            txtNaam.Clear();
            dtpGeboortedatum.SelectedDate = null;
            dgMedewerkers.SelectedIndex = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            KoppelLijsten();
        }

        private void dgMedewerkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huidigeMedewerker = (DataRowView)dgMedewerkers.SelectedItem;
            if (huidigeMedewerker == null)
            {
                WisMedewerkerInput();
            }
            else
            {
                txtNaam.Text = huidigeMedewerker[CnMedewerker].ToString();
                dtpGeboortedatum.SelectedDate = (DateTime)huidigeMedewerker[CnGeboortedatum];
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            WisMedewerkerInput();
            txtNaam.Focus();
        }

        private void btnSlaOp_Click(object sender, RoutedEventArgs e)
        {
            if (SlaMedewerkerOp()) WisMedewerkerInput();
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            string naamMedewerker = "";
            try
            {
                int index = dgMedewerkers.SelectedIndex;
                naamMedewerker = huidigeMedewerker[CnMedewerker].ToString();
                MedewerkerBeheer.VerwijderRecord((int)huidigeMedewerker[CnId]);
                MedewerkerBeheer.dvMedeWerkers.Delete(index);
                ToonMelding($"{naamMedewerker} is verwijderd", true);
                WisMedewerkerInput();
            }
            catch (Exception ex)
            {

                ToonMelding($"{naamMedewerker} is niet verwijderd\n{ex.Message}");
            }

        }
    }
}
