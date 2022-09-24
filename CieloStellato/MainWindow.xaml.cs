using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Interop;


namespace CieloStellato
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Costruttore classe MainWindow.
        public MainWindow()
        {
            InitializeComponent();
        }
        // Campi di classe.
        List<Navicella> flotta = new List<Navicella>(); // Lista di tipo Flotta.
        Random randx = new Random(); // Variabile per generare un numero random.
        bool inseguimento=true; // VAriabile booleana per la modalità inseguimento.



        private void cnbElemento(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender; // VAriabile per il controllo dell' elemento scelto.
            // Se la scelta selezionata è diversa dalla sezione vuota entra.
            if (cmb.SelectedItem != null)
            {
                cnvCielo.Children.Remove(((Navicella)cmb.SelectedItem).N); // Eliminazione dalla stella dal canvas.
                flotta.Remove((Navicella)cmb.SelectedItem); // RImozione dalla lista dell'oggetto.
                cmb.Items.Refresh(); // Viene aggiornata la combobox.
            }
        }

        private void cnbAggiungi(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender; // VAriabile per il controllo dell' elemento scelto.
            // Se la scelta selezionata è diversa dalla sezione vuota entra.
            if (cmb.SelectedItem != null)
            {
                // Ciclo per la creazione di tante stelle quante sono state selezionate nella combobox.
                for (int i = 0; i < cmb.SelectedIndex+1; i++)
                {
                    int x = randx.Next(1, 700); // Generazione della coordinata x random.
                    int y = randx.Next(1, 450); // Generazione della coordinata y random.
                    flotta.Add(new Navicella(x , y , cnvCielo)); // Creazione di un elemento dentro alla lista.
                    
                }
                cmbElemento.Items.Refresh(); // Viene aggiornata la combobox.
            }
            // COn questo for si fa partire il moto di tutte le stelle.
            for(int i=0;i<flotta.Count;i++)
                flotta[i].Movimento(inseguimento); // Richiamato il metodo di movimento per ogni singola stella e gli viene passo un booleano che decide se le stelle si devono inseguire.
        }

        // Metodo che parte appena viene aperta la finestra.
        private void carica(object sender, RoutedEventArgs e)
        {
            cmbElemento.ItemsSource = flotta;
            // Aggiornameto della combobox con le stelle che possono essere messe (da 1 a 5)
            for (int i = 1; i < 4; i++)
            {
                cmbAggiungi.Items.Add(i); // Aggiunta nella combobox.
                cmbAggiungi.Items.Refresh(); // Aggiornamento della combobox.
            }
        }

        // Metodo che viene applicato quando viene celta la combo box.
        private void aperture(object sender, MouseButtonEventArgs e)
        {
            cmbElemento.ItemsSource = flotta; // Passaggio dei dati.
            cmbElemento.Items.Refresh(); // Aggiornamento della combo box.
        }

        // 2 Metodi per la checkbox.
        private void Attivo(object sender, RoutedEventArgs e)
        {
            inseguimento = false; // Se è attiva l'inseguimento va a false.
        }

        private void Spento(object sender, RoutedEventArgs e)
        {
            inseguimento = true; // Se è disattivato l'inseguimento va a true.
        }

        private void btnSvuota_Click(object sender, RoutedEventArgs e)
        {

            cnvCielo.Children.Clear(); // Eliminazione di tutte le stelle dal canvas.
            flotta.Clear(); // Pulizia della lista.
            cmbElemento.Items.Refresh(); // Viene aggiornata la combobox.
            BitmapImage immagine = new BitmapImage(new Uri("../../sfondo.jpg", UriKind.Relative)); // Percorso per lo sfondo.
            Image sf = new Image(); // Creazione della proprietaà immagine.
            sf.Source = immagine; // Passaggio del percorcorso della stella.
            Canvas.SetLeft(sf, 0); // Posizionamento dell' immagine nel canvas per la coordinata x.
            Canvas.SetTop(sf, 0); // Posizionamento dell' immagine nel canvas per la coordinata y.
            cnvCielo.Children.Add(sf); // Inserimento dello sfondo nel canvas.


        }

        // Easter Egg.
        private void chiusura(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool i = true; // Variabile di controllo.
            // FIno a che non si risponde sì il programma continua a chiedere.
            do
            {
                if (MessageBox.Show("Prof mi da 10 ?", "Proooooooooooooooof!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; // Ferma la chiusura. 
                }
                else
                {
                    if (MessageBox.Show("Prof è sicuro!?", "Proooooooooooooooof!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        e.Cancel = true; // Ferma la chiusura. 
                    }
                    else
                    {
                        i = false; // Lascia che si chiuda il programma.
                        e.Cancel = false;
                    }
                }
            } while (i);
        }
    }

    public class Navicella
    {
        
        string _id; // Nome della stella in oggetto.
        int _xx, _yy; // Variabili di movimento della stella.
        double _movx = 0, _movy = 0; // 

        public Image N { get; set; } // Creazione di una proprietà pubblica.
        Random _rand = new Random(); // Variabile per geneare numiri random.
        int _dir; // Variabile per numero random.
        bool _avanti, _su; // Variabili booleane per controllare il movimento della stella a seconda della posizione nello schermo.
        DispatcherTimer _dt = new DispatcherTimer(); // Variabile per il tempo, una specie di multithreding.

        // Costruttore della classe.
        public Navicella(int xc, int yc, Canvas x)
        {
            _dir = _rand.Next(1, 5); // Generazione diun numero random.
            _xx = xc; // Copiatura delle coordinate.
            _yy = yc; // Copiatura delle coordinate.
            BitmapImage immagine = new BitmapImage(new Uri("../../stella.png", UriKind.Relative)); // Creazione di una variabile bitmat per il percorso dell'immagine della stella.
            N = new Image(); // Creazione della proprietaà immagine.
            N.Source = immagine; // Passaggio del percorcorso della stella.
            N.Width = 50; // Larghezza dell'immagine.
            N.Height = 50; // Altezza dell'immagine.
            Canvas.SetLeft(N, xc); // Posizionamento dell' immagine nel canvas per la coordinata x.
            Canvas.SetTop(N, yc); // Posizionamento dell' immagine nel canvas per la coordinata y.
            _id = "Navicella: " + x.Children.Add(N); // Creazione dell'id dell'immagine.
            Direzione(out _avanti, out _su); // Scelta della direzione in base alle coordinate.

        }

        // Metodo dell'inseguimento.
        public void Movimento(bool ins)
        {
            // Condizione vera la modalita inseguimentonon è attiva, se è falsa vuol dire che è attiva.
            if(ins)
                _dt.Tick -= timer_Tick; // Chiusura del threading precedente.
            _dt.Interval = TimeSpan.FromSeconds(0.01); // Settaggio dell'intervallo tra una operazione a un altra.
            _dt.Tick += timer_Tick; // Creazione del treading.
            _dt.Start(); // Partenza del processo continuo.
        }

        // Scelta della direzione iniziale.
        public void Direzione(out bool avanti, out bool su)
        {
            avanti = true; // Variabile di lavoro.
            su = true; // Variabile di lavoro.

            // Condizioni per le quali le stelle all'inizio non vanno tutte nella stessa direzione.
            if (_dir % 4 == 0)
            {
                avanti = true;
                su = true;
            }
            else if (_dir % 3 == 0)
            {
                avanti = false;
                su = true;
            }
            else if (_dir % 2 == 0)
            {
                avanti = true;
                su = false;
            }
            else if (_dir % 1 == 0)
            {
                avanti = false;
                su = false;
            }
        }

        // Movimento e controllo che non escano dai bordi.
        public void movimento()
        {
            Canvas.SetLeft(N, _xx); // Posizionamento nel canvas dell'immagine nelle coordinate x desiderate.
            Canvas.SetTop(N, _yy); // Posizionamento nel canvas dell'immagine nelle coordinate y desiderate.

            // Controllo che le stelle non escano dalla finestra e che rimbalzino correttamente 
            if (_xx > 830)
            {
                _avanti = false;
            }
            if (_xx < 40)
            {
                _avanti = true;
            }
            // Spostamento avanti.
            if (_avanti)
            {
                _xx += 1;
            }
            // Spostamento indietro.
            else
            {
                _xx -= 1;
            }
            if (_yy > 465)
            {
                _su = false;
            }
            if (_yy < 40)
            {
                _su = true;
            }
            // Spostamento giù.
            if (_su)
            {
                _yy += 1;
            }
            // Spostamento su.
            else
            {
                _yy -= 1;
            }
        }

        // Variabile campo di classe per la rotazione della stella.
        private int _rotazione = 0;

        // Metodo per la rotazione della stella.
        public void ROtazioneStella()
        {
            N.RenderTransform = new RotateTransform(_rotazione += 3, _movx, _movy);
        }

        // Metodo che eseguo ripetutamente nel dispaccertimer.
        void timer_Tick(object sender, EventArgs e)
        {
            movimento(); // Invoca il metodo del movimento.
            ROtazioneStella(); // Invoca il metodo per la rotazione della stella.
        }

        // Ovverride di ToString.
        public override string ToString()
        {
            return _id; // Passagio del nome della stella.  
        }

    }
}
