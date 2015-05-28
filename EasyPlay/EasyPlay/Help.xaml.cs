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
using System.Windows.Shapes;

namespace EasyPlay
{
    /// <summary>
    /// Interaktionslogik für Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            lbHelp.Items.Add("Titel Wiedergabe");
            lbHelp.Items.Add("Titel wiederholen");
            lbHelp.Items.Add("Playlists erstellen");
            lbHelp.Items.Add("Playlist wiedergeben");
            lbHelp.Items.Add("Playlist wiederholen");
            lbHelp.Items.Add("Titel zur Playlist hinzufügen");
            lbHelp.Items.Add("Album Wiedergabe");
            lbHelp.Items.Add("Interpret Wiedergabe");
            lbHelp.Items.Add("Warteliste");
            lbTitle.Text = "EasyPlay Hilfe";
            txbHelp.Text = "Hier finden Sie alles was Sie über den MP3-Player EasyPlay wissen müssen";
        }

        private void lbHelp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lbHelp.SelectedItem;
            switch (item.ToString())
            {
                case "Titel Wiedergabe":
                    lbTitle.Text = "Die Wiedergabe von Titeln";
                    txbHelp.Text = "Um einen einzelnen Titel wieder zugeben, muss in der Toolbar auf die Schaltfläche 'Titel' geklickt werden und dann den Titel, welcher";
                    txbHelp.Text += " wiedergegeben werden soll, doppelklicken.";
                    break;
                case "Titel wiederholen":
                    lbTitle.Text = "Die Wiedergabe von Titeln wiederholen";
                    txbHelp.Text = "Um den aktuell spielenden Titel zu wiederholen, muss man auf die entsprechende Schaltfläche neben der aktuellen Wiedergabe klicken.";
                    txbHelp.Text += " Der aktuelle Titel wird wiedergeben, wenn die Schalfläche einen orangen Rand hat.";
                    break;
                case "Playlists erstellen":
                    lbTitle.Text = "Playlists erstellen";
                    txbHelp.Text = "Um eine Playlist zu erstellen, muss in der Toolbar auf die Schaltfläche 'Playlists' gedrückt werden. Nun erscheint ganz oben neben der";
                    txbHelp.Text += " Toolbar eine Schaltfläche mit der Aufschrift 'Playlist erstellen' angzeigt. Mit einem Klick darauf, öffnet sich ein Popup-Fenster, in";
                    txbHelp.Text += " welchem der Name der Playlist definiert werden kann(Achtung!, dieser muss eindeutig sein). Nun werden alle, in der Bibliothek, vorhanden";
                    txbHelp.Text += " Titel dargestellt. Diese können dann durch einen einfachen Klick zur Liste hinzugefügt werden.";
                    break;
                case "Playlist wiedergeben":
                    lbTitle.Text = "Playlist wiedergeben";
                    txbHelp.Text = "Um eine Playlist abzuspielen, muss auf die Schaltfläche 'Playlists' geklickt werden, nun werden alle vorhanden Playlists dargestellt, damit";
                    txbHelp.Text += " man nun eine Abspielen kann muss man einen Doppelklick auf die entsprechende Liste machen. Nun werden alle Titel, welche sich in der Liste";
                    txbHelp.Text += " dargestellt. Der einzelne Titel kann nun wieder durch einen Doppelklick abgespielt werden.";
                    break;
                case "Playlist wiederholen":
                    lbTitle.Text = "Playlist wiederholen";
                    txbHelp.Text = "Um eine Playlist zu wiederholen, muss man die Schaltfläche, neben der Schaltfläche um Titel zuwiederholen, klicken. Hat die Schaltfläche einen";
                    txbHelp.Text += " orangen Rahmen, so wird die aktuelle Playlist wiederholt.";
                    break;
                case "Titel zur Playlist hinzufügen":
                    lbTitle.Text = "Titel zur Playlist hinzufügen";
                    txbHelp.Text = "Zur Zeit besteht noch keine Möglichkeit einen Titel zu einer bestehnden Playlist hinzuzufügen.";
                    break;
                case "Album Wiedergabe":
                    lbTitle.Text = "Album Wiedergabe";
                    txbHelp.Text = "Um ein Album wiederzugeben, muss auf die Schaltfläche 'Alben', in der Toolbar geklickt werden. Nun werden alle Alben in der Bibliothek dargestellt.";
                    txbHelp.Text += " Um nun ein Album wiederzugeben, muss man dieses wieder doppelklicken.";
                    break;
                case "Interpret Wiedergabe":
                    lbTitle.Text = "Interpret Wiedergabe";
                    txbHelp.Text = "Um alle Lieder eines Interprets wiederzugeben, muss auf die Schaltfläche 'Interpreten' in der Toolbar geklickt werden. Nun werden alle Lieder des";
                    txbHelp.Text += " gewählten Interprets dargestellt und man kann diese Abspielen.";
                    break;
                case "Warteliste":
                    lbTitle.Text = "Warteliste";
                    txbHelp.Text = "Bei der Warteliste handelt es sich um eine Liste, welche automatisch erstellt wird, aber kein Lied beinhaltet. Wird ein Lied zur Warteliste";
                    txbHelp.Text += " hinzugefügt, so wird dieses am Ende der Liste angefügt und wird nach dem Abspielen wieder aus der Liste gelöscht.";
                    break;
            }
        }
    }
}
