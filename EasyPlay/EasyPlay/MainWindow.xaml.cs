using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using System.Windows.Forms;

namespace EasyPlay
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Klassenvariabeln
        private MediaPlayer Player;
        private List<Playlist> Playlists;
        private Bibliothek Biblio;
        private Lied AktuelleWiedergabe;
        private Warteliste Wartelist;
        private DispatcherTimer Timer;
        private TimeSpan Pause;

        private enum MyType
        {
            Titel, Interpret, Album, Playlists, Warteliste
        }

        public MainWindow()
        {
            InitializeComponent();

            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;

            string saveFile = AppDomain.CurrentDomain.BaseDirectory + "easyplay.txt";
            if (!File.Exists(saveFile))
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                ofd.Description = "Ordner öffnen";
                ofd.ShowNewFolderButton = false;
                bool result = false;
                result = Convert.ToBoolean(ofd.ShowDialog());
                Biblio = new Bibliothek(ofd.SelectedPath);
                this.displayData(MyType.Titel);
            }

        Player = new MediaPlayer();
            
        }

        private void TitelButton_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Titel button is clicked.");
            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void PlaylistsButton_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Playlists button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Visible;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void AlbenButton_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Alben button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Visible;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void InterpretenButton_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Interpreten button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Visible;
        }

        private void WartelisteButton_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Warteliste button is clicked.");
            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
       
        }

        private void displayData(MyType type)
        {
            switch (type)
            {
                case MyType.Titel:
                    break;
                case MyType.Album:
                    break;
                case MyType.Interpret:
                    break;
                case MyType.Playlists:
                    break;
                case MyType.Warteliste:
                    break;
            }
        }
    }
}
