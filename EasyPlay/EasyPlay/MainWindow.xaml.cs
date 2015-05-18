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
using System.Windows.Media;
using System.Windows.Threading;

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
        private Playlist Playlist;

        public MainWindow()
        {
            InitializeComponent();

            BtnPlay.Visibility = Visibility.Visible;
            BtnPause.Visibility = Visibility.Hidden;

            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden; 
            
            Player = new MediaPlayer();   
        }

        private void TitelButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Titel button is clicked.");
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
        }

        private void PlaylistsButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Playlists button is clicked.");
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Visible;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
        }

        private void AlbenButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Alben button is clicked.");
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Visible;
            ListViewInterpreten.Visibility = Visibility.Hidden;
        }

        private void InterpretenButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Interpreten button is clicked.");
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Visible;
        }

        private void WartelisteButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Warteliste button is clicked.");
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            BtnPause.Visibility = Visibility.Hidden;
            BtnPlay.Visibility = Visibility.Visible;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
            BtnPlay.Visibility = Visibility.Hidden;
            BtnPause.Visibility = Visibility.Visible;
        }

        private void BtnNeuePlaylist_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Visible;
        }

        private void WeiterButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist = new Playlist(null, InputTextBox.Text);
            InputBox.Visibility = Visibility.Collapsed;
            ListViewTitel.Visibility = Visibility.Visible;
            //Playlist.addLied();
        }

        private void AbbrechenButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
        }
    }
}
