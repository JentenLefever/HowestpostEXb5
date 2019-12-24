using HoWestPost.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace HoWestPost.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Pakket> GewonePakketten = new List<Pakket>();
        public List<Pakket> PriorPakketten = new List<Pakket>();
        private Delivery delivery;       

        public MainWindow()
        {
            InitializeComponent();
            MinuutLijstAanvullen();
        }

        public void MinuutLijstAanvullen()
        {
            for (int getal = 30; getal < 91; getal++)
            {
                lstReisTijd.Items.Add(getal);
            }
            lstReisTijd.SelectedIndex = 0;
        }

        public void BerekenenEnWeergeven(PakketType soortPakket)
        {
            double BerekendeTijd = 1;

            switch (soortPakket)
            {
                case PakketType.Mini:
                    BerekendeTijd = Convert.ToDouble(lstReisTijd.SelectedItem);
                    break;
                case PakketType.Standaard:
                    BerekendeTijd = Convert.ToDouble(lstReisTijd.SelectedItem) * 1.2;
                    break;
                case PakketType.Maxi:
                    BerekendeTijd = Convert.ToDouble(lstReisTijd.SelectedItem) * 1.5;
                    break;
                default:
                    break;
            }            
           
            if (checkPrior.IsChecked == true)
            {
                PriorPakketten.Add(new Pakket { Reistijd = BerekendeTijd, PakketType = soortPakket, Prior = true });              
            }
            else
            {
                GewonePakketten.Add(new Pakket { Reistijd = BerekendeTijd, PakketType = soortPakket, Prior = false });              
            }

            WachtrijVullen();
        }

        public void WachtrijVullen()
        {
            tbWachtrij.Text = null;

            foreach (var pakket in PriorPakketten)
            {
                tbWachtrij.Text += $"{pakket.PakketType} pakket, {pakket.Reistijd} min reistijd(prior)\n";
            }

            foreach (var pakket in GewonePakketten)
            {
                tbWachtrij.Text += $"{pakket.PakketType} pakket, {pakket.Reistijd} min reistijd\n";
            }
        }   

        public void StartDeliveryProgress()
        {
            if (PriorPakketten.Count != 0)
            {
                tbTotaleReistijd.Text = PriorPakketten[0].Reistijd.ToString() + " min";
                tbPrior.Text = "ja";
                tbPakketType.Text = PriorPakketten[0].PakketType.ToString();

                delivery = new Delivery();
                delivery.DeliveryProgress += new DeliveryProgressHandler(D_DeliveryProgress);
                delivery.StartDelivery(PriorPakketten[0].Reistijd);
            }
            else if (GewonePakketten.Count != 0)
            {
                tbTotaleReistijd.Text = GewonePakketten[0].Reistijd.ToString() + " min";
                tbPrior.Text = "nee";
                tbPakketType.Text = GewonePakketten[0].PakketType.ToString();

                delivery = new Delivery();
                delivery.DeliveryProgress += new DeliveryProgressHandler(D_DeliveryProgress);
                delivery.StartDelivery(GewonePakketten[0].Reistijd);
            }
        }        

        void D_DeliveryProgress(object sender, DeliveryEventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(() => { D_DeliveryProgress(sender, e); });
            }
            else
            {
                lblResterendeTijd.Content = e.ResterendeTijd;
                if (lblResterendeTijd.Content.ToString() == "00:00:00")
                {
                    DeleteDeliveredPost();
                }                
            }
        }

        public void DeleteDeliveredPost()
        {
            if (tbPrior.Text == "ja")
            {
                PriorPakketten.RemoveAt(0);
            }
            else if (tbPrior.Text == "nee")
            {
                GewonePakketten.RemoveAt(0);
            }
            VoltooidPakketVerwijderen();
            StartDeliveryProgress();
        }

        public void VoltooidPakketVerwijderen()
        {
            tbTotaleReistijd.Text = null;
            lblResterendeTijd.Content = null;
            tbPrior.Text = null;
            tbPakketType.Text = null;

            WachtrijVullen();
        }

        //input
        //reistijd
        private void lstReisTijd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //0-0.9kg
        private void btnMini_Click(object sender, RoutedEventArgs e)
        {
            BerekenenEnWeergeven(PakketType.Mini);
            if (lblResterendeTijd.Content == null)
            {
                StartDeliveryProgress();
            }
        }
        //1-9.9kg
        private void btnStandaard_Click(object sender, RoutedEventArgs e)
        {
            BerekenenEnWeergeven(PakketType.Standaard);
            if (lblResterendeTijd.Content == null)
            {
                StartDeliveryProgress();
            }
        }
        //10-30kg
        private void btnMaxi_Click(object sender, RoutedEventArgs e)
        {
            BerekenenEnWeergeven(PakketType.Maxi);
            if (lblResterendeTijd.Content == null)
            {
                StartDeliveryProgress();
            }
        }
        //prior?
        private void checkPrior_Checked(object sender, RoutedEventArgs e)
        {

        }
        

        //output behandeling
        //wachtrij
        private void tbWachtrij_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //output onderweg
        //totale reidstijd
        private void tbTotaleReistijd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //pakket type
        private void tbPakketType_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //prior?
        private void tbPrior_TextChanged(object sender, TextChangedEventArgs e)
        {

        }       
    }
}
