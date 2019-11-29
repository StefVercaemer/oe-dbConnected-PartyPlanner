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
        TaakBeheer taakBeheer = new TaakBeheer();

        public MainWindow()
        {
            InitializeComponent();
        }

        void KoppelLijstMedewerkers()
        {
            MedewerkerBeheer.LaadDvMedeWerkers();
            dgMedewerkers.ItemsSource = MedewerkerBeheer.dvMedeWerkers;
        }

        void KoppelLijstenTaken()
        {
            taakBeheer.LaadDvTaken();
            lstTaken.ItemsSource = taakBeheer.Taken;
            lstTaken.Items.Refresh();
            cmbTaak.ItemsSource = taakBeheer.Taken;
            
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
            btnVerwijder.IsEnabled = false;
            KoppelLijstMedewerkers();
            KoppelLijstenTaken();
            tbkFeedBack.Visibility = Visibility.Hidden;
        }

        private void dgMedewerkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huidigeMedewerker = (DataRowView)dgMedewerkers.SelectedItem;
            if (huidigeMedewerker == null)
            {
                WisMedewerkerInput();
                btnVerwijder.IsEnabled = false;
            }
            else
            {
                txtNaam.Text = huidigeMedewerker[CnMedewerker].ToString();
                dtpGeboortedatum.SelectedDate = (DateTime)huidigeMedewerker[CnGeboortedatum];
                btnVerwijder.IsEnabled = true;
                tbkFeedBack.Visibility = Visibility.Hidden;
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            WisMedewerkerInput();
            txtNaam.Focus();
        }

        private void btnSlaOp_Click(object sender, RoutedEventArgs e)
        {
            if (SlaMedewerkerOp())
            {
                WisMedewerkerInput();
                KoppelLijstMedewerkers();
            }
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
                KoppelLijstMedewerkers();
            }
            catch (Exception ex)
            {

                ToonMelding($"{naamMedewerker} is niet verwijderd\n{ex.Message}");
            }

        }

        private void btnTaakToevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string taaknaam = txtTaak.Text;
                taakBeheer.VoegRecordToe(taaknaam);
                KoppelLijstenTaken();
                txtTaak.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception($"Taak niet opgeslagen\n{ex.Message}");
            }

        }
    }
}
