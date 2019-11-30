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
        ShiftBeheer shiftBeheer = new ShiftBeheer();
        Shift huidigeShift;

        public MainWindow()
        {
            InitializeComponent();

        }

        void KoppelLijstenMedewerkers()
        {
            MedewerkerBeheer.LaadDvMedeWerkers();
            dgMedewerkers.ItemsSource = MedewerkerBeheer.DvMedeWerkers;
            cmbMedewerker.ItemsSource = MedewerkerBeheer.Medewerkers;
        }

        void KoppelLijstenTaken()
        {
            TaakBeheer.LaadDvTaken();
            lstTaken.ItemsSource = TaakBeheer.Taken;
            lstTaken.Items.Refresh();
            cmbTaak.ItemsSource = TaakBeheer.Taken;
        }

        void KoppelUren()
        {
            lstUur.ItemsSource = Shift.InTeVullenUren;
        }

        void KoppelShifts()
        {
            lstShifts.ItemsSource = shiftBeheer.ShiftLijst;
            lstShifts.Items.Refresh();
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

        bool SlaShiftOp()
        {
            bool opgeslagen = true;
            try
            {
                int id = huidigeShift == null ?
                    MedewerkerBeheer.GeefNieuwId() :
                    huidigeShift.Id;

                int medewerkerId = ((Medewerker)cmbMedewerker.SelectedItem).Id;
                int taakId = ((Taak)cmbTaak.SelectedItem).Id;
                int uur = (int)lstUur.SelectedItem;
                string opmerkingen = txtOpmerking.Text;

                Shift shift = new Shift
                {
                    Id = id,
                    Verantwoordelijke = (Medewerker)cmbMedewerker.SelectedItem,
                    Opmerkingen = opmerkingen,
                    ToegewezenTaak = (Taak)cmbTaak.SelectedItem,
                    Uur = (int)lstUur.SelectedItem

                };
                ShiftBeheer.SlaOp(shift);
                ToonMelding($"{shift} is opgeslagen", true);
                WisMedewerkerInput();
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
                opgeslagen = false;
            }
            return opgeslagen;
        }

        void ToonVerantwoordelijke(Medewerker verantwoordelijke)
        {
            foreach (Medewerker medewerker in cmbMedewerker.Items)
            {
                if (verantwoordelijke.Id == medewerker.Id)
                {
                    cmbMedewerker.SelectedItem = medewerker;
                    break;
                }
            }
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
            KoppelLijstenMedewerkers();
            KoppelLijstenTaken();
            KoppelUren();
            KoppelShifts();
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
                KoppelLijstenMedewerkers();
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
                MedewerkerBeheer.DvMedeWerkers.Delete(index);
                ToonMelding($"{naamMedewerker} is verwijderd", true);
                WisMedewerkerInput();
                KoppelLijstenMedewerkers();
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
                TaakBeheer.VoegRecordToe(taaknaam);
                KoppelLijstenTaken();
                txtTaak.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception($"Taak niet opgeslagen\n{ex.Message}");
            }

        }

        private void lstShifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstShifts.SelectedItem != null)
            {
                huidigeShift = (Shift)lstShifts.SelectedItem;
                ToonVerantwoordelijke(huidigeShift.Verantwoordelijke);
                cmbTaak.SelectedItem = huidigeShift.ToegewezenTaak;
                lstUur.SelectedItem = huidigeShift.Uur;
                txtOpmerking.Text = huidigeShift.Opmerkingen;
                tbkFeedBack.Visibility = Visibility.Visible;
            }
            else
            {
                WisShiftInput();
                huidigeShift = null;
            }
        }

        private void btnShiftToevoegen_Click(object sender, RoutedEventArgs e)
        {
            lstShifts.SelectedItem = null;
        }

        private void WisShiftInput()
        {
            cmbMedewerker.SelectedItem = null;
            cmbTaak.SelectedItem = null;
            lstUur.SelectedItem = null;
            txtOpmerking.Clear();
        }

        private void btnVerwijderShift_Click(object sender, RoutedEventArgs e)
        {
            string naamShift = huidigeShift.ToString();
            try
            {
                ShiftBeheer.VerwijderRecord(huidigeShift.Id);
                ToonMelding($"{naamShift} is verwijderd", true);
                KoppelShifts();
            }
            catch (Exception ex)
            {
                ToonMelding($"{naamShift} is niet verwijderd\n{ex.Message}");
            }
        }

        private void btnSlaShiftOp_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTaak.SelectedItem == null || 
                cmbMedewerker.SelectedItem == null ||
                lstUur.SelectedItem == null)
            {
                ToonMelding("Selecteer een taak, een medewerker en een uur.");
            }
            else if (SlaShiftOp())
            {
                WisShiftInput();
                KoppelShifts();
            }

        }
    }
}
